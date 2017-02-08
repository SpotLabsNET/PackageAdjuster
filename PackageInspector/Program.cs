// Decompiled with JetBrains decompiler
// Type: PackageInspector.Program
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace PackageInspector
{
  internal static class Program
  {
    public const string Title = "CBS Package Inspector";

    [STAThread]
    private static int Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      PackageViewer packageViewer = new PackageViewer();
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      if (commandLineArgs.Length == 2 && (string.Compare(commandLineArgs[1], "/?", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(commandLineArgs[1], "/h", StringComparison.OrdinalIgnoreCase) == 0 || (string.Compare(commandLineArgs[1], "/help", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(commandLineArgs[1], "-?", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(commandLineArgs[1], "-h", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(commandLineArgs[1], "-help", StringComparison.OrdinalIgnoreCase) == 0)))
      {
        int num = (int) MessageBox.Show(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Usage:\r\nPackageInspector.exe [optional file or folder paths]\r\nExample:\r\nPackageInspector.exe C:\\component.man D:\\packages E:\\Windows-x86.cab C:\\kb\\update.mum", new object[0]), "CBS Package Inspector", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        return 0;
      }
      if (commandLineArgs.Length >= 2)
      {
        IList<string> filesToOpen = (IList<string>) new List<string>();
        for (int index = 1; index < commandLineArgs.Length; ++index)
          filesToOpen.Add(commandLineArgs[index]);
        int filesToLoad = Program.ParseFilesToLoad(packageViewer, filesToOpen);
        if (filesToLoad != 0)
          return filesToLoad;
      }
      Application.Run((Form) packageViewer);
      return 0;
    }

    private static int ParseFilesToLoad(PackageViewer packageViewer, IList<string> filesToOpen)
    {
      foreach (string path in (IEnumerable<string>) filesToOpen)
      {
        if (path.EndsWith(".cab", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".msu", StringComparison.OrdinalIgnoreCase) || (path.EndsWith(".man", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".manifest", StringComparison.OrdinalIgnoreCase)) || path.EndsWith("update.mum", StringComparison.OrdinalIgnoreCase))
        {
          if (!File.Exists(path))
          {
            int num = (int) MessageBox.Show(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Error. {0} does not exist.", new object[1]
            {
              (object) path
            }), "CBS Package Inspector", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            return -1;
          }
          packageViewer.InitialFileList.Add(path);
        }
        else
        {
          if (!Directory.Exists(path))
          {
            string text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Error. {0} does not exist.", new object[1]
            {
              (object) path
            });
            if (File.Exists(path))
              text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Error. {0} cannot be opened. Valid file extensions are .cab, .msu, .manifest and .man", new object[1]
              {
                (object) path
              });
            int num = (int) MessageBox.Show(text, "CBS Package Inspector", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            return -1;
          }
          string[] files1 = Directory.GetFiles(path, "*.cab", SearchOption.AllDirectories);
          string[] files2 = Directory.GetFiles(path, "*.msu", SearchOption.AllDirectories);
          string[] files3 = Directory.GetFiles(path, "*.man", SearchOption.AllDirectories);
          string[] files4 = Directory.GetFiles(path, "*.manifest", SearchOption.AllDirectories);
          foreach (string str in files1)
            packageViewer.InitialFileList.Add(str);
          foreach (string str in files2)
            packageViewer.InitialFileList.Add(str);
          foreach (string str in files3)
          {
            if (!str.EndsWith(".manifest", StringComparison.OrdinalIgnoreCase))
              packageViewer.InitialFileList.Add(str);
          }
          foreach (string str in files4)
            packageViewer.InitialFileList.Add(str);
        }
      }
      return 0;
    }
  }
}
