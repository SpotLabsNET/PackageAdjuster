// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.DependencyListControl
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PackageInspector.Controls
{
  public class DependencyListControl : UserControl, IItemViewer, IFindNavigator
  {
    private ListViewColumnSorter columnSorterDependencies = new ListViewColumnSorter();
    private IContainer components;
    private ListView listViewDependencies;
    private ColumnHeader columnHeaderDependentComponentNum;
    private ColumnHeader columnHeaderDependentComponentName;
    private ColumnHeader columnHeaderDependentComponentArchitecture;
    private ColumnHeader columnHeaderDependentComponentVersion;
    private ColumnHeader columnHeaderDependentComponentLanguage;
    private ContextMenuStrip commonContextMenuStrip;
    private ToolStripMenuItem copyTextContextMenuStripMenuItem;

    public object Data { get; set; }

    public DependencyListControl()
    {
      this.InitializeComponent();
      this.InitializeCustomUI();
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
      this.listViewDependencies = new ListView();
      this.columnHeaderDependentComponentNum = new ColumnHeader();
      this.columnHeaderDependentComponentName = new ColumnHeader();
      this.columnHeaderDependentComponentArchitecture = new ColumnHeader();
      this.columnHeaderDependentComponentVersion = new ColumnHeader();
      this.columnHeaderDependentComponentLanguage = new ColumnHeader();
      this.commonContextMenuStrip = new ContextMenuStrip(this.components);
      this.copyTextContextMenuStripMenuItem = new ToolStripMenuItem();
      this.commonContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.listViewDependencies.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeaderDependentComponentNum,
        this.columnHeaderDependentComponentName,
        this.columnHeaderDependentComponentArchitecture,
        this.columnHeaderDependentComponentVersion,
        this.columnHeaderDependentComponentLanguage
      });
      this.listViewDependencies.ContextMenuStrip = this.commonContextMenuStrip;
      this.listViewDependencies.Dock = DockStyle.Fill;
      this.listViewDependencies.FullRowSelect = true;
      this.listViewDependencies.GridLines = true;
      this.listViewDependencies.HideSelection = false;
      this.listViewDependencies.Location = new Point(0, 0);
      this.listViewDependencies.Name = "listViewDependencies";
      this.listViewDependencies.Size = new Size(390, 216);
      this.listViewDependencies.TabIndex = 1;
      this.listViewDependencies.UseCompatibleStateImageBehavior = false;
      this.listViewDependencies.View = View.Details;
      this.listViewDependencies.ColumnClick += new ColumnClickEventHandler(this.ListViewDependencies_ColumnClick);
      this.columnHeaderDependentComponentNum.Text = "No.";
      this.columnHeaderDependentComponentNum.Width = 35;
      this.columnHeaderDependentComponentName.Text = "Component Name";
      this.columnHeaderDependentComponentName.Width = 168;
      this.columnHeaderDependentComponentArchitecture.Text = "Processor Architecture";
      this.columnHeaderDependentComponentArchitecture.Width = 154;
      this.columnHeaderDependentComponentVersion.Text = "Version";
      this.columnHeaderDependentComponentVersion.Width = 120;
      this.columnHeaderDependentComponentLanguage.Text = "Language";
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
      this.copyTextContextMenuStripMenuItem.Click += new EventHandler(this.CopyTextContextMenuStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.listViewDependencies);
      this.Name = "DependencyListControl";
      this.Size = new Size(390, 216);
      this.commonContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void InitializeCustomUI()
    {
      this.listViewDependencies.ListViewItemSorter = (IComparer) this.columnSorterDependencies;
    }

    private void CopyTextContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.listViewDependencies.ContainsFocus)
        return;
      DependencyListControl.CopyListViewItemsToClipboard(this.listViewDependencies);
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
      WindowsComponent data = this.Data as WindowsComponent;
      if (data == null)
        return;
      int count = this.listViewDependencies.Items.Count;
      this.listViewDependencies.BeginUpdate();
      foreach (DependencyElement dependency in (IEnumerable<DependencyElement>) data.Dependencies)
      {
        if (dependency.DependentAssemblies != null && dependency.DependentAssemblies.Count != 0)
        {
          string name = dependency.DependentAssemblies[0].AssemblyIdentity.Name;
          string architecture = dependency.DependentAssemblies[0].AssemblyIdentity.Architecture;
          string version = dependency.DependentAssemblies[0].AssemblyIdentity.Version;
          string language = dependency.DependentAssemblies[0].AssemblyIdentity.Language;
          ListViewItem listViewItem = new ListViewItem(new string[5]
          {
            (count + 1).ToString((IFormatProvider) CultureInfo.CurrentCulture),
            name,
            architecture,
            version,
            language
          });
          listViewItem.Tag = (object) dependency;
          string str = name + "/" + architecture + "/" + version + "/" + language;
          listViewItem.Name = str;
          this.listViewDependencies.Items.Add(listViewItem);
          ++count;
        }
      }
      this.listViewDependencies.EndUpdate();
    }

    public void ClearView()
    {
      this.listViewDependencies.Items.Clear();
    }

    private void ListViewDependencies_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      ListViewColumnSorter.Sort(sender, e.Column);
    }

    private void CommonContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      this.copyTextContextMenuStripMenuItem.Enabled = this.listViewDependencies.SelectedItems.Count > 0;
    }

    public NavigationStatus NavigateToItem(ISearchProvider provider)
    {
      if (provider == null)
        return NavigationStatus.NotApplicable;
      DependencyElement dependencyElement = provider as DependencyElement;
      if (dependencyElement == null || dependencyElement.DependentAssemblies == null || dependencyElement.DependentAssemblies.Count == 0)
        return NavigationStatus.NotApplicable;
      string index = dependencyElement.DependentAssemblies[0].AssemblyIdentity.Name + "/" + dependencyElement.DependentAssemblies[0].AssemblyIdentity.Architecture + "/" + dependencyElement.DependentAssemblies[0].AssemblyIdentity.Version + "/" + dependencyElement.DependentAssemblies[0].AssemblyIdentity.Language;
      if (this.listViewDependencies.Items[index] == null)
        return NavigationStatus.Error;
      this.listViewDependencies.SelectedItems.Clear();
      this.listViewDependencies.Items[index].Selected = true;
      this.listViewDependencies.Items[index].Focused = true;
      return NavigationStatus.Success;
    }
  }
}
