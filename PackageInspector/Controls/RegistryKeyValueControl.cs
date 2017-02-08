// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.RegistryKeyValueControl
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
  public class RegistryKeyValueControl : UserControl, IFindNavigator
  {
    private ListViewColumnSorter columnSorterRegistryKeys = new ListViewColumnSorter();
    private ListViewColumnSorter columnSorterRegistryValues = new ListViewColumnSorter();
    private IContainer components;
    private SplitContainer splitContainerRegistry;
    private ListView listViewRegistryKeys;
    private ColumnHeader columnHeaderRegistryKeysNum;
    private ColumnHeader columnHeaderRegistryKeyName;
    private ListView listViewRegistryValues;
    private ColumnHeader regValueViewerNo;
    private ColumnHeader columnHeaderValueName;
    private ColumnHeader columnHeaderValueType;
    private ColumnHeader columnHeaderValue;
    private Button buttonRegistryValuesTitle;
    private ContextMenuStrip commonContextMenuStrip;
    private ToolStripMenuItem copyTextContextMenuStripMenuItem;
    private ColumnHeader columnHeaderRegistryKeyOwner;
    private ColumnHeader columnHeaderValueOwner;
    private ColumnHeader columnHeaderValueOperationHint;

    public object Data { get; set; }

    public RegistryKeyValueControl()
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
      this.splitContainerRegistry = new SplitContainer();
      this.listViewRegistryKeys = new ListView();
      this.columnHeaderRegistryKeysNum = new ColumnHeader();
      this.columnHeaderRegistryKeyName = new ColumnHeader();
      this.commonContextMenuStrip = new ContextMenuStrip(this.components);
      this.copyTextContextMenuStripMenuItem = new ToolStripMenuItem();
      this.listViewRegistryValues = new ListView();
      this.regValueViewerNo = new ColumnHeader();
      this.columnHeaderValueName = new ColumnHeader();
      this.columnHeaderValueType = new ColumnHeader();
      this.columnHeaderValue = new ColumnHeader();
      this.buttonRegistryValuesTitle = new Button();
      this.columnHeaderRegistryKeyOwner = new ColumnHeader();
      this.columnHeaderValueOwner = new ColumnHeader();
      this.columnHeaderValueOperationHint = new ColumnHeader();
      this.splitContainerRegistry.Panel1.SuspendLayout();
      this.splitContainerRegistry.Panel2.SuspendLayout();
      this.splitContainerRegistry.SuspendLayout();
      this.commonContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.splitContainerRegistry.Dock = DockStyle.Fill;
      this.splitContainerRegistry.Location = new Point(0, 0);
      this.splitContainerRegistry.Name = "splitContainerRegistry";
      this.splitContainerRegistry.Panel1.Controls.Add((Control) this.listViewRegistryKeys);
      this.splitContainerRegistry.Panel2.Controls.Add((Control) this.listViewRegistryValues);
      this.splitContainerRegistry.Panel2.Controls.Add((Control) this.buttonRegistryValuesTitle);
      this.splitContainerRegistry.Size = new Size(556, 299);
      this.splitContainerRegistry.SplitterDistance = 241;
      this.splitContainerRegistry.TabIndex = 0;
      this.listViewRegistryKeys.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeaderRegistryKeysNum,
        this.columnHeaderRegistryKeyName,
        this.columnHeaderRegistryKeyOwner
      });
      this.listViewRegistryKeys.ContextMenuStrip = this.commonContextMenuStrip;
      this.listViewRegistryKeys.Dock = DockStyle.Fill;
      this.listViewRegistryKeys.FullRowSelect = true;
      this.listViewRegistryKeys.GridLines = true;
      this.listViewRegistryKeys.HideSelection = false;
      this.listViewRegistryKeys.Location = new Point(0, 0);
      this.listViewRegistryKeys.Name = "listViewRegistryKeys";
      this.listViewRegistryKeys.Size = new Size(241, 299);
      this.listViewRegistryKeys.TabIndex = 1;
      this.listViewRegistryKeys.UseCompatibleStateImageBehavior = false;
      this.listViewRegistryKeys.View = View.Details;
      this.listViewRegistryKeys.SelectedIndexChanged += new EventHandler(this.ListViewRegistryKeys_SelectedIndexChanged);
      this.listViewRegistryKeys.ColumnClick += new ColumnClickEventHandler(this.ListViewRegistryKeys_ColumnClick);
      this.columnHeaderRegistryKeysNum.Text = "No.";
      this.columnHeaderRegistryKeysNum.TextAlign = HorizontalAlignment.Center;
      this.columnHeaderRegistryKeysNum.Width = 35;
      this.columnHeaderRegistryKeyName.Text = "Key Name";
      this.columnHeaderRegistryKeyName.Width = 320;
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
      this.listViewRegistryValues.Columns.AddRange(new ColumnHeader[6]
      {
        this.regValueViewerNo,
        this.columnHeaderValueName,
        this.columnHeaderValueType,
        this.columnHeaderValue,
        this.columnHeaderValueOwner,
        this.columnHeaderValueOperationHint
      });
      this.listViewRegistryValues.ContextMenuStrip = this.commonContextMenuStrip;
      this.listViewRegistryValues.Dock = DockStyle.Fill;
      this.listViewRegistryValues.FullRowSelect = true;
      this.listViewRegistryValues.GridLines = true;
      this.listViewRegistryValues.HideSelection = false;
      this.listViewRegistryValues.Location = new Point(0, 23);
      this.listViewRegistryValues.Name = "listViewRegistryValues";
      this.listViewRegistryValues.Size = new Size(311, 276);
      this.listViewRegistryValues.TabIndex = 1;
      this.listViewRegistryValues.UseCompatibleStateImageBehavior = false;
      this.listViewRegistryValues.View = View.Details;
      this.listViewRegistryValues.ColumnClick += new ColumnClickEventHandler(this.ListViewRegistryValues_ColumnClick);
      this.regValueViewerNo.Text = "No.";
      this.regValueViewerNo.TextAlign = HorizontalAlignment.Center;
      this.regValueViewerNo.Width = 35;
      this.columnHeaderValueName.Text = "Value Name";
      this.columnHeaderValueName.Width = 150;
      this.columnHeaderValueType.Text = "Value Type";
      this.columnHeaderValueType.Width = 100;
      this.columnHeaderValue.Text = "Value";
      this.columnHeaderValue.Width = 250;
      this.buttonRegistryValuesTitle.BackColor = SystemColors.ControlLight;
      this.buttonRegistryValuesTitle.Dock = DockStyle.Top;
      this.buttonRegistryValuesTitle.FlatAppearance.BorderColor = SystemColors.ControlLight;
      this.buttonRegistryValuesTitle.FlatAppearance.BorderSize = 0;
      this.buttonRegistryValuesTitle.FlatAppearance.MouseDownBackColor = SystemColors.ControlLight;
      this.buttonRegistryValuesTitle.FlatAppearance.MouseOverBackColor = SystemColors.ControlLight;
      this.buttonRegistryValuesTitle.FlatStyle = FlatStyle.Flat;
      this.buttonRegistryValuesTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.buttonRegistryValuesTitle.Location = new Point(0, 0);
      this.buttonRegistryValuesTitle.Name = "buttonRegistryValuesTitle";
      this.buttonRegistryValuesTitle.Size = new Size(311, 23);
      this.buttonRegistryValuesTitle.TabIndex = 2;
      this.buttonRegistryValuesTitle.Text = "Registry Values";
      this.buttonRegistryValuesTitle.TextAlign = ContentAlignment.MiddleLeft;
      this.buttonRegistryValuesTitle.UseVisualStyleBackColor = false;
      this.columnHeaderRegistryKeyOwner.Text = "Owner";
      this.columnHeaderValueOwner.Text = "Owner";
      this.columnHeaderValueOperationHint.Text = "Operation Hint";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.splitContainerRegistry);
      this.Name = "RegistryKeyValueControl";
      this.Size = new Size(556, 299);
      this.splitContainerRegistry.Panel1.ResumeLayout(false);
      this.splitContainerRegistry.Panel2.ResumeLayout(false);
      this.splitContainerRegistry.ResumeLayout(false);
      this.commonContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void InitializeCustomUI()
    {
      this.listViewRegistryKeys.ListViewItemSorter = (IComparer) this.columnSorterRegistryKeys;
      this.listViewRegistryValues.ListViewItemSorter = (IComparer) this.columnSorterRegistryValues;
    }

    public virtual void UpdateView()
    {
      if (this.Data == null)
        return;
      WindowsComponent data = this.Data as WindowsComponent;
      if (data == null)
        return;
      int count = this.listViewRegistryKeys.Items.Count;
      this.listViewRegistryKeys.BeginUpdate();
      foreach (RegistryKeyElement registryKey in (IEnumerable<RegistryKeyElement>) data.RegistryKeys)
      {
        ListViewItem listViewItem = new ListViewItem(new string[3]
        {
          (count + 1).ToString((IFormatProvider) CultureInfo.CurrentCulture),
          registryKey.KeyName,
          registryKey.Owner
        });
        listViewItem.Tag = (object) registryKey;
        string str = registryKey.KeyName + "/" + registryKey.Owner;
        listViewItem.Name = str;
        this.listViewRegistryKeys.Items.Add(listViewItem);
        ++count;
      }
      this.listViewRegistryKeys.EndUpdate();
    }

    public virtual void ClearView()
    {
      this.listViewRegistryKeys.Items.Clear();
      this.listViewRegistryValues.Items.Clear();
    }

    private void ListViewRegistryKeys_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.LoadRegistryValuesOnKeyChange();
    }

    private void LoadRegistryValuesOnKeyChange()
    {
      if (this.listViewRegistryKeys.SelectedItems.Count != 1)
      {
        this.listViewRegistryValues.Items.Clear();
      }
      else
      {
        RegistryKeyElement tag = (RegistryKeyElement) this.listViewRegistryKeys.SelectedItems[0].Tag;
        this.listViewRegistryValues.Items.Clear();
        int num = 0;
        this.listViewRegistryValues.BeginUpdate();
        foreach (RegistryValueElement registryValue in (IEnumerable<RegistryValueElement>) tag.RegistryValues)
        {
          this.listViewRegistryValues.Items.Add(new ListViewItem(new string[6]
          {
            (num + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture),
            registryValue.Name,
            registryValue.ValueType,
            registryValue.Value,
            registryValue.Owner,
            registryValue.OperationHint
          })
          {
            Name = registryValue.Name + "/" + registryValue.ValueType + "/" + registryValue.Value + "/" + registryValue.Owner + "/" + registryValue.OperationHint,
            Tag = (object) registryValue
          });
          ++num;
        }
        this.listViewRegistryValues.EndUpdate();
      }
    }

    private void CommonContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      ListView listView = (ListView) null;
      bool flag = false;
      if (this.listViewRegistryKeys.ContainsFocus)
        listView = this.listViewRegistryKeys;
      else if (this.listViewRegistryValues.ContainsFocus)
        listView = this.listViewRegistryValues;
      if (listView != null && listView.SelectedItems.Count > 0)
        flag = true;
      this.copyTextContextMenuStripMenuItem.Enabled = flag;
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

    private void CopyTextContextMenuStripMenuItem_Click(object sender, EventArgs e)
    {
      ListView listView = (ListView) null;
      if (this.listViewRegistryKeys.ContainsFocus)
        listView = this.listViewRegistryKeys;
      else if (this.listViewRegistryValues.ContainsFocus)
        listView = this.listViewRegistryValues;
      if (listView == null)
        return;
      RegistryKeyValueControl.CopyListViewItemsToClipboard(listView);
    }

    private void ListViewRegistryValues_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      ListViewColumnSorter.Sort(sender, e.Column);
    }

    private void ListViewRegistryKeys_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      ListViewColumnSorter.Sort(sender, e.Column);
    }

    public NavigationStatus NavigateToItem(ISearchProvider provider)
    {
      if (provider == null)
        return NavigationStatus.NotApplicable;
      RegistryKeyElement registryKeyElement = provider as RegistryKeyElement;
      RegistryValueElement registryValueElement = provider as RegistryValueElement;
      if (registryKeyElement == null && registryValueElement == null)
        return NavigationStatus.NotApplicable;
      if (registryKeyElement != null)
        return this.SetSelectedRegistryKey(registryKeyElement);
      return this.SetSelectedRegistryValue(registryValueElement);
    }

    private NavigationStatus SetSelectedRegistryKey(RegistryKeyElement registryKeyElement)
    {
      string index = registryKeyElement.KeyName + "/" + registryKeyElement.Owner;
      if (this.listViewRegistryKeys.Items[index] == null)
        return NavigationStatus.Error;
      this.listViewRegistryKeys.SelectedItems.Clear();
      this.listViewRegistryKeys.Items[index].Selected = true;
      this.listViewRegistryKeys.Items[index].Focused = true;
      this.LoadRegistryValuesOnKeyChange();
      return NavigationStatus.Success;
    }

    private NavigationStatus SetSelectedRegistryValue(RegistryValueElement registryValueElement)
    {
      string index = registryValueElement.Name + "/" + registryValueElement.ValueType + "/" + registryValueElement.Value + "/" + registryValueElement.Owner + "/" + registryValueElement.OperationHint;
      if (this.listViewRegistryValues.Items[index] == null)
        return NavigationStatus.Error;
      this.listViewRegistryValues.Items[index].Selected = true;
      this.listViewRegistryValues.Items[index].Focused = true;
      return NavigationStatus.Success;
    }
  }
}
