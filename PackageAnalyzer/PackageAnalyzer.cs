// Decompiled with JetBrains decompiler
// Type: PackageInspector.PackageAnalyzer
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Xml;
using System.Xml.XPath;

namespace PackageInspector
{
  public class PackageAnalyzer : IComponentAnalyzer, IDisposable, IMumAnalyzer
  {
    private string packagePath;
    private bool disposed;

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public IList<WindowsComponent> Analyze(string path)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      if (path.Length == 0)
        throw new ArgumentException("'path' parameter is of 0 length");
      string empty = string.Empty;
      string packageDir;
      if (path.EndsWith(".cab", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".msu", StringComparison.OrdinalIgnoreCase))
      {
        if (!File.Exists(path))
          throw new FileNotFoundException(path);
        this.packagePath = path;
        packageDir = this.ExpandPackage();
      }
      else
      {
        if (!Directory.Exists(path))
          throw new DirectoryNotFoundException(path);
        packageDir = path;
      }
      IList<WindowsComponent> components = (IList<WindowsComponent>) new List<WindowsComponent>();
      PackageAnalyzer.ProcessManifests(packageDir, components);
      return components;
    }

    private static void ProcessManifests(string packageDir, IList<WindowsComponent> components)
    {
      FileInfo[] files = new DirectoryInfo(packageDir).GetFiles("*.manifest", SearchOption.AllDirectories);
      PackageAnalyzerTracer.Instance.Write((object) (files.Length.ToString() + " manifest(s) found"));
      foreach (FileSystemInfo fileSystemInfo in files)
      {
        foreach (WindowsComponent windowsComponent in (IEnumerable<WindowsComponent>) new ManifestAnalyzer().Analyze(fileSystemInfo.FullName))
          components.Add(windowsComponent);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (!this.disposed)
        return;
      int num = disposing ? 1 : 0;
      this.disposed = true;
    }

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    private string ExpandPackage()
    {
      FileInfo fileInfo = new FileInfo(this.packagePath);
      string str1 = ScratchPath.Instance.Folder + "\\" + fileInfo.Name;
      string str2 = str1.Substring(0, str1.Length - fileInfo.Extension.Length);
      string path = str2 + "\\MSU";
      string str3 = fileInfo.FullName;
      if (string.Compare(fileInfo.Extension, ".msu", StringComparison.OrdinalIgnoreCase) == 0)
      {
        if (!Directory.Exists(path))
        {
          Directory.CreateDirectory(path);
          PackageAnalyzer.ExecuteCommand("expand.exe \"" + str3 + "\" -F:* " + path + " >NUL");
        }
        str3 = path + "\\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + ".CAB";
      }
      string str4 = str2 + "\\CAB";
      if (!Directory.Exists(str4))
      {
        Directory.CreateDirectory(str4);
        PackageAnalyzer.ExecuteCommand("expand.exe \"" + str3 + "\" -F:* " + str4 + " >NUL");
        foreach (string file in Directory.GetFiles(str4, "*.cab", SearchOption.TopDirectoryOnly))
          PackageAnalyzer.ExpandPackage(file, str4);
      }
      return str4;
    }

    private static string ExpandPackage(string packageCab, string destinationFolder)
    {
      FileInfo fileInfo = new FileInfo(packageCab);
      string str1 = destinationFolder + "\\" + fileInfo.Name;
      string str2 = str1.Substring(0, str1.Length - fileInfo.Extension.Length);
      string fullName = fileInfo.FullName;
      if (!Directory.Exists(str2))
        Directory.CreateDirectory(str2);
      PackageAnalyzer.ExecuteCommand("expand.exe \"" + fullName + "\" -F:* " + str2 + " >NUL");
      foreach (string file in Directory.GetFiles(str2, "*.cab", SearchOption.TopDirectoryOnly))
        PackageAnalyzer.ExpandPackage(file, str2);
      return str2;
    }

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    private static int ExecuteCommand(string command)
    {
      Process process = Process.Start(new ProcessStartInfo("cmd.exe", "/C " + command)
      {
        CreateNoWindow = true,
        UseShellExecute = false
      });
      process.WaitForExit();
      int exitCode = process.ExitCode;
      process.Close();
      return exitCode;
    }

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public IList<MumElement> AnalyzeMum(string packageName)
    {
      if (packageName == null)
        throw new ArgumentNullException("packageName");
      if (packageName.Length == 0)
        throw new ArgumentException("'file' parameter is of 0 length");
      string empty = string.Empty;
      string path;
      if (packageName.EndsWith(".cab", StringComparison.OrdinalIgnoreCase) || packageName.EndsWith(".msu", StringComparison.OrdinalIgnoreCase))
      {
        if (!File.Exists(packageName))
          throw new FileNotFoundException(packageName);
        this.packagePath = packageName;
        path = this.ExpandPackage();
      }
      else
      {
        if (!Directory.Exists(packageName))
          throw new DirectoryNotFoundException(packageName);
        path = packageName;
      }
      string[] files = Directory.GetFiles(path, "*.mum", SearchOption.AllDirectories);
      if (files == null)
        throw new PackageAnalyzerException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} is not a valid CBS package", new object[1]
        {
          (object) packageName
        }));
      if (files.Length == 0)
        throw new PackageAnalyzerException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} is not a valid CBS package", new object[1]
        {
          (object) packageName
        }));
      IList<MumElement> mumElementList = (IList<MumElement>) new List<MumElement>();
      foreach (string fileName in files)
      {
        FileInfo fileInfo = new FileInfo(fileName);
        using (StreamReader reader = fileInfo.OpenText())
        {
          try
          {
            XmlNamespaceManager namespaces;
            MumElement mumElement = new MumElement((IXPathNavigable) PackageAnalyzer.LoadPackageXml(reader, out namespaces).DocumentElement, fileInfo.FullName);
            mumElementList.Add(mumElement);
          }
          catch (XmlException ex)
          {
            PackageAnalyzerTracer.Instance.Write((object) (ex.ToString() + Environment.NewLine));
            throw new PackageAnalyzerException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} is not a valid CBS package", new object[1]
            {
              (object) packageName
            }));
          }
        }
      }
      return mumElementList;
    }

    private static XmlDocument LoadPackageXml(StreamReader reader, out XmlNamespaceManager namespaces)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load((TextReader) reader);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string uri = (string) null;
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) documentElement.Attributes)
      {
        if (attribute.Value.ToUpperInvariant().Contains("SCHEMAS-MICROSOFT-COM"))
          uri = attribute.Value;
      }
      namespaces = new XmlNamespaceManager(xmlDocument.NameTable);
      namespaces.AddNamespace("ns", uri);
      return xmlDocument;
    }
  }
}
