// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.GenericCommandsTreeListControl
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PackageInspector.Controls
{
  public class GenericCommandsTreeListControl : TreeListControl, IFindNavigator
  {
    private IContainer components;
    private ToolStripMenuItem copyTextTreeViewContextMenuStripMenuItem;

    public GenericCommandsTreeListControl()
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
      this.copyTextTreeViewContextMenuStripMenuItem = new ToolStripMenuItem();
      this.SuspendLayout();
      this.copyTextTreeViewContextMenuStripMenuItem.Name = "copyTextContextMenuStripMenuItem";
      this.copyTextTreeViewContextMenuStripMenuItem.Size = new Size(152, 22);
      this.copyTextTreeViewContextMenuStripMenuItem.Text = "Copy Text";
      this.copyTextTreeViewContextMenuStripMenuItem.Click += new EventHandler(this.CopyTextTreeViewContextMenuStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = "GenericCommandsTreeListControl";
      this.ResumeLayout(false);
    }

    private void InitializeCustomUI()
    {
      this.treeViewContextMenuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.copyTextTreeViewContextMenuStripMenuItem
      });
      this.treeView.Nodes.Add(new TreeNode("Commands")
      {
        NodeFont = new Font(this.Font, FontStyle.Bold)
      });
    }

    public override void UpdateView()
    {
      if (this.Data == null)
        return;
      WindowsComponent data = this.Data as WindowsComponent;
      if (data == null)
        return;
      foreach (GenericCommandElement genericCommand in (IEnumerable<GenericCommandElement>) data.GenericCommands)
      {
        TreeNode node = new TreeNode(genericCommand.ExecutableName);
        node.Tag = (object) genericCommand;
        string str = genericCommand.ExecutableName;
        foreach (KeyValuePair<string, string> attribute in (IEnumerable<KeyValuePair<string, string>>) genericCommand.AttributeMap)
          str = str + "/" + attribute.Key + "/";
        node.Name = str;
        this.treeView.Nodes[0].Nodes.Add(node);
      }
      this.treeView.Nodes[0].Expand();
    }

    public override void ClearView()
    {
      this.treeView.Nodes[0].Nodes.Clear();
      this.listViewAttributeMap.Items.Clear();
    }

    protected override void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
    {
      this.listViewAttributeMap.Items.Clear();
      if (e.Node.Level == 0)
        return;
      object tag = e.Node.Tag;
      if (tag == null)
        return;
      IAttributeType attributeType = tag as IAttributeType;
      if (attributeType == null)
        return;
      foreach (string key in (IEnumerable<string>) attributeType.AttributeMap.Keys)
        this.listViewAttributeMap.Items.Add(new ListViewItem(new string[2]
        {
          key,
          attributeType.AttributeMap[key]
        }));
    }

    private void CopyTextTreeViewContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.treeView.ContainsFocus)
        return;
      this.CopyTreeViewItemsToClipboard();
    }

    private void CopyTreeViewItemsToClipboard()
    {
      if (this.treeView.SelectedNode == null)
        return;
      Clipboard.SetText(this.treeView.SelectedNode.Text, TextDataFormat.Text);
    }

    public NavigationStatus NavigateToItem(ISearchProvider provider)
    {
      if (provider == null)
        return NavigationStatus.NotApplicable;
      GenericCommandElement genericCommandElement = provider as GenericCommandElement;
      if (genericCommandElement == null)
        return NavigationStatus.NotApplicable;
      string index = genericCommandElement.ExecutableName;
      foreach (KeyValuePair<string, string> attribute in (IEnumerable<KeyValuePair<string, string>>) genericCommandElement.AttributeMap)
        index = index + "/" + attribute.Key + "/";
      if (this.treeView.Nodes[0].Nodes[index] == null)
        return NavigationStatus.Error;
      this.treeView.SelectedNode = this.treeView.Nodes[0].Nodes[index];
      return NavigationStatus.Success;
    }
  }
}
