// Decompiled with JetBrains decompiler
// Type: PackageInspector.ImageAnalyzer
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace PackageInspector
{
  public class ImageAnalyzer : IComponentAnalyzer, IDisposable
  {
    private IList<WindowsComponent> components = (IList<WindowsComponent>) new List<WindowsComponent>();
    private bool disposed;

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public IList<WindowsComponent> Analyze(string path)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      if (path.Length == 0)
        throw new ArgumentException("'path' parameter is of 0 length");
      if (!Directory.Exists(path))
        throw new DirectoryNotFoundException(path);
      this.ProcessManifest(path);
      return this.components;
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
    private void ProcessManifest(string windowsPath)
    {
      FileInfo[] files = new DirectoryInfo(windowsPath + "\\winsxs\\Manifests").GetFiles("*.manifest", SearchOption.AllDirectories);
      PackageAnalyzerTracer.Instance.Write((object) (files.Length.ToString() + " manifest(s) found"));
      int num = 0;
      foreach (FileInfo fileInfo in files)
      {
        PackageAnalyzerTracer.Instance.Write((object) (num + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture));
        ++num;
        foreach (WindowsComponent windowsComponent in (IEnumerable<WindowsComponent>) new ManifestAnalyzer().Analyze(fileInfo.FullName))
          this.components.Add(windowsComponent);
      }
    }
  }
}
