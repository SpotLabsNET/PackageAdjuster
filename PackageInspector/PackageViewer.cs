// Decompiled with JetBrains decompiler
// Type: PackageInspector.PackageViewer
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using PackageInspector.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace PackageInspector
{
  internal class PackageViewer : Form
  {
    private PackageAnalyzer packageAnalyzer = new PackageAnalyzer();
    private PackageAnalyzerTracer tracer = PackageAnalyzerTracer.Instance;
    private IComponentAnalyzer manifestAnalyzer = (IComponentAnalyzer) new ManifestAnalyzer();
    private IDictionary<string, IList<WindowsComponent>> packageAndComponentMap = (IDictionary<string, IList<WindowsComponent>>) new Dictionary<string, IList<WindowsComponent>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private IList<string> removedPackageList = (IList<string>) new List<string>();
    private IDictionary<string, IList<MumElement>> packageAndMumMap = (IDictionary<string, IList<MumElement>>) new Dictionary<string, IList<MumElement>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private string logFile = string.Empty;
    private FindDialog findDialog = new FindDialog();
    private bool consolidatedView;
    private bool inspectPackage;
    private PleaseWaitDialog pleaseWaitDialog;
    private bool workDone;
    private IContainer components;
    private MenuStrip mainMenuStrip;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem helpToolStripMenuItem;
    private SplitContainer packageSplitContainer;
    private TreeView packageTreeView;
    private TabControl featureSetDetailsTabControl;
    private TabPage tabPageComponents;
    private ToolStripMenuItem exitToolStripMenuItem;
    private ToolStrip mainToolStrip;
    private ToolStripSeparator toolStripSeparator3;
    private SaveFileDialog exportFileDialog;
    private OpenFileDialog openFileDialog;
    private ImageList imageList;
    private StatusStrip mainStatusStrip;
    private ToolStripStatusLabel toolStripStatusLabelPleaseWait;
    private TabPage tabPageOutput;
    private RichTextBox outputRichTextBox;
    private ToolStripMenuItem openToolStripMenuItem;
    private ToolStripMenuItem cbsPackagesMenuItem;
    private ToolStripMenuItem folderContainingCBSPackagesMenuItem;
    private ToolStripDropDownButton openToolStripButton;
    private ToolStripMenuItem folderContainingCBSPackagesToolStripMenuItem;
    private ToolStripMenuItem cbsPackagesToolStripMenuItem;
    private FolderBrowserDialog folderBrowserDialog;
    private ToolStripMenuItem inspectToolStripMenuItem;
    private ToolStripButton inspectToolStripButton;
    private ContextMenuStrip packagesContextMenuStrip;
    private ToolStripMenuItem inspectToolStripContextMenuItem;
    private ToolStripMenuItem aboutToolStripMenuItem;
    private BackgroundWorker backgroundWorker;
    private ToolStripButton toolStripButtonExport;
    private TabPage tabPageApplicability;
    private ToolStripMenuItem scratchPathToolStripContextMenuItem;
    private ToolStripMenuItem toolStripMenuItemUsage;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem viewToolStripMenuItem;
    private ToolStripMenuItem itemsPerComponentToolStripMenuItem;
    private ToolStripMenuItem anAlreadyExpandedCBSPackageMenuItem;
    private ToolStripMenuItem anAlreadyExpandedCBSPackageToolStripMenuItem;
    private OpenFileDialog openUpdateMumFileDialog;
    private ToolStripButton toolStripButtonFind;
    private ToolStripMenuItem findToolStripMenuItem;
    private SplitContainer componentSplitContainer;
    private ToolStripMenuItem removeToolStripMenuItem;
    private ContextMenuStrip commonContextMenuStrip;
    private ToolStripMenuItem copyTextContextMenuStripMenuItem;
    private ToolStripMenuItem copyTextToolStripMenuItem;
    private MumTreeListControl mumTreeListControl;
    private TabControl componentDetailsTabControl;
    private TabPage tabPageFiles;
    private TabPage tabPageRegistryKeys;
    private TabPage tabPageDependencies;
    private TabPage tabPageSmiSettings;
    private TabPage tabPageGenericCommands;
    private SmiSettingsTreeListControl smiSettingsTreeListControl;
    private GenericCommandsTreeListControl genericCommandsTreeListControl;
    private RegistryKeyValueControl registryKeyValueControl;
    private FileListControl fileListControl;
    private DependencyListControl dependencyListControl;
    private ComponentListControl componentListControl;
    private ToolStripMenuItem exportToolStripMenuItem;

    public IList<string> InitialFileList { get; private set; }

    public PackageViewer()
    {
      this.InitializeComponent();
      this.componentListControl.SelectionChanged += new EventHandler<ComponentEventArgs>(this.ComponentListControl_SelectionChanged);
      this.InitializeLogFile();
      this.InitialFileList = (IList<string>) new List<string>();
    }

    private void ComponentListControl_SelectionChanged(object sender, ComponentEventArgs e)
    {
      if (this.consolidatedView)
        return;
      this.ClearComponentRelatedViews();
      this.UpdateComponentRelatedViews(e.Component);
    }

    private void UpdateComponentRelatedViews(WindowsComponent windowsComponent)
    {
      this.UpdateFileView(windowsComponent);
      this.UpdateRegistryKeyView(windowsComponent);
      this.UpdateDependencyView(windowsComponent);
      this.UpdateSmiSettingsView(windowsComponent);
      this.UpdateGenericCommandsView(windowsComponent);
    }

    private void InitializeLogFile()
    {
      this.tracer.Info += new EventHandler<PackageAnalyzerInfoEventArgs>(this.Tracer_Info);
      if (!Directory.Exists(ScratchPath.Instance.Folder))
        Directory.CreateDirectory(ScratchPath.Instance.Folder);
      this.logFile = ScratchPath.Instance.Folder + "\\" + Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location) + ".log";
    }

    private void Tracer_Info(object sender, PackageAnalyzerInfoEventArgs e)
    {
      this.Log(e.Message);
      this.AppendOutputTextBox(e.Message);
    }

    private void PackagesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      bool flag = e.Node.Level == 1;
      this.inspectToolStripMenuItem.Enabled = flag;
      this.inspectToolStripContextMenuItem.Enabled = flag;
      this.scratchPathToolStripContextMenuItem.Enabled = flag;
      this.removeToolStripMenuItem.Enabled = flag;
      this.inspectToolStripButton.Enabled = flag;
      this.UpdateComponentsListView();
      this.UpdatePackageApplicabilityViews();
    }

    private void UpdatePackageApplicabilityViews()
    {
      bool flag = false;
      string text = string.Empty;
      this.mumTreeListControl.ClearView();
      string tag = (string) this.packageTreeView.SelectedNode.Tag;
      if (tag == null)
        return;
      switch (PackageViewer.GetFileType(tag))
      {
        case FileType.Cab:
        case FileType.Msu:
        case FileType.ExpandedPackage:
          if (!this.packageAndMumMap.ContainsKey(tag) && !this.inspectPackage)
            break;
          this.UseWaitCursor = true;
          try
          {
            IList<MumElement> mumElements = this.packageAnalyzer.AnalyzeMum(tag);
            this.UpdatePackageApplicabilityTreeView(tag, mumElements);
          }
          catch (PackageAnalyzerException ex)
          {
            flag = true;
            text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} is not a valid CBS package", new object[1]
            {
              (object) tag
            });
          }
          this.UseWaitCursor = false;
          if (!flag)
            break;
          int num = (int) MessageBox.Show((IWin32Window) this, text, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
          break;
      }
    }

    private void UpdatePackageApplicabilityTreeView(string filePath, IList<MumElement> mumElements)
    {
      if (!this.packageAndMumMap.ContainsKey(filePath))
        this.packageAndMumMap.Add(filePath, mumElements);
      this.mumTreeListControl.Data = (object) mumElements;
      this.mumTreeListControl.UpdateView();
    }

    private void UpdateComponentsListView()
    {
      bool flag = false;
      string text = string.Empty;
      this.componentListControl.ClearView();
      string tag = (string) this.packageTreeView.SelectedNode.Tag;
      if (tag == null)
        return;
      FileType fileType = PackageViewer.GetFileType(tag);
      if (!this.packageAndComponentMap.ContainsKey(tag) && !this.inspectPackage)
      {
        this.ClearComponentRelatedViews();
      }
      else
      {
        this.UseWaitCursor = true;
        this.toolStripStatusLabelPleaseWait.Visible = true;
        this.workDone = false;
        int num1 = 24;
        IList<WindowsComponent> windowsComponentList;
        if (fileType == FileType.Cab || fileType == FileType.Msu || fileType == FileType.ExpandedPackage)
        {
          this.backgroundWorker.RunWorkerAsync();
          try
          {
            windowsComponentList = this.packageAnalyzer.Analyze(tag);
          }
          catch (PackageAnalyzerException ex)
          {
            text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Error processing {0}", new object[1]
            {
              (object) tag
            });
            windowsComponentList = (IList<WindowsComponent>) new List<WindowsComponent>();
            flag = true;
          }
          if (fileType == FileType.Msu)
            num1 = 25;
          if (fileType == FileType.ExpandedPackage)
            num1 = 31;
        }
        else
        {
          this.backgroundWorker.RunWorkerAsync();
          try
          {
            windowsComponentList = this.manifestAnalyzer.Analyze(tag);
          }
          catch (PackageAnalyzerException ex)
          {
            text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Error processing {0}", new object[1]
            {
              (object) tag
            });
            windowsComponentList = (IList<WindowsComponent>) new List<WindowsComponent>();
            flag = true;
          }
          num1 = 26;
        }
        this.packageTreeView.SelectedNode.ImageIndex = num1;
        this.packageTreeView.SelectedNode.SelectedImageIndex = num1;
        if (!this.packageAndComponentMap.ContainsKey(tag))
          this.packageAndComponentMap.Add(tag, windowsComponentList);
        this.componentListControl.Data = (object) windowsComponentList;
        this.componentListControl.UpdateView();
        this.ClearComponentRelatedViews();
        if (this.consolidatedView)
          this.UpdateConsolidatedComponentViews();
        else
          this.UpdateComponentRelatedViews();
        this.workDone = true;
        this.UseWaitCursor = false;
        this.toolStripStatusLabelPleaseWait.Visible = false;
        if (!flag)
          return;
        int num2 = (int) MessageBox.Show((IWin32Window) this, text, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
    }

    private static FileType GetFileType(string filePath)
    {
      if (filePath.EndsWith(".CAB", StringComparison.OrdinalIgnoreCase))
        return FileType.Cab;
      if (filePath.EndsWith(".MSU", StringComparison.OrdinalIgnoreCase))
        return FileType.Msu;
      return filePath.EndsWith(".MAN", StringComparison.OrdinalIgnoreCase) || filePath.EndsWith(".MANIFEST", StringComparison.OrdinalIgnoreCase) ? FileType.Manifest : FileType.ExpandedPackage;
    }

    private void ClearComponentRelatedViews()
    {
      this.fileListControl.ClearView();
      this.genericCommandsTreeListControl.ClearView();
      this.smiSettingsTreeListControl.ClearView();
      this.registryKeyValueControl.ClearView();
      this.dependencyListControl.ClearView();
    }

    private void UpdateConsolidatedComponentViews()
    {
      string tag = (string) this.packageTreeView.SelectedNode.Tag;
      if (tag == null)
        return;
      foreach (WindowsComponent component in (IEnumerable<WindowsComponent>) this.packageAndComponentMap[tag])
      {
        this.UpdateFileView(component);
        this.UpdateRegistryKeyView(component);
        this.UpdateDependencyView(component);
        this.UpdateSmiSettingsView(component);
        this.UpdateGenericCommandsView(component);
      }
    }

    private void UpdateComponentRelatedViews()
    {
      string tag = (string) this.packageTreeView.SelectedNode.Tag;
      if (tag == null)
        return;
      WindowsComponent selectedComponent = this.componentListControl.SelectedComponent;
      foreach (WindowsComponent component in (IEnumerable<WindowsComponent>) this.packageAndComponentMap[tag])
      {
        if (string.Compare(selectedComponent.AssemblyIdentity.Name, component.AssemblyIdentity.Name, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(selectedComponent.AssemblyIdentity.Architecture, component.AssemblyIdentity.Architecture, StringComparison.OrdinalIgnoreCase) == 0 && (string.Compare(selectedComponent.AssemblyIdentity.Version, component.AssemblyIdentity.Version, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(selectedComponent.AssemblyIdentity.Language, component.AssemblyIdentity.Language, StringComparison.OrdinalIgnoreCase) == 0) && string.Compare(selectedComponent.AssemblyIdentity.VersionScope, component.AssemblyIdentity.VersionScope, StringComparison.OrdinalIgnoreCase) == 0)
        {
          this.UpdateFileView(component);
          this.UpdateRegistryKeyView(component);
          this.UpdateDependencyView(component);
          this.UpdateSmiSettingsView(component);
          this.UpdateGenericCommandsView(component);
          break;
        }
      }
    }

    private void UpdateFileView(WindowsComponent component)
    {
      this.fileListControl.Data = (object) component;
      this.fileListControl.UpdateView();
    }

    private void UpdateRegistryKeyView(WindowsComponent component)
    {
      this.registryKeyValueControl.Data = (object) component;
      this.registryKeyValueControl.UpdateView();
    }

    private void UpdateDependencyView(WindowsComponent component)
    {
      this.dependencyListControl.Data = (object) component;
      this.dependencyListControl.UpdateView();
    }

    private void UpdateSmiSettingsView(WindowsComponent component)
    {
      this.smiSettingsTreeListControl.Data = (object) component;
      this.smiSettingsTreeListControl.UpdateView();
    }

    private void UpdateGenericCommandsView(WindowsComponent component)
    {
      this.genericCommandsTreeListControl.Data = (object) component;
      this.genericCommandsTreeListControl.UpdateView();
    }

    private void AppendOutputTextBox(string text)
    {
      if (this.outputRichTextBox.InvokeRequired)
        this.Invoke((Delegate) new PackageViewer.SetTextCallback(this.AppendOutputTextBox), (object) text);
      else
        this.outputRichTextBox.AppendText(text + Environment.NewLine);
    }

    private static void ValidationEventHandler(object sender, ValidationEventArgs e)
    {
      switch (e.Severity)
      {
        case XmlSeverityType.Error:
          throw new XmlException(e.Message, (Exception) e.Exception);
      }
    }

    private void Log(string message)
    {
      if (message == null)
        return;
      using (StreamWriter streamWriter = new StreamWriter(this.logFile, true))
      {
        DateTime now = DateTime.Now;
        string str = now.ToShortDateString() + ", " + now.ToShortTimeString() + "\t" + message;
        streamWriter.WriteLine(str);
      }
    }

    private void SelectPackages(IList<string> fileList)
    {
      foreach (string file in (IEnumerable<string>) fileList)
      {
        FileInfo fileInfo = new FileInfo(file);
        if (string.Compare(fileInfo.Extension, ".cab", StringComparison.OrdinalIgnoreCase) == 0)
        {
          int num = 19;
          this.packageTreeView.Nodes[0].Nodes.Add(new TreeNode(fileInfo.Name, num, num)
          {
            Tag = (object) fileInfo.FullName.ToUpperInvariant(),
            Name = fileInfo.Name.ToUpperInvariant()
          });
        }
        else if (string.Compare(fileInfo.Extension, ".msu", StringComparison.OrdinalIgnoreCase) == 0)
        {
          int num = 20;
          this.packageTreeView.Nodes[0].Nodes.Add(new TreeNode(fileInfo.Name, num, num)
          {
            Tag = (object) fileInfo.FullName.ToUpperInvariant(),
            Name = fileInfo.Name.ToUpperInvariant()
          });
        }
        else if (string.Compare(fileInfo.Extension, ".manifest", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(fileInfo.Extension, ".man", StringComparison.OrdinalIgnoreCase) == 0)
        {
          int num = 21;
          this.packageTreeView.Nodes[0].Nodes.Add(new TreeNode(fileInfo.Name, num, num)
          {
            Tag = (object) fileInfo.FullName.ToUpperInvariant(),
            Name = fileInfo.Name.ToUpperInvariant()
          });
        }
        else if (string.Compare(fileInfo.Name, "update.mum", StringComparison.OrdinalIgnoreCase) == 0)
        {
          int num = 30;
          this.packageTreeView.Nodes[0].Nodes.Add(new TreeNode(fileInfo.Directory.Name, num, num)
          {
            Tag = (object) fileInfo.DirectoryName.ToUpperInvariant(),
            Name = fileInfo.Directory.Name.ToUpperInvariant()
          });
        }
      }
    }

    private void CbsPackagesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.OpenCBSPackages();
    }

    private void FolderContainingCBSPackagesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.OpenFolderContainingCBSPackages();
    }

    private void OpenCBSPackages()
    {
      if (this.openFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.SelectPackages((IList<string>) new List<string>((IEnumerable<string>) this.openFileDialog.FileNames));
      this.packageTreeView.Nodes[0].Expand();
    }

    private void OpenFolderContainingCBSPackages()
    {
      if (this.folderBrowserDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string[] files1 = Directory.GetFiles(this.folderBrowserDialog.SelectedPath, "*.cab", SearchOption.AllDirectories);
      string[] files2 = Directory.GetFiles(this.folderBrowserDialog.SelectedPath, "*.msu", SearchOption.AllDirectories);
      string[] files3 = Directory.GetFiles(this.folderBrowserDialog.SelectedPath, "*.man", SearchOption.AllDirectories);
      string[] files4 = Directory.GetFiles(this.folderBrowserDialog.SelectedPath, "*.manifest", SearchOption.AllDirectories);
      IList<string> fileList = (IList<string>) new List<string>();
      foreach (string str in files1)
        fileList.Add(str);
      foreach (string str in files2)
        fileList.Add(str);
      foreach (string str in files3)
        fileList.Add(str);
      foreach (string str in files4)
        fileList.Add(str);
      this.SelectPackages(fileList);
      this.packageTreeView.Nodes[0].Expand();
    }

    private void InspectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.InspectPackage();
    }

    private void InspectPackage()
    {
      this.inspectPackage = true;
      this.UpdateComponentsListView();
      this.UpdatePackageApplicabilityViews();
      this.inspectPackage = false;
    }

    private void InspectToolStripButton_Click(object sender, EventArgs e)
    {
      this.InspectPackage();
    }

    private void InspectToolStripContextMenuItem_Click(object sender, EventArgs e)
    {
      this.InspectPackage();
    }

    private void CbsPackagesMenuItem_Click(object sender, EventArgs e)
    {
      this.OpenCBSPackages();
    }

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int num = (int) new AboutBox().ShowDialog((IWin32Window) this);
    }

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      this.ShowPleaseWaitDialog();
    }

    private void ShowPleaseWaitDialog()
    {
      if (this.pleaseWaitDialog == null)
        this.pleaseWaitDialog = new PleaseWaitDialog();
      if (this.pleaseWaitDialog.InvokeRequired)
      {
        this.Invoke((Delegate) new PackageViewer.ShowWaitDialogCallback(this.ShowPleaseWaitDialog));
      }
      else
      {
        this.pleaseWaitDialog.Show();
        while (!this.workDone)
        {
          Thread.Sleep(100);
          Application.DoEvents();
        }
        this.pleaseWaitDialog.Close();
        this.pleaseWaitDialog.Dispose();
        this.pleaseWaitDialog = (PleaseWaitDialog) null;
      }
    }

    private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.ExportInspectedPackagesToXml();
    }

    private void ExportToolStripButton_Click(object sender, EventArgs e)
    {
      this.ExportInspectedPackagesToXml();
    }

    private void ExportInspectedPackagesToXml()
    {
      if (this.exportFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string fileName = this.exportFileDialog.FileName;
      try
      {
        if (File.Exists(fileName))
          File.Delete(fileName);
      }
      catch (IOException ex)
      {
        PackageAnalyzerTracer.Instance.Write((object) ex.ToString());
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Unable to export file {0}", new object[1]
        {
          (object) fileName
        }), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        return;
      }
      XmlDocument xmlDocument = new XmlDocument();
      XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
      xmlDocument.AppendChild((XmlNode) xmlDeclaration);
      XmlElement element1 = xmlDocument.CreateElement("Servicing");
      foreach (string key in (IEnumerable<string>) this.packageAndComponentMap.Keys)
      {
        XmlElement element2 = xmlDocument.CreateElement("cbsItem");
        FileInfo fileInfo = new FileInfo(key);
        XmlElement element3 = xmlDocument.CreateElement("path");
        element3.InnerText = fileInfo.FullName;
        element2.AppendChild((XmlNode) element3);
        element2.SetAttribute("name", fileInfo.Name);
        string empty = string.Empty;
        string str = key.EndsWith(".CAB", StringComparison.OrdinalIgnoreCase) || key.EndsWith(".MSU", StringComparison.OrdinalIgnoreCase) ? "package" : (key.EndsWith(".MANIFEST", StringComparison.OrdinalIgnoreCase) || key.EndsWith(".MAN", StringComparison.OrdinalIgnoreCase) ? "manifest" : "expandedPackage");
        element2.SetAttribute("type", str);
        foreach (WindowsComponent windowsComponent in (IEnumerable<WindowsComponent>) this.packageAndComponentMap[key])
        {
          XmlElement element4 = xmlDocument.CreateElement("component");
          element4.SetAttribute("name", windowsComponent.AssemblyIdentity.Name);
          element4.SetAttribute("version", windowsComponent.AssemblyIdentity.Version);
          element4.SetAttribute("processorArchitecture", windowsComponent.AssemblyIdentity.Architecture);
          element4.SetAttribute("language", windowsComponent.AssemblyIdentity.Language);
          element2.AppendChild((XmlNode) element4);
        }
        element1.AppendChild((XmlNode) element2);
      }
      xmlDocument.AppendChild((XmlNode) element1);
      try
      {
        xmlDocument.Save(fileName);
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Inspected CBS packages successfully exported to {0}", new object[1]
        {
          (object) fileName
        }), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      }
      catch (XmlException ex)
      {
        PackageAnalyzerTracer.Instance.Write((object) ex.ToString());
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Unable to export file {0}", new object[1]
        {
          (object) fileName
        }), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      PackageAnalyzerTracer.Instance.Write((object) "background processing completed");
    }

    private void ScratchPathToolStripContextMenuItem_Click(object sender, EventArgs e)
    {
      string tag = (string) this.packageTreeView.SelectedNode.Tag;
      if (tag == null)
        return;
      FileType fileType = PackageViewer.GetFileType(tag);
      string empty = string.Empty;
      string str = fileType != FileType.Manifest ? (fileType != FileType.ExpandedPackage ? (!this.packageAndComponentMap.ContainsKey(tag) ? Path.GetDirectoryName(tag) : ScratchPath.Instance.Folder + "\\" + Path.GetFileNameWithoutExtension(tag)) : tag) : Path.GetDirectoryName(tag);
      if (!Directory.Exists(str))
        return;
      Process.Start("explorer.exe", str);
    }

    private void PackageViewer_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.CloseViewer())
        e.Cancel = false;
      else
        e.Cancel = true;
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private bool CloseViewer()
    {
      bool flag = true;
      if (bool.Parse(ConfigurationSettings.AppSettings["showCloseConfirmDialog"]) && (this.packageTreeView.Nodes[0].Nodes.Count > 0 || this.removedPackageList.Count > 0))
      {
        CloseConfirm closeConfirm = new CloseConfirm();
        closeConfirm.EnableCleanup = this.packageAndComponentMap.Count > 0 || this.removedPackageList.Count > 0;
        if (closeConfirm.ShowDialog((IWin32Window) this) != DialogResult.Yes)
          return false;
        flag = closeConfirm.EnableCleanup;
      }
      this.packageAndComponentMap.Clear();
      this.packageAnalyzer.Dispose();
      this.manifestAnalyzer.Dispose();
      string text = string.Empty;
      if (!flag)
        return true;
      try
      {
        this.UseWaitCursor = true;
        foreach (string file in Directory.GetFiles(ScratchPath.Instance.Folder, "*", SearchOption.AllDirectories))
        {
          FileAttributes fileAttributes = File.GetAttributes(file) & ~(FileAttributes.ReadOnly | FileAttributes.Archive);
          File.SetAttributes(file, fileAttributes);
        }
        Directory.Delete(ScratchPath.Instance.Folder, true);
      }
      catch (IOException ex)
      {
        text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Unable to clean up the scratch directory {0}{1}Error message: {2}{3}{4}Would you like to open this folder to manually clean it up?", (object) ScratchPath.Instance.Folder, (object) Environment.NewLine, (object) ex.Message, (object) Environment.NewLine, (object) Environment.NewLine);
      }
      catch (UnauthorizedAccessException ex)
      {
        text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Unable to clean up the scratch directory {0}{1}Error message: {2}{3}{4}Would you like to open this folder to manually clean it up?", (object) ScratchPath.Instance.Folder, (object) Environment.NewLine, (object) ex.Message, (object) Environment.NewLine, (object) Environment.NewLine);
      }
      finally
      {
        this.UseWaitCursor = false;
      }
      if (text.Length > 0 && MessageBox.Show((IWin32Window) this, text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        Process.Start("explorer.exe", ScratchPath.Instance.Folder);
      return true;
    }

    private void ToolStripMenuItemUsage_Click(object sender, EventArgs e)
    {
      string appSetting1 = ConfigurationSettings.AppSettings["helpPage"];
      string appSetting2 = ConfigurationSettings.AppSettings["helpPageViewerPath"];
      if (!File.Exists(appSetting1))
        return;
      try
      {
        Process.Start(appSetting2, appSetting1);
      }
      catch (Win32Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
    }

    private void SortListViewColumn(object sender, ColumnClickEventArgs e)
    {
      if (sender == null)
        return;
      ListView listView = sender as ListView;
      if (listView == null || listView.ListViewItemSorter == null)
        return;
      ListViewColumnSorter listViewItemSorter = listView.ListViewItemSorter as ListViewColumnSorter;
      if (listViewItemSorter == null || e.Column == 0)
        return;
      if (e.Column == listViewItemSorter.SortColumn)
      {
        if (listViewItemSorter.Order == SortOrder.Ascending)
          listViewItemSorter.Order = SortOrder.Descending;
        else if (listViewItemSorter.Order == SortOrder.Descending)
          listViewItemSorter.Order = SortOrder.Ascending;
      }
      else
      {
        listViewItemSorter.SortColumn = e.Column;
        listViewItemSorter.Order = SortOrder.Ascending;
      }
      listView.Sort();
    }

    private void PackageViewer_Load(object sender, EventArgs e)
    {
      if (this.InitialFileList.Count > 0)
        this.SelectPackages(this.InitialFileList);
      this.packageTreeView.Nodes[0].Expand();
    }

    private void ItemsPerComponentToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SwitchConsolidatedView();
    }

    private void SwitchConsolidatedView()
    {
      this.consolidatedView = !this.itemsPerComponentToolStripMenuItem.Checked;
      this.UpdateComponentsListView();
      this.UpdatePackageApplicabilityViews();
      this.SuspendLayout();
      if (this.consolidatedView)
      {
        this.componentDetailsTabControl.TabPages.Remove(this.tabPageFiles);
        this.componentDetailsTabControl.TabPages.Remove(this.tabPageRegistryKeys);
        this.componentDetailsTabControl.TabPages.Remove(this.tabPageDependencies);
        this.componentDetailsTabControl.TabPages.Remove(this.tabPageSmiSettings);
        this.componentDetailsTabControl.TabPages.Remove(this.tabPageGenericCommands);
        this.featureSetDetailsTabControl.TabPages.Add(this.tabPageFiles);
        this.featureSetDetailsTabControl.TabPages.Add(this.tabPageRegistryKeys);
        this.featureSetDetailsTabControl.TabPages.Add(this.tabPageDependencies);
        this.featureSetDetailsTabControl.TabPages.Add(this.tabPageSmiSettings);
        this.featureSetDetailsTabControl.TabPages.Add(this.tabPageGenericCommands);
        this.componentSplitContainer.Panel2Collapsed = true;
      }
      else
      {
        this.featureSetDetailsTabControl.TabPages.Remove(this.tabPageFiles);
        this.featureSetDetailsTabControl.TabPages.Remove(this.tabPageRegistryKeys);
        this.featureSetDetailsTabControl.TabPages.Remove(this.tabPageDependencies);
        this.featureSetDetailsTabControl.TabPages.Remove(this.tabPageSmiSettings);
        this.featureSetDetailsTabControl.TabPages.Remove(this.tabPageGenericCommands);
        this.componentDetailsTabControl.TabPages.Add(this.tabPageFiles);
        this.componentDetailsTabControl.TabPages.Add(this.tabPageRegistryKeys);
        this.componentDetailsTabControl.TabPages.Add(this.tabPageDependencies);
        this.componentDetailsTabControl.TabPages.Add(this.tabPageSmiSettings);
        this.componentDetailsTabControl.TabPages.Add(this.tabPageGenericCommands);
        this.componentSplitContainer.Panel2Collapsed = false;
      }
      this.ResumeLayout();
    }

    private void AnAlreadyExpandedCBSPackageMenuItem_Click(object sender, EventArgs e)
    {
      this.OpenAnAlreadyExpandedCBSPackage();
    }

    private void AnAlreadyExpandedCBSPackageToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.OpenAnAlreadyExpandedCBSPackage();
    }

    private void OpenAnAlreadyExpandedCBSPackage()
    {
      if (this.openUpdateMumFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.SelectPackages((IList<string>) new List<string>((IEnumerable<string>) this.openUpdateMumFileDialog.FileNames));
      this.packageTreeView.Nodes[0].Expand();
    }

    private void PackageViewer_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyData == (Keys.O | Keys.Control))
        this.OpenCBSPackages();
      else if (e.KeyData == (Keys.O | Keys.Control | Keys.Alt))
        this.OpenFolderContainingCBSPackages();
      else if (e.KeyData == (Keys.O | Keys.Shift | Keys.Control))
        this.OpenAnAlreadyExpandedCBSPackage();
      else if (e.KeyData == (Keys.E | Keys.Control))
        this.ExportInspectedPackagesToXml();
      else if (e.KeyData == (Keys.I | Keys.Control))
      {
        this.InspectPackage();
      }
      else
      {
        if (e.KeyData != (Keys.F | Keys.Control))
          return;
        this.FindInInspectedPackages();
      }
    }

    private void ToolStripButtonFind_Click(object sender, EventArgs e)
    {
      this.FindInInspectedPackages();
    }

    private void FindInInspectedPackages()
    {
      this.findDialog.PackageAndComponentMap = this.packageAndComponentMap;
      this.findDialog.PackageAndMumMap = this.packageAndMumMap;
      int num = (int) this.findDialog.ShowDialog((IWin32Window) this);
      if (this.findDialog.SelectedFoundItem == null)
        return;
      this.JumpToSelectedItem(this.findDialog);
    }

    private void JumpToSelectedItem(FindDialog dialog)
    {
      TreeNode node = this.packageTreeView.Nodes[0].Nodes[dialog.SelectedFoundItem.Path.Split(new char[1]
      {
        '/'
      }, StringSplitOptions.None)[0].Trim().ToUpperInvariant()];
      if (node == null)
        return;
      this.packageTreeView.SelectedNode = node;
      bool flag = false;
      List<TabPage> navigatedTabPages = new List<TabPage>();
      Dictionary<IFindNavigator, TabPage> dictionary = new Dictionary<IFindNavigator, TabPage>();
      dictionary.Add((IFindNavigator) this.componentListControl, this.tabPageComponents);
      dictionary.Add((IFindNavigator) this.registryKeyValueControl, this.tabPageRegistryKeys);
      dictionary.Add((IFindNavigator) this.fileListControl, this.tabPageFiles);
      dictionary.Add((IFindNavigator) this.dependencyListControl, this.tabPageDependencies);
      dictionary.Add((IFindNavigator) this.mumTreeListControl, this.tabPageApplicability);
      dictionary.Add((IFindNavigator) this.genericCommandsTreeListControl, this.tabPageGenericCommands);
      dictionary.Add((IFindNavigator) this.smiSettingsTreeListControl, this.tabPageSmiSettings);
      using (IEnumerator<ISearchProvider> enumerator = dialog.SelectedFoundItem.SearchProviders.GetEnumerator())
      {
label_11:
        while (enumerator.MoveNext())
        {
          ISearchProvider current = enumerator.Current;
          foreach (KeyValuePair<IFindNavigator, TabPage> keyValuePair in dictionary)
          {
            switch (keyValuePair.Key.NavigateToItem(current))
            {
              case NavigationStatus.Error:
                flag = true;
                goto label_11;
              case NavigationStatus.Success:
                TabPage tabPage = keyValuePair.Value;
                if (!navigatedTabPages.Contains(tabPage))
                {
                  navigatedTabPages.Add(tabPage);
                  goto label_11;
                }
                else
                  goto label_11;
              default:
                continue;
            }
          }
        }
      }
      if (flag)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Unable to navigate to the selected item.", new object[0]), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
      else
        this.NavigateToFoundTabPage(navigatedTabPages);
    }

    private void NavigateToFoundTabPage(List<TabPage> navigatedTabPages)
    {
      if (this.consolidatedView)
      {
        if (navigatedTabPages.Count == 0)
          return;
        TabPage navigatedTabPage = navigatedTabPages[navigatedTabPages.Count - 1];
        TabControl parent = navigatedTabPage.Parent as TabControl;
        if (parent == null)
          return;
        parent.SelectedTab = navigatedTabPage;
      }
      else
      {
        foreach (TabPage navigatedTabPage in navigatedTabPages)
        {
          TabControl parent = navigatedTabPage.Parent as TabControl;
          if (parent != null)
            parent.SelectedTab = navigatedTabPage;
        }
      }
    }

    private void FindToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.FindInInspectedPackages();
    }

    private void PackageTreeView_DoubleClick(object sender, EventArgs e)
    {
      if (this.packageTreeView.SelectedNode == null || this.packageTreeView.SelectedNode.Level != 1)
        return;
      this.InspectPackage();
    }

    private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string tag = (string) this.packageTreeView.SelectedNode.Tag;
      if (tag == null)
        return;
      if (this.packageAndComponentMap.ContainsKey(tag))
      {
        this.packageAndComponentMap.Remove(tag);
        if (!this.removedPackageList.Contains(tag.ToUpperInvariant()))
          this.removedPackageList.Add(tag.ToUpperInvariant());
      }
      this.packageTreeView.Nodes[0].Nodes.Remove(this.packageTreeView.SelectedNode);
    }

    private void CopyContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.outputRichTextBox.ContainsFocus || this.outputRichTextBox.SelectedText.Length <= 0)
        return;
      Clipboard.SetText(this.outputRichTextBox.SelectedText, TextDataFormat.Text);
    }

    private static void CopyTreeViewItemsToClipboard(TreeView treeView)
    {
      if (treeView.SelectedNode == null)
        return;
      Clipboard.SetText(treeView.SelectedNode.Text, TextDataFormat.Text);
    }

    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.packageTreeView.SelectedNode == null)
        return;
      Clipboard.SetText(this.packageTreeView.SelectedNode.Text, TextDataFormat.Text);
    }

    private void CommonContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      bool flag = true;
      if (this.outputRichTextBox.ContainsFocus && this.outputRichTextBox.SelectedText.Length == 0)
        flag = false;
      this.copyTextContextMenuStripMenuItem.Enabled = flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
      {
        this.components.Dispose();
        this.manifestAnalyzer.Dispose();
        this.packageAnalyzer.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PackageViewer));
      TreeNode treeNode = new TreeNode("CBS Packages and Comp. Manifests");
      this.mainMenuStrip = new MenuStrip();
      this.fileToolStripMenuItem = new ToolStripMenuItem();
      this.openToolStripMenuItem = new ToolStripMenuItem();
      this.cbsPackagesMenuItem = new ToolStripMenuItem();
      this.folderContainingCBSPackagesMenuItem = new ToolStripMenuItem();
      this.anAlreadyExpandedCBSPackageMenuItem = new ToolStripMenuItem();
      this.exitToolStripMenuItem = new ToolStripMenuItem();
      this.viewToolStripMenuItem = new ToolStripMenuItem();
      this.itemsPerComponentToolStripMenuItem = new ToolStripMenuItem();
      this.toolsToolStripMenuItem = new ToolStripMenuItem();
      this.inspectToolStripMenuItem = new ToolStripMenuItem();
      this.exportToolStripMenuItem = new ToolStripMenuItem();
      this.findToolStripMenuItem = new ToolStripMenuItem();
      this.helpToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripMenuItemUsage = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.aboutToolStripMenuItem = new ToolStripMenuItem();
      this.packagesContextMenuStrip = new ContextMenuStrip(this.components);
      this.inspectToolStripContextMenuItem = new ToolStripMenuItem();
      this.scratchPathToolStripContextMenuItem = new ToolStripMenuItem();
      this.copyTextToolStripMenuItem = new ToolStripMenuItem();
      this.removeToolStripMenuItem = new ToolStripMenuItem();
      this.imageList = new ImageList(this.components);
      this.commonContextMenuStrip = new ContextMenuStrip(this.components);
      this.copyTextContextMenuStripMenuItem = new ToolStripMenuItem();
      this.mainToolStrip = new ToolStrip();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.openToolStripButton = new ToolStripDropDownButton();
      this.cbsPackagesToolStripMenuItem = new ToolStripMenuItem();
      this.folderContainingCBSPackagesToolStripMenuItem = new ToolStripMenuItem();
      this.anAlreadyExpandedCBSPackageToolStripMenuItem = new ToolStripMenuItem();
      this.inspectToolStripButton = new ToolStripButton();
      this.toolStripButtonExport = new ToolStripButton();
      this.toolStripButtonFind = new ToolStripButton();
      this.exportFileDialog = new SaveFileDialog();
      this.openFileDialog = new OpenFileDialog();
      this.mainStatusStrip = new StatusStrip();
      this.toolStripStatusLabelPleaseWait = new ToolStripStatusLabel();
      this.folderBrowserDialog = new FolderBrowserDialog();
      this.backgroundWorker = new BackgroundWorker();
      this.openUpdateMumFileDialog = new OpenFileDialog();
      this.packageSplitContainer = new SplitContainer();
      this.packageTreeView = new TreeView();
      this.featureSetDetailsTabControl = new TabControl();
      this.tabPageComponents = new TabPage();
      this.componentSplitContainer = new SplitContainer();
      this.componentListControl = new ComponentListControl();
      this.componentDetailsTabControl = new TabControl();
      this.tabPageFiles = new TabPage();
      this.fileListControl = new FileListControl();
      this.tabPageRegistryKeys = new TabPage();
      this.registryKeyValueControl = new RegistryKeyValueControl();
      this.tabPageDependencies = new TabPage();
      this.dependencyListControl = new DependencyListControl();
      this.tabPageSmiSettings = new TabPage();
      this.smiSettingsTreeListControl = new SmiSettingsTreeListControl();
      this.tabPageGenericCommands = new TabPage();
      this.genericCommandsTreeListControl = new GenericCommandsTreeListControl();
      this.tabPageApplicability = new TabPage();
      this.mumTreeListControl = new MumTreeListControl();
      this.tabPageOutput = new TabPage();
      this.outputRichTextBox = new RichTextBox();
      this.mainMenuStrip.SuspendLayout();
      this.packagesContextMenuStrip.SuspendLayout();
      this.commonContextMenuStrip.SuspendLayout();
      this.mainToolStrip.SuspendLayout();
      this.mainStatusStrip.SuspendLayout();
      this.packageSplitContainer.Panel1.SuspendLayout();
      this.packageSplitContainer.Panel2.SuspendLayout();
      this.packageSplitContainer.SuspendLayout();
      this.featureSetDetailsTabControl.SuspendLayout();
      this.tabPageComponents.SuspendLayout();
      this.componentSplitContainer.Panel1.SuspendLayout();
      this.componentSplitContainer.Panel2.SuspendLayout();
      this.componentSplitContainer.SuspendLayout();
      this.componentDetailsTabControl.SuspendLayout();
      this.tabPageFiles.SuspendLayout();
      this.tabPageRegistryKeys.SuspendLayout();
      this.tabPageDependencies.SuspendLayout();
      this.tabPageSmiSettings.SuspendLayout();
      this.tabPageGenericCommands.SuspendLayout();
      this.tabPageApplicability.SuspendLayout();
      this.tabPageOutput.SuspendLayout();
      this.SuspendLayout();
      this.mainMenuStrip.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.fileToolStripMenuItem,
        (ToolStripItem) this.viewToolStripMenuItem,
        (ToolStripItem) this.toolsToolStripMenuItem,
        (ToolStripItem) this.helpToolStripMenuItem
      });
      this.mainMenuStrip.Location = new Point(0, 0);
      this.mainMenuStrip.Name = "mainMenuStrip";
      this.mainMenuStrip.Size = new Size(1016, 24);
      this.mainMenuStrip.TabIndex = 1;
      this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.openToolStripMenuItem,
        (ToolStripItem) this.exitToolStripMenuItem
      });
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      this.openToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.cbsPackagesMenuItem,
        (ToolStripItem) this.folderContainingCBSPackagesMenuItem,
        (ToolStripItem) this.anAlreadyExpandedCBSPackageMenuItem
      });
      this.openToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("openToolStripMenuItem.Image");
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new Size(103, 22);
      this.openToolStripMenuItem.Text = "&Open";
      this.cbsPackagesMenuItem.Name = "cbsPackagesMenuItem";
      this.cbsPackagesMenuItem.ShortcutKeys = Keys.O | Keys.Control;
      this.cbsPackagesMenuItem.Size = new Size(397, 22);
      this.cbsPackagesMenuItem.Text = "CBS Packages/Manifest Files...";
      this.cbsPackagesMenuItem.Click += new EventHandler(this.CbsPackagesMenuItem_Click);
      this.folderContainingCBSPackagesMenuItem.Name = "folderContainingCBSPackagesMenuItem";
      this.folderContainingCBSPackagesMenuItem.ShortcutKeys = Keys.O | Keys.Control | Keys.Alt;
      this.folderContainingCBSPackagesMenuItem.Size = new Size(397, 22);
      this.folderContainingCBSPackagesMenuItem.Text = "Folder Containing CBS Packages/Manifest Files...";
      this.folderContainingCBSPackagesMenuItem.Click += new EventHandler(this.FolderContainingCBSPackagesToolStripMenuItem_Click);
      this.anAlreadyExpandedCBSPackageMenuItem.Name = "anAlreadyExpandedCBSPackageMenuItem";
      this.anAlreadyExpandedCBSPackageMenuItem.ShortcutKeys = Keys.O | Keys.Shift | Keys.Control;
      this.anAlreadyExpandedCBSPackageMenuItem.Size = new Size(397, 22);
      this.anAlreadyExpandedCBSPackageMenuItem.Text = "An Already Expanded CBS Package...";
      this.anAlreadyExpandedCBSPackageMenuItem.Click += new EventHandler(this.AnAlreadyExpandedCBSPackageMenuItem_Click);
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new Size(103, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new EventHandler(this.ExitToolStripMenuItem_Click);
      this.viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.itemsPerComponentToolStripMenuItem
      });
      this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
      this.viewToolStripMenuItem.Size = new Size(44, 20);
      this.viewToolStripMenuItem.Text = "&View";
      this.itemsPerComponentToolStripMenuItem.Checked = true;
      this.itemsPerComponentToolStripMenuItem.CheckOnClick = true;
      this.itemsPerComponentToolStripMenuItem.CheckState = CheckState.Checked;
      this.itemsPerComponentToolStripMenuItem.Name = "itemsPerComponentToolStripMenuItem";
      this.itemsPerComponentToolStripMenuItem.Size = new Size(188, 22);
      this.itemsPerComponentToolStripMenuItem.Text = "&Items per component";
      this.itemsPerComponentToolStripMenuItem.ToolTipText = "View files, registry keys etc. on a component by component basis. Uncheck this to view all package contents at once.";
      this.itemsPerComponentToolStripMenuItem.Click += new EventHandler(this.ItemsPerComponentToolStripMenuItem_Click);
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.inspectToolStripMenuItem,
        (ToolStripItem) this.exportToolStripMenuItem,
        (ToolStripItem) this.findToolStripMenuItem
      });
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new Size(48, 20);
      this.toolsToolStripMenuItem.Text = "&Tools";
      this.inspectToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("inspectToolStripMenuItem.Image");
      this.inspectToolStripMenuItem.Name = "inspectToolStripMenuItem";
      this.inspectToolStripMenuItem.ShortcutKeys = Keys.I | Keys.Control;
      this.inspectToolStripMenuItem.Size = new Size(156, 22);
      this.inspectToolStripMenuItem.Text = "&Inspect";
      this.inspectToolStripMenuItem.Click += new EventHandler(this.InspectToolStripMenuItem_Click);
      this.exportToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("exportToolStripMenuItem.Image");
      this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
      this.exportToolStripMenuItem.ShortcutKeys = Keys.E | Keys.Control;
      this.exportToolStripMenuItem.Size = new Size(156, 22);
      this.exportToolStripMenuItem.Text = "&Export...";
      this.exportToolStripMenuItem.ToolTipText = "Save's inspected CBS packages";
      this.exportToolStripMenuItem.Click += new EventHandler(this.ExportToolStripMenuItem_Click);
      this.findToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("findToolStripMenuItem.Image");
      this.findToolStripMenuItem.Name = "findToolStripMenuItem";
      this.findToolStripMenuItem.ShortcutKeys = Keys.F | Keys.Control;
      this.findToolStripMenuItem.Size = new Size(156, 22);
      this.findToolStripMenuItem.Text = "Find...";
      this.findToolStripMenuItem.Click += new EventHandler(this.FindToolStripMenuItem_Click);
      this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.toolStripMenuItemUsage,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.aboutToolStripMenuItem
      });
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new Size(44, 20);
      this.helpToolStripMenuItem.Text = "&Help";
      this.toolStripMenuItemUsage.Name = "toolStripMenuItemUsage";
      this.toolStripMenuItemUsage.ShortcutKeys = Keys.F1;
      this.toolStripMenuItemUsage.Size = new Size(230, 22);
      this.toolStripMenuItemUsage.Text = "&Usage";
      this.toolStripMenuItemUsage.Click += new EventHandler(this.ToolStripMenuItemUsage_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(227, 6);
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new Size(230, 22);
      this.aboutToolStripMenuItem.Text = "&About CBS Package Inspector";
      this.aboutToolStripMenuItem.Click += new EventHandler(this.AboutToolStripMenuItem_Click);
      this.packagesContextMenuStrip.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.inspectToolStripContextMenuItem,
        (ToolStripItem) this.scratchPathToolStripContextMenuItem,
        (ToolStripItem) this.copyTextToolStripMenuItem,
        (ToolStripItem) this.removeToolStripMenuItem
      });
      this.packagesContextMenuStrip.Name = "packagesContextMenuStrip";
      this.packagesContextMenuStrip.Size = new Size(246, 92);
      this.inspectToolStripContextMenuItem.Enabled = false;
      this.inspectToolStripContextMenuItem.Image = (Image) componentResourceManager.GetObject("inspectToolStripContextMenuItem.Image");
      this.inspectToolStripContextMenuItem.Name = "inspectToolStripContextMenuItem";
      this.inspectToolStripContextMenuItem.Size = new Size(245, 22);
      this.inspectToolStripContextMenuItem.Text = "Inspect";
      this.inspectToolStripContextMenuItem.Click += new EventHandler(this.InspectToolStripContextMenuItem_Click);
      this.scratchPathToolStripContextMenuItem.Image = (Image) componentResourceManager.GetObject("scratchPathToolStripContextMenuItem.Image");
      this.scratchPathToolStripContextMenuItem.Name = "scratchPathToolStripContextMenuItem";
      this.scratchPathToolStripContextMenuItem.Size = new Size(245, 22);
      this.scratchPathToolStripContextMenuItem.Text = "Open Scratch/Containing Folder";
      this.scratchPathToolStripContextMenuItem.Click += new EventHandler(this.ScratchPathToolStripContextMenuItem_Click);
      this.copyTextToolStripMenuItem.Name = "copyTextToolStripMenuItem";
      this.copyTextToolStripMenuItem.Size = new Size(245, 22);
      this.copyTextToolStripMenuItem.Text = "Copy Text";
      this.copyTextToolStripMenuItem.Click += new EventHandler(this.CopyToolStripMenuItem_Click);
      this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
      this.removeToolStripMenuItem.Size = new Size(245, 22);
      this.removeToolStripMenuItem.Text = "Remove";
      this.removeToolStripMenuItem.Click += new EventHandler(this.RemoveToolStripMenuItem_Click);
      this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
      this.imageList.TransparentColor = Color.Transparent;
      this.imageList.Images.SetKeyName(0, "FileNew.ico");
      this.imageList.Images.SetKeyName(1, "FileOpen.ico");
      this.imageList.Images.SetKeyName(2, "FileSave.ico");
      this.imageList.Images.SetKeyName(3, "FeatureSet.ico");
      this.imageList.Images.SetKeyName(4, "BuildPackages.ico");
      this.imageList.Images.SetKeyName(5, "Component.ico");
      this.imageList.Images.SetKeyName(6, "Processor.ico");
      this.imageList.Images.SetKeyName(7, "BuildType.ico");
      this.imageList.Images.SetKeyName(8, "regedit.ico");
      this.imageList.Images.SetKeyName(9, "Depends.ico");
      this.imageList.Images.SetKeyName(10, "ReverseDepends.ico");
      this.imageList.Images.SetKeyName(11, "search.ico");
      this.imageList.Images.SetKeyName(12, "Files.ico");
      this.imageList.Images.SetKeyName(13, "SearchResults.ico");
      this.imageList.Images.SetKeyName(14, "app.ico");
      this.imageList.Images.SetKeyName(15, "error.ico");
      this.imageList.Images.SetKeyName(16, "Output.ico");
      this.imageList.Images.SetKeyName(17, "custcon.ico");
      this.imageList.Images.SetKeyName(18, "Standard.ico");
      this.imageList.Images.SetKeyName(19, "CabIcon.ico");
      this.imageList.Images.SetKeyName(20, "MsuIcon.ico");
      this.imageList.Images.SetKeyName(21, "ManifestIcon.ico");
      this.imageList.Images.SetKeyName(22, "SmiIcon.ico");
      this.imageList.Images.SetKeyName(23, "ImagesAndCbsPackages.ico");
      this.imageList.Images.SetKeyName(24, "CabIconExpanded.ico");
      this.imageList.Images.SetKeyName(25, "MsuIconExpanded.ico");
      this.imageList.Images.SetKeyName(26, "ManifestIconExpanded.ico");
      this.imageList.Images.SetKeyName(27, "Inspect.ico");
      this.imageList.Images.SetKeyName(28, "PackageInfo.ico");
      this.imageList.Images.SetKeyName(29, "GCIcon.ico");
      this.imageList.Images.SetKeyName(30, "PackageFolder.ico");
      this.imageList.Images.SetKeyName(31, "PackageFolderExpanded.ico");
      this.commonContextMenuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.copyTextContextMenuStripMenuItem
      });
      this.commonContextMenuStrip.Name = "commonContextMenuStrip";
      this.commonContextMenuStrip.Size = new Size(128, 26);
      this.commonContextMenuStrip.Opening += new CancelEventHandler(this.CommonContextMenuStrip_Opening);
      this.copyTextContextMenuStripMenuItem.Name = "copyTextContextMenuStripMenuItem";
      this.copyTextContextMenuStripMenuItem.Size = new Size((int) sbyte.MaxValue, 22);
      this.copyTextContextMenuStripMenuItem.Text = "Copy Text";
      this.copyTextContextMenuStripMenuItem.Click += new EventHandler(this.CopyContextMenuStripMenuItem_Click);
      this.mainToolStrip.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.openToolStripButton,
        (ToolStripItem) this.inspectToolStripButton,
        (ToolStripItem) this.toolStripButtonExport,
        (ToolStripItem) this.toolStripButtonFind
      });
      this.mainToolStrip.Location = new Point(0, 24);
      this.mainToolStrip.Name = "mainToolStrip";
      this.mainToolStrip.Size = new Size(1016, 25);
      this.mainToolStrip.TabIndex = 3;
      this.mainToolStrip.Text = "toolStrip1";
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(6, 25);
      this.openToolStripButton.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.cbsPackagesToolStripMenuItem,
        (ToolStripItem) this.folderContainingCBSPackagesToolStripMenuItem,
        (ToolStripItem) this.anAlreadyExpandedCBSPackageToolStripMenuItem
      });
      this.openToolStripButton.Image = (Image) componentResourceManager.GetObject("openToolStripButton.Image");
      this.openToolStripButton.ImageTransparentColor = Color.Magenta;
      this.openToolStripButton.Name = "openToolStripButton";
      this.openToolStripButton.Size = new Size(65, 22);
      this.openToolStripButton.Text = "Open";
      this.cbsPackagesToolStripMenuItem.Name = "cbsPackagesToolStripMenuItem";
      this.cbsPackagesToolStripMenuItem.ShortcutKeys = Keys.O | Keys.Control;
      this.cbsPackagesToolStripMenuItem.Size = new Size(397, 22);
      this.cbsPackagesToolStripMenuItem.Text = "CBS Packages/Manifest Files...";
      this.cbsPackagesToolStripMenuItem.Click += new EventHandler(this.CbsPackagesToolStripMenuItem_Click);
      this.folderContainingCBSPackagesToolStripMenuItem.Name = "folderContainingCBSPackagesToolStripMenuItem";
      this.folderContainingCBSPackagesToolStripMenuItem.ShortcutKeys = Keys.O | Keys.Control | Keys.Alt;
      this.folderContainingCBSPackagesToolStripMenuItem.Size = new Size(397, 22);
      this.folderContainingCBSPackagesToolStripMenuItem.Text = "Folder Containing CBS Packages/Manifest Files...";
      this.folderContainingCBSPackagesToolStripMenuItem.Click += new EventHandler(this.FolderContainingCBSPackagesToolStripMenuItem_Click);
      this.anAlreadyExpandedCBSPackageToolStripMenuItem.Name = "anAlreadyExpandedCBSPackageToolStripMenuItem";
      this.anAlreadyExpandedCBSPackageToolStripMenuItem.ShortcutKeys = Keys.O | Keys.Shift | Keys.Control;
      this.anAlreadyExpandedCBSPackageToolStripMenuItem.Size = new Size(397, 22);
      this.anAlreadyExpandedCBSPackageToolStripMenuItem.Text = "An Already Expanded CBS Package...";
      this.anAlreadyExpandedCBSPackageToolStripMenuItem.Click += new EventHandler(this.AnAlreadyExpandedCBSPackageToolStripMenuItem_Click);
      this.inspectToolStripButton.Enabled = false;
      this.inspectToolStripButton.Image = (Image) componentResourceManager.GetObject("inspectToolStripButton.Image");
      this.inspectToolStripButton.ImageTransparentColor = Color.Magenta;
      this.inspectToolStripButton.Name = "inspectToolStripButton";
      this.inspectToolStripButton.Size = new Size(65, 22);
      this.inspectToolStripButton.Text = "Inspect";
      this.inspectToolStripButton.ToolTipText = "Inspects the selected CBS package. You can also double-click the mouse on the selected package.";
      this.inspectToolStripButton.Click += new EventHandler(this.InspectToolStripButton_Click);
      this.toolStripButtonExport.Image = (Image) componentResourceManager.GetObject("toolStripButtonExport.Image");
      this.toolStripButtonExport.ImageTransparentColor = Color.Magenta;
      this.toolStripButtonExport.Name = "toolStripButtonExport";
      this.toolStripButtonExport.Size = new Size(60, 22);
      this.toolStripButtonExport.Text = "Export";
      this.toolStripButtonExport.ToolTipText = "Exports package and component detail about the inspected CBS packages";
      this.toolStripButtonExport.Click += new EventHandler(this.ExportToolStripButton_Click);
      this.toolStripButtonFind.Image = (Image) componentResourceManager.GetObject("toolStripButtonFind.Image");
      this.toolStripButtonFind.ImageTransparentColor = Color.Magenta;
      this.toolStripButtonFind.Name = "toolStripButtonFind";
      this.toolStripButtonFind.Size = new Size(50, 22);
      this.toolStripButtonFind.Text = "Find";
      this.toolStripButtonFind.Click += new EventHandler(this.ToolStripButtonFind_Click);
      this.exportFileDialog.DefaultExt = "xml";
      this.exportFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
      this.exportFileDialog.Title = "Export Inspected CBS Packages";
      this.openFileDialog.Filter = "CBS files (*.cab, *.msu, *.manifest, *.man)|*.cab;*.msu;*.manifest;*.man|Cab files (*.cab)|*.cab|Msu files (*.msu)|*.msu|Manifest files (*.manifest;*.man)|*.manifest;*.man|All files (*.*)|*.*";
      this.openFileDialog.Multiselect = true;
      this.openFileDialog.Title = "Open CBS Packages or Manifest Files";
      this.mainStatusStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.toolStripStatusLabelPleaseWait
      });
      this.mainStatusStrip.Location = new Point(0, 595);
      this.mainStatusStrip.Name = "mainStatusStrip";
      this.mainStatusStrip.Size = new Size(1016, 22);
      this.mainStatusStrip.TabIndex = 1;
      this.mainStatusStrip.Text = "statusStrip1";
      this.toolStripStatusLabelPleaseWait.Name = "toolStripStatusLabelPleaseWait";
      this.toolStripStatusLabelPleaseWait.Size = new Size(186, 17);
      this.toolStripStatusLabelPleaseWait.Text = "Analyzing manifests. Please wait...";
      this.toolStripStatusLabelPleaseWait.TextAlign = ContentAlignment.MiddleLeft;
      this.toolStripStatusLabelPleaseWait.ToolTipText = "Analyzing manifests. Please wait...";
      this.toolStripStatusLabelPleaseWait.Visible = false;
      this.folderBrowserDialog.Description = "Open folder containing CBS packages or manifest files";
      this.backgroundWorker.WorkerSupportsCancellation = true;
      this.backgroundWorker.DoWork += new DoWorkEventHandler(this.BackgroundWorker_DoWork);
      this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
      this.openUpdateMumFileDialog.Filter = "update.mum file (update.mum)|update.mum";
      this.openUpdateMumFileDialog.Title = "Open a previously expanded CBS package through its update.mum file";
      this.packageSplitContainer.Dock = DockStyle.Fill;
      this.packageSplitContainer.Location = new Point(0, 49);
      this.packageSplitContainer.Name = "packageSplitContainer";
      this.packageSplitContainer.Panel1.Controls.Add((Control) this.packageTreeView);
      this.packageSplitContainer.Panel2.Controls.Add((Control) this.featureSetDetailsTabControl);
      this.packageSplitContainer.Size = new Size(1016, 546);
      this.packageSplitContainer.SplitterDistance = 220;
      this.packageSplitContainer.TabIndex = 0;
      this.packageTreeView.AllowDrop = true;
      this.packageTreeView.ContextMenuStrip = this.packagesContextMenuStrip;
      this.packageTreeView.Dock = DockStyle.Fill;
      this.packageTreeView.FullRowSelect = true;
      this.packageTreeView.HideSelection = false;
      this.packageTreeView.ImageIndex = 4;
      this.packageTreeView.ImageList = this.imageList;
      this.packageTreeView.Location = new Point(0, 0);
      this.packageTreeView.Name = "packageTreeView";
      treeNode.ImageKey = "ImagesAndCbsPackages.ico";
      treeNode.Name = "NodeRoot";
      treeNode.NodeFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      treeNode.SelectedImageKey = "ImagesAndCbsPackages.ico";
      treeNode.Text = "CBS Packages and Comp. Manifests";
      this.packageTreeView.Nodes.AddRange(new TreeNode[1]
      {
        treeNode
      });
      this.packageTreeView.SelectedImageIndex = 0;
      this.packageTreeView.ShowNodeToolTips = true;
      this.packageTreeView.Size = new Size(220, 546);
      this.packageTreeView.TabIndex = 0;
      this.packageTreeView.DoubleClick += new EventHandler(this.PackageTreeView_DoubleClick);
      this.packageTreeView.AfterSelect += new TreeViewEventHandler(this.PackagesTreeView_AfterSelect);
      this.featureSetDetailsTabControl.Controls.Add((Control) this.tabPageComponents);
      this.featureSetDetailsTabControl.Controls.Add((Control) this.tabPageApplicability);
      this.featureSetDetailsTabControl.Controls.Add((Control) this.tabPageOutput);
      this.featureSetDetailsTabControl.Dock = DockStyle.Fill;
      this.featureSetDetailsTabControl.ImageList = this.imageList;
      this.featureSetDetailsTabControl.Location = new Point(0, 0);
      this.featureSetDetailsTabControl.Name = "featureSetDetailsTabControl";
      this.featureSetDetailsTabControl.SelectedIndex = 0;
      this.featureSetDetailsTabControl.Size = new Size(792, 546);
      this.featureSetDetailsTabControl.TabIndex = 0;
      this.tabPageComponents.Controls.Add((Control) this.componentSplitContainer);
      this.tabPageComponents.ImageIndex = 5;
      this.tabPageComponents.Location = new Point(4, 23);
      this.tabPageComponents.Name = "tabPageComponents";
      this.tabPageComponents.Padding = new Padding(3);
      this.tabPageComponents.Size = new Size(784, 519);
      this.tabPageComponents.TabIndex = 0;
      this.tabPageComponents.Text = "Components";
      this.tabPageComponents.UseVisualStyleBackColor = true;
      this.componentSplitContainer.Dock = DockStyle.Fill;
      this.componentSplitContainer.Location = new Point(3, 3);
      this.componentSplitContainer.Name = "componentSplitContainer";
      this.componentSplitContainer.Orientation = Orientation.Horizontal;
      this.componentSplitContainer.Panel1.Controls.Add((Control) this.componentListControl);
      this.componentSplitContainer.Panel2.Controls.Add((Control) this.componentDetailsTabControl);
      this.componentSplitContainer.Size = new Size(778, 513);
      this.componentSplitContainer.SplitterDistance = 219;
      this.componentSplitContainer.TabIndex = 0;
      this.componentListControl.Data = (object) null;
      this.componentListControl.Dock = DockStyle.Fill;
      this.componentListControl.Location = new Point(0, 0);
      this.componentListControl.Name = "componentListControl";
      this.componentListControl.Size = new Size(778, 219);
      this.componentListControl.TabIndex = 1;
      this.componentDetailsTabControl.Controls.Add((Control) this.tabPageFiles);
      this.componentDetailsTabControl.Controls.Add((Control) this.tabPageRegistryKeys);
      this.componentDetailsTabControl.Controls.Add((Control) this.tabPageDependencies);
      this.componentDetailsTabControl.Controls.Add((Control) this.tabPageSmiSettings);
      this.componentDetailsTabControl.Controls.Add((Control) this.tabPageGenericCommands);
      this.componentDetailsTabControl.Dock = DockStyle.Fill;
      this.componentDetailsTabControl.ImageList = this.imageList;
      this.componentDetailsTabControl.Location = new Point(0, 0);
      this.componentDetailsTabControl.Name = "componentDetailsTabControl";
      this.componentDetailsTabControl.SelectedIndex = 0;
      this.componentDetailsTabControl.Size = new Size(778, 290);
      this.componentDetailsTabControl.TabIndex = 0;
      this.tabPageFiles.Controls.Add((Control) this.fileListControl);
      this.tabPageFiles.ImageIndex = 12;
      this.tabPageFiles.Location = new Point(4, 23);
      this.tabPageFiles.Name = "tabPageFiles";
      this.tabPageFiles.Padding = new Padding(3);
      this.tabPageFiles.Size = new Size(770, 263);
      this.tabPageFiles.TabIndex = 0;
      this.tabPageFiles.Text = "Files";
      this.tabPageFiles.UseVisualStyleBackColor = true;
      this.fileListControl.Data = (object) null;
      this.fileListControl.Dock = DockStyle.Fill;
      this.fileListControl.Location = new Point(3, 3);
      this.fileListControl.Name = "fileListControl";
      this.fileListControl.Size = new Size(764, 257);
      this.fileListControl.TabIndex = 1;
      this.tabPageRegistryKeys.Controls.Add((Control) this.registryKeyValueControl);
      this.tabPageRegistryKeys.ImageIndex = 8;
      this.tabPageRegistryKeys.Location = new Point(4, 23);
      this.tabPageRegistryKeys.Name = "tabPageRegistryKeys";
      this.tabPageRegistryKeys.Padding = new Padding(3);
      this.tabPageRegistryKeys.Size = new Size(770, 263);
      this.tabPageRegistryKeys.TabIndex = 1;
      this.tabPageRegistryKeys.Text = "Registry Keys";
      this.tabPageRegistryKeys.UseVisualStyleBackColor = true;
      this.registryKeyValueControl.Data = (object) null;
      this.registryKeyValueControl.Dock = DockStyle.Fill;
      this.registryKeyValueControl.Location = new Point(3, 3);
      this.registryKeyValueControl.Name = "registryKeyValueControl";
      this.registryKeyValueControl.Size = new Size(764, 257);
      this.registryKeyValueControl.TabIndex = 1;
      this.tabPageDependencies.Controls.Add((Control) this.dependencyListControl);
      this.tabPageDependencies.ImageIndex = 9;
      this.tabPageDependencies.Location = new Point(4, 23);
      this.tabPageDependencies.Name = "tabPageDependencies";
      this.tabPageDependencies.Padding = new Padding(3);
      this.tabPageDependencies.Size = new Size(770, 263);
      this.tabPageDependencies.TabIndex = 2;
      this.tabPageDependencies.Text = "Dependencies";
      this.tabPageDependencies.UseVisualStyleBackColor = true;
      this.dependencyListControl.Data = (object) null;
      this.dependencyListControl.Dock = DockStyle.Fill;
      this.dependencyListControl.Location = new Point(3, 3);
      this.dependencyListControl.Name = "dependencyListControl";
      this.dependencyListControl.Size = new Size(764, 257);
      this.dependencyListControl.TabIndex = 1;
      this.tabPageSmiSettings.Controls.Add((Control) this.smiSettingsTreeListControl);
      this.tabPageSmiSettings.ImageKey = "SmiIcon.ico";
      this.tabPageSmiSettings.Location = new Point(4, 23);
      this.tabPageSmiSettings.Name = "tabPageSmiSettings";
      this.tabPageSmiSettings.Size = new Size(770, 263);
      this.tabPageSmiSettings.TabIndex = 5;
      this.tabPageSmiSettings.Text = "SMI Settings";
      this.tabPageSmiSettings.UseVisualStyleBackColor = true;
      this.smiSettingsTreeListControl.Data = (object) null;
      this.smiSettingsTreeListControl.Dock = DockStyle.Fill;
      this.smiSettingsTreeListControl.Location = new Point(0, 0);
      this.smiSettingsTreeListControl.Name = "smiSettingsTreeListControl";
      this.smiSettingsTreeListControl.Size = new Size(770, 263);
      this.smiSettingsTreeListControl.TabIndex = 1;
      this.tabPageGenericCommands.Controls.Add((Control) this.genericCommandsTreeListControl);
      this.tabPageGenericCommands.ImageIndex = 29;
      this.tabPageGenericCommands.Location = new Point(4, 23);
      this.tabPageGenericCommands.Name = "tabPageGenericCommands";
      this.tabPageGenericCommands.Size = new Size(770, 263);
      this.tabPageGenericCommands.TabIndex = 6;
      this.tabPageGenericCommands.Text = "Generic Commands";
      this.tabPageGenericCommands.UseVisualStyleBackColor = true;
      this.genericCommandsTreeListControl.Data = (object) null;
      this.genericCommandsTreeListControl.Dock = DockStyle.Fill;
      this.genericCommandsTreeListControl.Location = new Point(0, 0);
      this.genericCommandsTreeListControl.Name = "genericCommandsTreeListControl";
      this.genericCommandsTreeListControl.Size = new Size(770, 263);
      this.genericCommandsTreeListControl.TabIndex = 1;
      this.tabPageApplicability.ContextMenuStrip = this.commonContextMenuStrip;
      this.tabPageApplicability.Controls.Add((Control) this.mumTreeListControl);
      this.tabPageApplicability.ImageIndex = 28;
      this.tabPageApplicability.Location = new Point(4, 23);
      this.tabPageApplicability.Name = "tabPageApplicability";
      this.tabPageApplicability.Size = new Size(784, 519);
      this.tabPageApplicability.TabIndex = 4;
      this.tabPageApplicability.Text = "Package Applicability";
      this.tabPageApplicability.UseVisualStyleBackColor = true;
      this.mumTreeListControl.Data = (object) null;
      this.mumTreeListControl.Dock = DockStyle.Fill;
      this.mumTreeListControl.Location = new Point(0, 0);
      this.mumTreeListControl.Name = "mumTreeListControl";
      this.mumTreeListControl.Size = new Size(784, 519);
      this.mumTreeListControl.TabIndex = 1;
      this.tabPageOutput.Controls.Add((Control) this.outputRichTextBox);
      this.tabPageOutput.ImageIndex = 16;
      this.tabPageOutput.Location = new Point(4, 23);
      this.tabPageOutput.Name = "tabPageOutput";
      this.tabPageOutput.Size = new Size(784, 519);
      this.tabPageOutput.TabIndex = 3;
      this.tabPageOutput.Text = "Output";
      this.tabPageOutput.UseVisualStyleBackColor = true;
      this.outputRichTextBox.BackColor = SystemColors.Window;
      this.outputRichTextBox.ContextMenuStrip = this.commonContextMenuStrip;
      this.outputRichTextBox.Dock = DockStyle.Fill;
      this.outputRichTextBox.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.outputRichTextBox.Location = new Point(0, 0);
      this.outputRichTextBox.Name = "outputRichTextBox";
      this.outputRichTextBox.ReadOnly = true;
      this.outputRichTextBox.Size = new Size(784, 519);
      this.outputRichTextBox.TabIndex = 0;
      this.outputRichTextBox.Text = "";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1016, 617);
      this.Controls.Add((Control) this.packageSplitContainer);
      this.Controls.Add((Control) this.mainToolStrip);
      this.Controls.Add((Control) this.mainMenuStrip);
      this.Controls.Add((Control) this.mainStatusStrip);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MainMenuStrip = this.mainMenuStrip;
      this.Name = "PackageViewer";
      this.Text = "CBS Package Inspector";
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.PackageViewer_Load);
      this.FormClosing += new FormClosingEventHandler(this.PackageViewer_FormClosing);
      this.KeyDown += new KeyEventHandler(this.PackageViewer_KeyDown);
      this.mainMenuStrip.ResumeLayout(false);
      this.mainMenuStrip.PerformLayout();
      this.packagesContextMenuStrip.ResumeLayout(false);
      this.commonContextMenuStrip.ResumeLayout(false);
      this.mainToolStrip.ResumeLayout(false);
      this.mainToolStrip.PerformLayout();
      this.mainStatusStrip.ResumeLayout(false);
      this.mainStatusStrip.PerformLayout();
      this.packageSplitContainer.Panel1.ResumeLayout(false);
      this.packageSplitContainer.Panel2.ResumeLayout(false);
      this.packageSplitContainer.ResumeLayout(false);
      this.featureSetDetailsTabControl.ResumeLayout(false);
      this.tabPageComponents.ResumeLayout(false);
      this.componentSplitContainer.Panel1.ResumeLayout(false);
      this.componentSplitContainer.Panel2.ResumeLayout(false);
      this.componentSplitContainer.ResumeLayout(false);
      this.componentDetailsTabControl.ResumeLayout(false);
      this.tabPageFiles.ResumeLayout(false);
      this.tabPageRegistryKeys.ResumeLayout(false);
      this.tabPageDependencies.ResumeLayout(false);
      this.tabPageSmiSettings.ResumeLayout(false);
      this.tabPageGenericCommands.ResumeLayout(false);
      this.tabPageApplicability.ResumeLayout(false);
      this.tabPageOutput.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate void SetTextCallback(string text);

    private delegate void ShowWaitDialogCallback();
  }
}
