// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.MumTreeListControl
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PackageInspector.Controls
{
  public class MumTreeListControl : TreeListControl, IFindNavigator
  {
    private IContainer components;
    private ToolStripMenuItem copyTextTreeViewContextMenuStripMenuItem;
    private ToolStripMenuItem copyMumFilePathTreeViewContextMenuStripMenuItem;
    private ToolStripMenuItem openMumFileTreeViewContextMenuStripMenuItem;

    public MumTreeListControl()
    {
      this.InitializeComponent();
      this.InitializeCustomUI();
    }

    private void InitializeCustomUI()
    {
      this.treeViewContextMenuStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.copyTextTreeViewContextMenuStripMenuItem,
        (ToolStripItem) this.copyMumFilePathTreeViewContextMenuStripMenuItem,
        (ToolStripItem) this.openMumFileTreeViewContextMenuStripMenuItem
      });
      this.treeView.Nodes.Add(new TreeNode("Package Manifests")
      {
        NodeFont = new Font(this.Font, FontStyle.Bold)
      });
    }

    public override void UpdateView()
    {
      if (this.Data == null)
        return;
      IList<MumElement> data = this.Data as IList<MumElement>;
      if (data == null)
        return;
      this.treeView.BeginUpdate();
      foreach (MumElement mumElement in (IEnumerable<MumElement>) data)
      {
        TreeNode node1 = new TreeNode(mumElement.AssemblyIdentity.Name);
        node1.Tag = (object) mumElement;
        string str1 = mumElement.AssemblyIdentity.Name + "/" + mumElement.MumFile;
        node1.Name = str1;
        TreeNode node2 = new TreeNode("AssemblyIdentity");
        node2.Tag = (object) mumElement.AssemblyIdentity;
        string str2 = mumElement.AssemblyIdentity.Name + "/" + mumElement.AssemblyIdentity.Architecture + "/" + mumElement.AssemblyIdentity.Version + "/" + mumElement.AssemblyIdentity.Language + "/" + mumElement.AssemblyIdentity.VersionScope;
        node2.Name = str2;
        node1.Nodes.Add(node2);
        TreeNode node3 = new TreeNode("Package");
        node3.Tag = (object) mumElement.Package;
        node3.Name = "Package";
        TreeNode node4 = new TreeNode("Parents");
        node4.Tag = (object) mumElement.Package.Parents;
        node4.Name = "Parents";
        foreach (ParentElement parent in (IEnumerable<ParentElement>) mumElement.Package.Parents)
        {
          TreeNode node5 = new TreeNode("Parent");
          node5.Tag = (object) parent;
          node5.Name = "Parent";
          foreach (AssemblyIdentityElement assemblyIdentity in (IEnumerable<ParentAssemblyIdentityElement>) parent.AssemblyIdentities)
            node5.Nodes.Add(new TreeNode(assemblyIdentity.Name)
            {
              Tag = (object) assemblyIdentity,
              Name = assemblyIdentity.Name + "/" + assemblyIdentity.Architecture + "/" + assemblyIdentity.Version + "/" + assemblyIdentity.Language + "/" + assemblyIdentity.VersionScope
            });
          node4.Nodes.Add(node5);
        }
        node3.Nodes.Add(node4);
        TreeNode node6 = new TreeNode("Updates");
        node6.Tag = (object) mumElement.Package.Updates;
        node6.Name = "Updates";
        foreach (UpdateElement update in (IEnumerable<UpdateElement>) mumElement.Package.Updates)
        {
          TreeNode node5 = new TreeNode(update.Name);
          node5.Tag = (object) update;
          node5.Name = update.Name;
          node6.Nodes.Add(node5);
          if (update.DeploymentComponent != null)
          {
            TreeNode node7 = new TreeNode("Component - " + update.DeploymentComponent.AssemblyIdentity.Name);
            node7.Tag = (object) update.DeploymentComponent;
            string str3 = update.DeploymentComponent.AssemblyIdentity.Name + "/" + update.DeploymentComponent.AssemblyIdentity.Architecture + "/" + update.DeploymentComponent.AssemblyIdentity.Version + "/" + update.DeploymentComponent.AssemblyIdentity.Language + "/" + update.DeploymentComponent.AssemblyIdentity.VersionScope;
            node7.Name = str3;
            node5.Nodes.Add(node7);
          }
          if (update.DeploymentPackage != null)
          {
            TreeNode node7 = new TreeNode("Package - " + update.DeploymentPackage.AssemblyIdentity.Name);
            node7.Tag = (object) update.DeploymentPackage;
            string str3 = update.DeploymentPackage.AssemblyIdentity.Name + "/" + update.DeploymentPackage.AssemblyIdentity.Architecture + "/" + update.DeploymentPackage.AssemblyIdentity.Version + "/" + update.DeploymentPackage.AssemblyIdentity.Language + "/" + update.DeploymentPackage.AssemblyIdentity.VersionScope;
            node7.Name = str3;
            node5.Nodes.Add(node7);
          }
          if (update.Driver != null)
          {
            TreeNode node7 = new TreeNode("Driver - " + update.Driver.AssemblyIdentity.Name);
            node7.Tag = (object) update.Driver;
            string str3 = update.Driver.AssemblyIdentity.Name + "/" + update.Driver.AssemblyIdentity.Architecture + "/" + update.Driver.AssemblyIdentity.Version + "/" + update.Driver.AssemblyIdentity.Language + "/" + update.Driver.AssemblyIdentity.VersionScope;
            node7.Name = str3;
            node5.Nodes.Add(node7);
          }
        }
        node3.Nodes.Add(node6);
        node1.Nodes.Add(node3);
        this.treeView.Nodes[0].Nodes.Add(node1);
      }
      this.treeView.EndUpdate();
      this.treeView.Nodes[0].Expand();
    }

    public override void ClearView()
    {
      this.listViewAttributeMap.Items.Clear();
      this.treeView.Nodes[0].Nodes.Clear();
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

    private void CopyMumFilePathTreeViewContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      string mumFilePath = this.GetMumFilePath();
      if (mumFilePath.Length == 0)
        return;
      Clipboard.SetText(mumFilePath, TextDataFormat.Text);
    }

    private string GetMumFilePath()
    {
      if (!this.treeView.ContainsFocus || this.treeView.SelectedNode == null)
        return string.Empty;
      TreeNode treeNode = this.treeView.SelectedNode;
      while (treeNode.Level > 1)
        treeNode = treeNode.Parent;
      if (treeNode.Level != 1)
        return string.Empty;
      return ((MumElement) treeNode.Tag).MumFile;
    }

    private void OpenMumFileTreeViewContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      string mumFilePath = this.GetMumFilePath();
      if (mumFilePath.Length == 0)
        return;
      string appSetting = ConfigurationSettings.AppSettings["mumFileViewerPath"];
      try
      {
        Process.Start(appSetting, mumFilePath);
      }
      catch (Win32Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
    }

    public NavigationStatus NavigateToItem(ISearchProvider provider)
    {
      if (provider == null)
        return NavigationStatus.NotApplicable;
      MumElement mumElement = provider as MumElement;
      if (mumElement != null)
      {
        string index = mumElement.AssemblyIdentity.Name + "/" + mumElement.MumFile;
        if (this.treeView.Nodes[0].Nodes[index] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.Nodes[0].Nodes[index];
        return NavigationStatus.Success;
      }
      if (provider is PackageElement)
      {
        if (this.treeView.SelectedNode.Nodes["Package"] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes["Package"];
        return NavigationStatus.Success;
      }
      if (provider is ParentElement)
      {
        if (this.treeView.SelectedNode.Nodes["Parents"] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes["Parents"];
        if (this.treeView.SelectedNode.Nodes["Parent"] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes["Parent"];
        return NavigationStatus.Success;
      }
      ParentAssemblyIdentityElement assemblyIdentityElement1 = provider as ParentAssemblyIdentityElement;
      if (assemblyIdentityElement1 != null)
      {
        string index = assemblyIdentityElement1.Name + "/" + assemblyIdentityElement1.Architecture + "/" + assemblyIdentityElement1.Version + "/" + assemblyIdentityElement1.Language + "/" + assemblyIdentityElement1.VersionScope;
        if (this.treeView.SelectedNode.Nodes[index] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[index];
        return NavigationStatus.Success;
      }
      MumAssemblyIdentityElement assemblyIdentityElement2 = provider as MumAssemblyIdentityElement;
      if (assemblyIdentityElement2 != null)
      {
        string index = assemblyIdentityElement2.Name + "/" + assemblyIdentityElement2.Architecture + "/" + assemblyIdentityElement2.Version + "/" + assemblyIdentityElement2.Language + "/" + assemblyIdentityElement2.VersionScope;
        if (this.treeView.SelectedNode.Nodes[index] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[index];
        return NavigationStatus.Success;
      }
      UpdateElement updateElement = provider as UpdateElement;
      if (updateElement != null)
      {
        if (this.treeView.SelectedNode.Nodes["Updates"] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes["Updates"];
        if (this.treeView.SelectedNode.Nodes[updateElement.Name] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[updateElement.Name];
        return NavigationStatus.Success;
      }
      DeploymentComponentElement componentElement = provider as DeploymentComponentElement;
      if (componentElement != null)
      {
        string index = componentElement.AssemblyIdentity.Name + "/" + componentElement.AssemblyIdentity.Architecture + "/" + componentElement.AssemblyIdentity.Version + "/" + componentElement.AssemblyIdentity.Language + "/" + componentElement.AssemblyIdentity.VersionScope;
        if (this.treeView.SelectedNode.Nodes[index] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[index];
        return NavigationStatus.Success;
      }
      DeploymentDriverElement deploymentDriverElement = provider as DeploymentDriverElement;
      if (deploymentDriverElement != null)
      {
        string index = deploymentDriverElement.AssemblyIdentity.Name + "/" + deploymentDriverElement.AssemblyIdentity.Architecture + "/" + deploymentDriverElement.AssemblyIdentity.Version + "/" + deploymentDriverElement.AssemblyIdentity.Language + "/" + deploymentDriverElement.AssemblyIdentity.VersionScope;
        if (this.treeView.SelectedNode.Nodes[index] == null)
          return NavigationStatus.Error;
        this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[index];
        return NavigationStatus.Success;
      }
      DeploymentPackageElement deploymentPackageElement = provider as DeploymentPackageElement;
      if (deploymentPackageElement == null)
        return NavigationStatus.NotApplicable;
      string index1 = deploymentPackageElement.AssemblyIdentity.Name + "/" + deploymentPackageElement.AssemblyIdentity.Architecture + "/" + deploymentPackageElement.AssemblyIdentity.Version + "/" + deploymentPackageElement.AssemblyIdentity.Language + "/" + deploymentPackageElement.AssemblyIdentity.VersionScope;
      if (this.treeView.SelectedNode.Nodes[index1] == null)
        return NavigationStatus.Error;
      this.treeView.SelectedNode = this.treeView.SelectedNode.Nodes[index1];
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
      this.copyMumFilePathTreeViewContextMenuStripMenuItem = new ToolStripMenuItem();
      this.openMumFileTreeViewContextMenuStripMenuItem = new ToolStripMenuItem();
      this.SuspendLayout();
      this.copyTextTreeViewContextMenuStripMenuItem.Name = "copyTextTreeViewContextMenuStripMenuItem";
      this.copyTextTreeViewContextMenuStripMenuItem.Size = new Size(185, 22);
      this.copyTextTreeViewContextMenuStripMenuItem.Text = "Copy Text";
      this.copyTextTreeViewContextMenuStripMenuItem.Click += new EventHandler(this.CopyTextTreeViewContextMenuStripMenuItem_Click);
      this.copyMumFilePathTreeViewContextMenuStripMenuItem.Name = "copyMumFilePathTreeViewContextMenuStripMenuItem";
      this.copyMumFilePathTreeViewContextMenuStripMenuItem.Size = new Size(185, 22);
      this.copyMumFilePathTreeViewContextMenuStripMenuItem.Text = "Copy .Mum File Path";
      this.copyMumFilePathTreeViewContextMenuStripMenuItem.Click += new EventHandler(this.CopyMumFilePathTreeViewContextMenuStripMenuItem_Click);
      this.openMumFileTreeViewContextMenuStripMenuItem.Name = "openMumFileTreeViewContextMenuStripMenuItem";
      this.openMumFileTreeViewContextMenuStripMenuItem.Size = new Size(185, 22);
      this.openMumFileTreeViewContextMenuStripMenuItem.Text = "Open .Mum File";
      this.openMumFileTreeViewContextMenuStripMenuItem.Click += new EventHandler(this.OpenMumFileTreeViewContextMenuStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = "MumTreeListControl";
      this.ResumeLayout(false);
    }
  }
}
