// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.ComponentListControl
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PackageInspector.Controls
{
  public class ComponentListControl : UserControl, IItemViewer, IFindNavigator
  {
    private ListViewColumnSorter columnSorterComponents = new ListViewColumnSorter();
    private IContainer components;
    private ListView listViewComponents;
    private ColumnHeader columnHeaderComponentNum;
    private ColumnHeader columnHeaderName;
    private ColumnHeader columnHeaderProcessorArchitecture;
    private ColumnHeader columnHeaderVersion;
    private ColumnHeader columnHeaderLanguage;
    private ColumnHeader columnHeaderVersionScope;
    private ContextMenuStrip manifestContextMenuStrip;
    private ToolStripMenuItem copyTextManifestContextMenuStripMenuItem;
    private ToolStripMenuItem copyFilePathManifestContextMenuStripMenuItem;
    private ToolStripMenuItem openFileManifestContextMenuStripMenuItem;

    public WindowsComponent SelectedComponent { get; private set; }

    public object Data { get; set; }

    public event EventHandler<ComponentEventArgs> SelectionChanged;

    public ComponentListControl()
    {
      this.InitializeComponent();
      this.InitializeCustomUI();
    }

    private void InitializeCustomUI()
    {
      this.listViewComponents.ListViewItemSorter = (IComparer) this.columnSorterComponents;
    }

    private void OpenFileManifestContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      string manifestFilePath = this.GetManifestFilePath();
      if (manifestFilePath.Length == 0)
        return;
      string appSetting = ConfigurationSettings.AppSettings["manifestFileViewerPath"];
      try
      {
        Process.Start(appSetting, manifestFilePath);
      }
      catch (Win32Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
    }

    private void ManifestContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      if (!this.listViewComponents.ContainsFocus)
        return;
      bool flag = this.listViewComponents.SelectedItems.Count > 0;
      this.copyTextManifestContextMenuStripMenuItem.Enabled = flag;
      this.copyFilePathManifestContextMenuStripMenuItem.Enabled = flag;
      this.openFileManifestContextMenuStripMenuItem.Enabled = flag;
    }

    private void CopyTextManifestContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.listViewComponents.ContainsFocus)
        return;
      ComponentListControl.CopyListViewItemsToClipboard(this.listViewComponents);
    }

    private void CopyFilePathManifestContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      string manifestFilePath = this.GetManifestFilePath();
      if (manifestFilePath.Length == 0)
        return;
      Clipboard.SetText(manifestFilePath, TextDataFormat.Text);
    }

    private string GetManifestFilePath()
    {
      if (this.listViewComponents.ContainsFocus)
      {
        string empty = string.Empty;
        IEnumerator enumerator = this.listViewComponents.SelectedItems.GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
            return ((ListViewItem) enumerator.Current).Name.Split(new char[1]
            {
              '/'
            }, StringSplitOptions.None)[5];
        }
        finally
        {
          IDisposable disposable = enumerator as IDisposable;
          if (disposable != null)
            disposable.Dispose();
        }
      }
      return string.Empty;
    }

    private void ListViewComponents_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      ListViewColumnSorter.Sort(sender, e.Column);
    }

    private static void CopyListViewItemsToClipboard(ListView listView)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string appSetting = ConfigurationSettings.AppSettings["listviewCopyItemSeparator"];
      foreach (ListViewItem selectedItem in listView.SelectedItems)
      {
        int index;
        for (index = 0; index < selectedItem.SubItems.Count - 1; ++index)
        {
          stringBuilder.Append(selectedItem.SubItems[index].Text);
          stringBuilder.Append(appSetting);
        }
        stringBuilder.Append(selectedItem.SubItems[index].Text);
        stringBuilder.Append(Environment.NewLine);
      }
      if (stringBuilder.Length <= 0)
        return;
      Clipboard.SetText(stringBuilder.ToString(), TextDataFormat.Text);
    }

    public void UpdateView()
    {
      if (this.Data == null)
        return;
      IList<WindowsComponent> data = this.Data as IList<WindowsComponent>;
      if (data == null)
        return;
      int num = 0;
      this.listViewComponents.BeginUpdate();
      foreach (WindowsComponent windowsComponent in (IEnumerable<WindowsComponent>) data)
      {
        ListViewItem listViewItem = new ListViewItem(new string[6]
        {
          (num + 1).ToString((IFormatProvider) CultureInfo.CurrentCulture),
          windowsComponent.AssemblyIdentity.Name,
          windowsComponent.AssemblyIdentity.Architecture,
          windowsComponent.AssemblyIdentity.Version,
          windowsComponent.AssemblyIdentity.Language,
          windowsComponent.AssemblyIdentity.VersionScope
        });
        string str = windowsComponent.AssemblyIdentity.Name + "/" + windowsComponent.AssemblyIdentity.Architecture + "/" + windowsComponent.AssemblyIdentity.Version + "/" + windowsComponent.AssemblyIdentity.Language + "/" + windowsComponent.AssemblyIdentity.VersionScope + "/" + windowsComponent.ManifestFile;
        listViewItem.Tag = (object) windowsComponent;
        listViewItem.Name = str;
        this.listViewComponents.Items.Add(listViewItem);
        ++num;
      }
      this.listViewComponents.EndUpdate();
      if (this.listViewComponents.Items.Count <= 0)
        return;
      this.listViewComponents.Items[0].Selected = true;
      this.SelectedComponent = (WindowsComponent) this.listViewComponents.Items[0].Tag;
    }

    public void ClearView()
    {
      this.listViewComponents.Items.Clear();
    }

    private void ListViewComponents_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listViewComponents.SelectedItems.Count != 1)
        return;
      this.SelectedComponent = (WindowsComponent) this.listViewComponents.SelectedItems[0].Tag;
      if (this.SelectionChanged == null)
        return;
      this.SelectionChanged((object) this, new ComponentEventArgs(this.SelectedComponent));
    }

    public NavigationStatus NavigateToItem(ISearchProvider provider)
    {
      if (provider == null)
        return NavigationStatus.NotApplicable;
      WindowsComponent windowsComponent = provider as WindowsComponent;
      if (windowsComponent == null)
        return NavigationStatus.NotApplicable;
      this.SelectedComponent = windowsComponent;
      string index = windowsComponent.AssemblyIdentity.Name + "/" + windowsComponent.AssemblyIdentity.Architecture + "/" + windowsComponent.AssemblyIdentity.Version + "/" + windowsComponent.AssemblyIdentity.Language + "/" + windowsComponent.AssemblyIdentity.VersionScope + "/" + windowsComponent.ManifestFile;
      NavigationStatus navigationStatus = NavigationStatus.Success;
      if (this.listViewComponents.Items[index] != null)
      {
        this.listViewComponents.Items[index].Selected = true;
        this.listViewComponents.Items[index].Focused = true;
      }
      else
        navigationStatus = NavigationStatus.Error;
      if (this.SelectionChanged != null)
        this.SelectionChanged((object) this, new ComponentEventArgs(this.SelectedComponent));
      return navigationStatus;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.listViewComponents = new ListView();
      this.columnHeaderComponentNum = new ColumnHeader();
      this.columnHeaderName = new ColumnHeader();
      this.columnHeaderProcessorArchitecture = new ColumnHeader();
      this.columnHeaderVersion = new ColumnHeader();
      this.columnHeaderLanguage = new ColumnHeader();
      this.columnHeaderVersionScope = new ColumnHeader();
      this.manifestContextMenuStrip = new ContextMenuStrip(this.components);
      this.copyTextManifestContextMenuStripMenuItem = new ToolStripMenuItem();
      this.copyFilePathManifestContextMenuStripMenuItem = new ToolStripMenuItem();
      this.openFileManifestContextMenuStripMenuItem = new ToolStripMenuItem();
      this.manifestContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.listViewComponents.Columns.AddRange(new ColumnHeader[6]
      {
        this.columnHeaderComponentNum,
        this.columnHeaderName,
        this.columnHeaderProcessorArchitecture,
        this.columnHeaderVersion,
        this.columnHeaderLanguage,
        this.columnHeaderVersionScope
      });
      this.listViewComponents.ContextMenuStrip = this.manifestContextMenuStrip;
      this.listViewComponents.Dock = DockStyle.Fill;
      this.listViewComponents.FullRowSelect = true;
      this.listViewComponents.GridLines = true;
      this.listViewComponents.HideSelection = false;
      this.listViewComponents.Location = new Point(0, 0);
      this.listViewComponents.MultiSelect = false;
      this.listViewComponents.Name = "listViewComponents";
      this.listViewComponents.Size = new Size(326, 183);
      this.listViewComponents.TabIndex = 1;
      this.listViewComponents.UseCompatibleStateImageBehavior = false;
      this.listViewComponents.View = View.Details;
      this.listViewComponents.SelectedIndexChanged += new EventHandler(this.ListViewComponents_SelectedIndexChanged);
      this.listViewComponents.ColumnClick += new ColumnClickEventHandler(this.ListViewComponents_ColumnClick);
      this.columnHeaderComponentNum.Text = "No.";
      this.columnHeaderComponentNum.Width = 35;
      this.columnHeaderName.Text = "Name";
      this.columnHeaderName.Width = 238;
      this.columnHeaderProcessorArchitecture.Text = "Processor Architecture";
      this.columnHeaderProcessorArchitecture.Width = 150;
      this.columnHeaderVersion.Text = "Version";
      this.columnHeaderVersion.Width = 100;
      this.columnHeaderLanguage.Text = "Language";
      this.columnHeaderLanguage.Width = 90;
      this.columnHeaderVersionScope.Text = "Version Scope";
      this.columnHeaderVersionScope.Width = 81;
      this.manifestContextMenuStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.copyTextManifestContextMenuStripMenuItem,
        (ToolStripItem) this.copyFilePathManifestContextMenuStripMenuItem,
        (ToolStripItem) this.openFileManifestContextMenuStripMenuItem
      });
      this.manifestContextMenuStrip.Name = "metadataContextMenuStrip";
      this.manifestContextMenuStrip.Size = new Size(200, 70);
      this.manifestContextMenuStrip.Opening += new CancelEventHandler(this.ManifestContextMenuStrip_Opening);
      this.copyTextManifestContextMenuStripMenuItem.Name = "copyTextManifestContextMenuStripMenuItem";
      this.copyTextManifestContextMenuStripMenuItem.Size = new Size(199, 22);
      this.copyTextManifestContextMenuStripMenuItem.Text = "Copy Text";
      this.copyTextManifestContextMenuStripMenuItem.Click += new EventHandler(this.CopyTextManifestContextMenuStripMenuItem_Click);
      this.copyFilePathManifestContextMenuStripMenuItem.Name = "copyFilePathManifestContextMenuStripMenuItem";
      this.copyFilePathManifestContextMenuStripMenuItem.Size = new Size(199, 22);
      this.copyFilePathManifestContextMenuStripMenuItem.Text = "Copy Manifest File Path";
      this.copyFilePathManifestContextMenuStripMenuItem.Click += new EventHandler(this.CopyFilePathManifestContextMenuStripMenuItem_Click);
      this.openFileManifestContextMenuStripMenuItem.Name = "openFileManifestContextMenuStripMenuItem";
      this.openFileManifestContextMenuStripMenuItem.Size = new Size(199, 22);
      this.openFileManifestContextMenuStripMenuItem.Text = "Open Manifest File";
      this.openFileManifestContextMenuStripMenuItem.Click += new EventHandler(this.OpenFileManifestContextMenuStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.listViewComponents);
      this.Name = "ComponentListControl";
      this.Size = new Size(326, 183);
      this.manifestContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
