// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.TreeListControl
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PackageInspector.Controls
{
  public class TreeListControl : UserControl, IItemViewer
  {
    private IContainer components;
    private SplitContainer splitContainer;
    protected TreeView treeView;
    protected ToolStrip toolStripTreeView;
    protected ToolStripButton toolStripButtonExpandAll;
    protected ToolStripButton toolStripButtonCollapseAll;
    protected ListView listViewAttributeMap;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    protected ContextMenuStrip listViewContextMenuStrip;
    private ToolStripMenuItem copyTextContextMenuStripMenuItem;
    protected ContextMenuStrip treeViewContextMenuStrip;

    public object Data { get; set; }

    public TreeListControl()
    {
      this.InitializeComponent();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TreeListControl));
      this.splitContainer = new SplitContainer();
      this.treeView = new TreeView();
      this.treeViewContextMenuStrip = new ContextMenuStrip(this.components);
      this.toolStripTreeView = new ToolStrip();
      this.toolStripButtonExpandAll = new ToolStripButton();
      this.toolStripButtonCollapseAll = new ToolStripButton();
      this.listViewAttributeMap = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.listViewContextMenuStrip = new ContextMenuStrip(this.components);
      this.copyTextContextMenuStripMenuItem = new ToolStripMenuItem();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.toolStripTreeView.SuspendLayout();
      this.listViewContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.splitContainer.Dock = DockStyle.Fill;
      this.splitContainer.Location = new Point(0, 0);
      this.splitContainer.Name = "splitContainer";
      this.splitContainer.Panel1.Controls.Add((Control) this.treeView);
      this.splitContainer.Panel1.Controls.Add((Control) this.toolStripTreeView);
      this.splitContainer.Panel2.Controls.Add((Control) this.listViewAttributeMap);
      this.splitContainer.Size = new Size(603, 341);
      this.splitContainer.SplitterDistance = 297;
      this.splitContainer.TabIndex = 0;
      this.treeView.ContextMenuStrip = this.treeViewContextMenuStrip;
      this.treeView.Dock = DockStyle.Fill;
      this.treeView.HideSelection = false;
      this.treeView.Location = new Point(0, 25);
      this.treeView.Name = "treeView";
      this.treeView.Size = new Size(297, 316);
      this.treeView.TabIndex = 3;
      this.treeView.AfterSelect += new TreeViewEventHandler(this.TreeViewAfterSelect);
      this.treeViewContextMenuStrip.Name = "treeViewContextMenuStrip";
      this.treeViewContextMenuStrip.Size = new Size(61, 4);
      this.toolStripTreeView.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.toolStripButtonExpandAll,
        (ToolStripItem) this.toolStripButtonCollapseAll
      });
      this.toolStripTreeView.Location = new Point(0, 0);
      this.toolStripTreeView.Name = "toolStripTreeView";
      this.toolStripTreeView.Size = new Size(297, 25);
      this.toolStripTreeView.TabIndex = 4;
      this.toolStripTreeView.Text = "toolStrip1";
      this.toolStripButtonExpandAll.Image = (Image) componentResourceManager.GetObject("toolStripButtonExpandAll.Image");
      this.toolStripButtonExpandAll.ImageTransparentColor = Color.Magenta;
      this.toolStripButtonExpandAll.Name = "toolStripButtonExpandAll";
      this.toolStripButtonExpandAll.Size = new Size(82, 22);
      this.toolStripButtonExpandAll.Text = "Expand All";
      this.toolStripButtonExpandAll.Click += new EventHandler(this.ToolStripButtonExpandAllClick);
      this.toolStripButtonCollapseAll.Image = (Image) componentResourceManager.GetObject("toolStripButtonCollapseAll.Image");
      this.toolStripButtonCollapseAll.ImageTransparentColor = Color.Magenta;
      this.toolStripButtonCollapseAll.Name = "toolStripButtonCollapseAll";
      this.toolStripButtonCollapseAll.Size = new Size(89, 22);
      this.toolStripButtonCollapseAll.Text = "Collapse All";
      this.toolStripButtonCollapseAll.Click += new EventHandler(this.ToolStripButtonCollapseAllClick);
      this.listViewAttributeMap.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeader1,
        this.columnHeader2
      });
      this.listViewAttributeMap.ContextMenuStrip = this.listViewContextMenuStrip;
      this.listViewAttributeMap.Dock = DockStyle.Fill;
      this.listViewAttributeMap.FullRowSelect = true;
      this.listViewAttributeMap.GridLines = true;
      this.listViewAttributeMap.HideSelection = false;
      this.listViewAttributeMap.Location = new Point(0, 0);
      this.listViewAttributeMap.Name = "listViewAttributeMap";
      this.listViewAttributeMap.Size = new Size(302, 341);
      this.listViewAttributeMap.TabIndex = 3;
      this.listViewAttributeMap.UseCompatibleStateImageBehavior = false;
      this.listViewAttributeMap.View = View.Details;
      this.columnHeader1.Text = "Attribute Name";
      this.columnHeader1.Width = 165;
      this.columnHeader2.Text = "Attribute Value";
      this.columnHeader2.Width = 148;
      this.listViewContextMenuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.copyTextContextMenuStripMenuItem
      });
      this.listViewContextMenuStrip.Name = "commonContextMenuStrip";
      this.listViewContextMenuStrip.Size = new Size(128, 26);
      this.listViewContextMenuStrip.Opening += new CancelEventHandler(this.ListViewContextMenuStrip_Opening);
      this.copyTextContextMenuStripMenuItem.Name = "copyTextContextMenuStripMenuItem";
      this.copyTextContextMenuStripMenuItem.Size = new Size((int) sbyte.MaxValue, 22);
      this.copyTextContextMenuStripMenuItem.Text = "Copy Text";
      this.copyTextContextMenuStripMenuItem.Click += new EventHandler(this.CopyTextListViewContextMenuStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.splitContainer);
      this.Name = "TreeListControl";
      this.Size = new Size(603, 341);
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel1.PerformLayout();
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.ResumeLayout(false);
      this.toolStripTreeView.ResumeLayout(false);
      this.toolStripTreeView.PerformLayout();
      this.listViewContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public virtual void UpdateView()
    {
      throw new NotImplementedException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Please override this method in the derived classes.", new object[0]));
    }

    public virtual void ClearView()
    {
      throw new NotImplementedException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Please override this method in the derived classes.", new object[0]));
    }

    protected virtual void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
    {
      throw new NotImplementedException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Please override this method in the derived classes.", new object[0]));
    }

    protected virtual void ToolStripButtonExpandAllClick(object sender, EventArgs e)
    {
      if (this.treeView.SelectedNode == null)
      {
        if (this.treeView.Nodes == null || this.treeView.Nodes[0] == null)
          return;
        this.treeView.SelectedNode = this.treeView.Nodes[0];
      }
      this.treeView.BeginUpdate();
      this.treeView.SelectedNode.ExpandAll();
      this.treeView.EndUpdate();
    }

    protected virtual void ToolStripButtonCollapseAllClick(object sender, EventArgs e)
    {
      if (this.treeView.SelectedNode == null)
      {
        if (this.treeView.Nodes == null || this.treeView.Nodes[0] == null)
          return;
        this.treeView.SelectedNode = this.treeView.Nodes[0];
      }
      this.treeView.BeginUpdate();
      this.treeView.SelectedNode.Collapse(false);
      this.treeView.EndUpdate();
    }

    private void ListViewContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      if (!this.listViewAttributeMap.ContainsFocus)
        return;
      bool flag = false;
      if (this.listViewAttributeMap.SelectedItems.Count > 0)
        flag = true;
      this.copyTextContextMenuStripMenuItem.Enabled = flag;
    }

    private void CopyTextListViewContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.listViewAttributeMap.ContainsFocus)
        return;
      this.CopyListViewItemsToClipboard();
    }

    private void CopyListViewItemsToClipboard()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string appSetting = ConfigurationSettings.AppSettings["listviewCopyItemSeparator"];
      foreach (ListViewItem selectedItem in this.listViewAttributeMap.SelectedItems)
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
  }
}
