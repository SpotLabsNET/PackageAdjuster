// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.FileListControl
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
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PackageInspector.Controls
{
  public class FileListControl : UserControl, IItemViewer, IFindNavigator
  {
    private ListViewColumnSorter columnSorterFiles = new ListViewColumnSorter();
    private IContainer components;
    private ListView listViewFiles;
    private ColumnHeader columnHeaderFileNum;
    private ColumnHeader columnHeaderFileName;
    private ColumnHeader columnHeaderDestinationPath;
    private ColumnHeader columnHeaderSourcePath;
    private ColumnHeader columnHeaderImportPath;
    private ContextMenuStrip commonContextMenuStrip;
    private ToolStripMenuItem copyTextContextMenuStripMenuItem;
    private ToolStripMenuItem openContainingFolderContextMenuStripMenuItem;

    public object Data { get; set; }

    public FileListControl()
    {
      this.InitializeComponent();
      this.InitializeCustomUI();
    }

    private void InitializeCustomUI()
    {
      this.listViewFiles.ListViewItemSorter = (IComparer) this.columnSorterFiles;
    }

    private void OpenContainingFolderContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.listViewFiles.SelectedItems.Count != 1)
        return;
      WindowsComponent tag = (WindowsComponent) this.listViewFiles.SelectedItems[0].Tag;
      string str = Path.GetDirectoryName(tag.ManifestFile) + "\\" + Path.GetFileNameWithoutExtension(tag.ManifestFile);
      if (!Directory.Exists(str))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Unable to find the containing folder. This option does not work when inspecting .manifest or .man files", new object[0]), "CBS Package Inspector", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
      }
      else
        Process.Start("explorer.exe", str);
    }

    private void CopyTextContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.listViewFiles.ContainsFocus)
        return;
      FileListControl.CopyListViewItemsToClipboard(this.listViewFiles);
    }

    private void CommonContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      int count = this.listViewFiles.SelectedItems.Count;
      this.copyTextContextMenuStripMenuItem.Enabled = count > 0;
      this.openContainingFolderContextMenuStripMenuItem.Enabled = count == 1;
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
      int count = this.listViewFiles.Items.Count;
      this.listViewFiles.BeginUpdate();
      foreach (FileElement file in (IEnumerable<FileElement>) data.Files)
      {
        ListViewItem listViewItem = new ListViewItem(new string[5]
        {
          (count + 1).ToString((IFormatProvider) CultureInfo.CurrentCulture),
          file.Name,
          file.DestinationPath,
          file.SourcePath,
          file.ImportPath
        });
        listViewItem.Tag = (object) data;
        string str = file.Name + "/" + file.DestinationPath + "/" + file.SourcePath + "/" + file.ImportPath;
        listViewItem.Name = str;
        this.listViewFiles.Items.Add(listViewItem);
        ++count;
      }
      this.listViewFiles.EndUpdate();
    }

    public void ClearView()
    {
      this.listViewFiles.Items.Clear();
    }

    private void ListViewFiles_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      ListViewColumnSorter.Sort(sender, e.Column);
    }

    public NavigationStatus NavigateToItem(ISearchProvider provider)
    {
      if (provider == null)
        return NavigationStatus.NotApplicable;
      FileElement fileElement = provider as FileElement;
      if (fileElement == null)
        return NavigationStatus.NotApplicable;
      string index = fileElement.Name + "/" + fileElement.DestinationPath + "/" + fileElement.SourcePath + "/" + fileElement.ImportPath;
      if (this.listViewFiles.Items[index] == null)
        return NavigationStatus.Error;
      this.listViewFiles.SelectedItems.Clear();
      this.listViewFiles.Items[index].Selected = true;
      this.listViewFiles.Items[index].Focused = true;
      return NavigationStatus.Success;
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
      this.listViewFiles = new ListView();
      this.columnHeaderFileNum = new ColumnHeader();
      this.columnHeaderFileName = new ColumnHeader();
      this.columnHeaderDestinationPath = new ColumnHeader();
      this.columnHeaderSourcePath = new ColumnHeader();
      this.columnHeaderImportPath = new ColumnHeader();
      this.commonContextMenuStrip = new ContextMenuStrip(this.components);
      this.copyTextContextMenuStripMenuItem = new ToolStripMenuItem();
      this.openContainingFolderContextMenuStripMenuItem = new ToolStripMenuItem();
      this.commonContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.listViewFiles.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeaderFileNum,
        this.columnHeaderFileName,
        this.columnHeaderDestinationPath,
        this.columnHeaderSourcePath,
        this.columnHeaderImportPath
      });
      this.listViewFiles.ContextMenuStrip = this.commonContextMenuStrip;
      this.listViewFiles.Dock = DockStyle.Fill;
      this.listViewFiles.FullRowSelect = true;
      this.listViewFiles.GridLines = true;
      this.listViewFiles.HideSelection = false;
      this.listViewFiles.Location = new Point(0, 0);
      this.listViewFiles.Name = "listViewFiles";
      this.listViewFiles.Size = new Size(618, 258);
      this.listViewFiles.TabIndex = 1;
      this.listViewFiles.UseCompatibleStateImageBehavior = false;
      this.listViewFiles.View = View.Details;
      this.listViewFiles.ColumnClick += new ColumnClickEventHandler(this.ListViewFiles_ColumnClick);
      this.columnHeaderFileNum.Text = "No.";
      this.columnHeaderFileNum.Width = 35;
      this.columnHeaderFileName.Text = "Name";
      this.columnHeaderFileName.Width = 134;
      this.columnHeaderDestinationPath.Text = "Destination Path";
      this.columnHeaderDestinationPath.Width = 130;
      this.columnHeaderSourcePath.Text = "Source Path";
      this.columnHeaderSourcePath.Width = 151;
      this.columnHeaderImportPath.Text = "Import Path";
      this.columnHeaderImportPath.Width = 150;
      this.commonContextMenuStrip.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.copyTextContextMenuStripMenuItem,
        (ToolStripItem) this.openContainingFolderContextMenuStripMenuItem
      });
      this.commonContextMenuStrip.Name = "commonContextMenuStrip";
      this.commonContextMenuStrip.Size = new Size(202, 48);
      this.commonContextMenuStrip.Opening += new CancelEventHandler(this.CommonContextMenuStrip_Opening);
      this.copyTextContextMenuStripMenuItem.Name = "copyTextContextMenuStripMenuItem";
      this.copyTextContextMenuStripMenuItem.Size = new Size(201, 22);
      this.copyTextContextMenuStripMenuItem.Text = "Copy Text";
      this.copyTextContextMenuStripMenuItem.Click += new EventHandler(this.CopyTextContextMenuStripMenuItem_Click);
      this.openContainingFolderContextMenuStripMenuItem.Name = "openContainingFolderContextMenuStripMenuItem";
      this.openContainingFolderContextMenuStripMenuItem.Size = new Size(201, 22);
      this.openContainingFolderContextMenuStripMenuItem.Text = "Open Containing Folder";
      this.openContainingFolderContextMenuStripMenuItem.Click += new EventHandler(this.OpenContainingFolderContextMenuStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.listViewFiles);
      this.Name = "FileListControl";
      this.Size = new Size(618, 258);
      this.commonContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
