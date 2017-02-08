// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.SmiSettingsTreeListControl
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
  public class SmiSettingsTreeListControl : TreeListControl, IFindNavigator
  {
    private IContainer components;
    private ToolStripMenuItem copyTextTreeViewContextMenuStripMenuItem;

    public SmiSettingsTreeListControl()
    {
      this.InitializeComponent();
      this.InitializeCustomUI();
    }

    private void InitializeCustomUI()
    {
      this.treeViewContextMenuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.copyTextTreeViewContextMenuStripMenuItem
      });
      this.treeView.Nodes.Add(new TreeNode("Configuration")
      {
        Name = "Configuration",
        NodeFont = new Font(this.Font, FontStyle.Bold),
        Nodes = {
          {
            "Elements",
            "Elements"
          },
          {
            "Complex Types",
            "Complex Types"
          },
          {
            "Simple Types",
            "Simple Types"
          }
        }
      });
      this.treeView.Nodes[0].Expand();
    }

    public override void UpdateView()
    {
      if (this.Data == null)
        return;
      WindowsComponent data = this.Data as WindowsComponent;
      if (data == null)
        return;
      foreach (XsdElement element in (IEnumerable<XsdElement>) data.SmiConfiguration.Elements)
        this.treeView.Nodes[0].Nodes[0].Nodes.Add(new TreeNode(element.Name)
        {
          Tag = (object) element,
          Name = element.Name
        });
      foreach (XsdComplexType complexType in (IEnumerable<XsdComplexType>) data.SmiConfiguration.ComplexTypes)
      {
        TreeNode node1 = new TreeNode(complexType.Name);
        node1.Tag = (object) complexType;
        node1.Name = complexType.Name;
        this.treeView.Nodes[0].Nodes[1].Nodes.Add(node1);
        foreach (XsdSequence sequence in (IEnumerable<XsdSequence>) complexType.Sequences)
        {
          TreeNode node2 = new TreeNode("Sequence");
          node2.Tag = (object) sequence;
          node2.Name = "Sequence";
          foreach (XsdElement element in (IEnumerable<XsdElement>) sequence.Elements)
            node2.Nodes.Add(new TreeNode(element.Name)
            {
              Tag = (object) element,
              Name = element.Name
            });
          node1.Nodes.Add(node2);
        }
      }
      foreach (XsdSimpleType simpleType in (IEnumerable<XsdSimpleType>) data.SmiConfiguration.SimpleTypes)
      {
        TreeNode node1 = new TreeNode(simpleType.Name);
        node1.Tag = (object) simpleType;
        node1.Name = simpleType.Name;
        this.treeView.Nodes[0].Nodes[2].Nodes.Add(node1);
        TreeNode node2 = new TreeNode("Restriction");
        node2.Tag = (object) simpleType.Restriction;
        node2.Name = "Restriction";
        node2.Nodes.Add(new TreeNode("Base = " + simpleType.Restriction.Base)
        {
          Tag = (object) simpleType.Restriction.Base,
          Name = simpleType.Restriction.Base
        });
        if (simpleType.Restriction.MinInclusive != null && simpleType.Restriction.MinInclusive.Value != null)
          node2.Nodes.Add(new TreeNode("MinInclusive = " + simpleType.Restriction.MinInclusive.Value)
          {
            Tag = (object) simpleType.Restriction.MinInclusive,
            Name = simpleType.Restriction.MinInclusive.Value
          });
        if (simpleType.Restriction.MaxInclusive != null && simpleType.Restriction.MaxInclusive.Value != null)
          node2.Nodes.Add(new TreeNode("MaxInclusive = " + simpleType.Restriction.MaxInclusive.Value)
          {
            Tag = (object) simpleType.Restriction.MaxInclusive,
            Name = simpleType.Restriction.MaxInclusive.Value
          });
        foreach (XsdEnumeration enumeration in (IEnumerable<XsdEnumeration>) simpleType.Restriction.Enumerations)
          node2.Nodes.Add(new TreeNode("Enumeration = " + enumeration.Value)
          {
            Tag = (object) enumeration,
            Name = enumeration.Value
          });
        node1.Nodes.Add(node2);
      }
      this.treeView.Nodes[0].Expand();
    }

    public override void ClearView()
    {
      this.treeView.Nodes[0].Nodes[0].Nodes.Clear();
      this.treeView.Nodes[0].Nodes[1].Nodes.Clear();
      this.treeView.Nodes[0].Nodes[2].Nodes.Clear();
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
      if (provider is SmiConfigurationElement)
      {
        this.treeView.SelectedNode = this.treeView.Nodes[0];
        return NavigationStatus.Success;
      }
      XsdSimpleType xsdSimpleType = provider as XsdSimpleType;
      if (xsdSimpleType != null)
      {
        string name = xsdSimpleType.Name;
        if (this.treeView.Nodes[0].Nodes["Simple Types"].Nodes[name] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.Nodes[0].Nodes["Simple Types"].Nodes[name];
        return NavigationStatus.Success;
      }
      if (provider is XsdRestriction)
      {
        if (this.treeView.SelectedNode.Nodes["Restriction"] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes["Restriction"];
        return NavigationStatus.Success;
      }
      XsdEnumeration xsdEnumeration = provider as XsdEnumeration;
      if (xsdEnumeration != null)
      {
        string index = xsdEnumeration.Value;
        if (this.treeView.SelectedNode.Nodes[index] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[index];
        return NavigationStatus.Success;
      }
      XsdMinInclusive xsdMinInclusive = provider as XsdMinInclusive;
      if (xsdMinInclusive != null)
      {
        string index = xsdMinInclusive.Value;
        if (this.treeView.SelectedNode.Nodes[index] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[index];
        return NavigationStatus.Success;
      }
      XsdMaxInclusive xsdMaxInclusive = provider as XsdMaxInclusive;
      if (xsdMaxInclusive != null)
      {
        string index = xsdMaxInclusive.Value;
        if (this.treeView.SelectedNode.Nodes[index] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[index];
        return NavigationStatus.Success;
      }
      XsdComplexType xsdComplexType = provider as XsdComplexType;
      if (xsdComplexType != null)
      {
        string name = xsdComplexType.Name;
        if (this.treeView.Nodes[0].Nodes["Complex Types"].Nodes[name] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.Nodes[0].Nodes["Complex Types"].Nodes[name];
        return NavigationStatus.Success;
      }
      if (provider is XsdSequence)
      {
        if (this.treeView.SelectedNode.Nodes["Sequence"] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes["Sequence"];
        return NavigationStatus.Success;
      }
      XsdElement xsdElement = provider as XsdElement;
      if (xsdElement == null)
        return NavigationStatus.NotApplicable;
      string name1 = xsdElement.Name;
      if (this.treeView.SelectedNode == this.treeView.Nodes[0])
        this.treeView.SelectedNode = this.treeView.Nodes[0].Nodes["Elements"];
      if (this.treeView.SelectedNode.Nodes[name1] == null)
        return NavigationStatus.Error;
      this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[name1];
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
      this.copyTextTreeViewContextMenuStripMenuItem = new ToolStripMenuItem();
      this.SuspendLayout();
      this.copyTextTreeViewContextMenuStripMenuItem.Name = "copyTextTreeViewContextMenuStripMenuItem";
      this.copyTextTreeViewContextMenuStripMenuItem.Size = new Size(152, 22);
      this.copyTextTreeViewContextMenuStripMenuItem.Text = "Copy Text";
      this.copyTextTreeViewContextMenuStripMenuItem.Click += new EventHandler(this.CopyTextTreeViewContextMenuStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = "SmiSettingsTreeListControl";
      this.ResumeLayout(false);
    }
  }
}
