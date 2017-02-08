// Decompiled with JetBrains decompiler
// Type: PackageInspector.FindDialog
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
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PackageInspector
{
  public class FindDialog : Form
  {
    private List<string> selectedSearchIdentifiers = new List<string>();
    private ListViewColumnSorter listViewColumnSorter = new ListViewColumnSorter();
    private IContainer components;
    private GroupBox groupBoxSearchCriteria;
    private Label labelItem;
    private ListView listViewFindResults;
    private ColumnHeader columnHeaderNumber;
    private ColumnHeader columnHeaderValue;
    private Button buttonClear;
    private Button buttonFind;
    private ColumnHeader columnHeaderPath;
    private Label labelFindStatus;
    private CheckBox checkBoxSMISettingsSearch;
    private CheckBox checkBoxPackageInfoSearch;
    private CheckBox checkBoxGenericCommandsSearch;
    private CheckBox checkBoxFileSearch;
    private CheckBox checkBoxComponentSearch;
    private CheckBox checkBoxRegistryKeySearch;
    private RadioButton radioButtonCustomSearch;
    private RadioButton radioButtonSearchAll;
    private ContextMenuStrip copyContextMenuStrip;
    private ToolStripMenuItem copyTextToolStripMenuItem;
    private Label labelNote2;
    private ComboBox comboBoxItemToFind;

    public IDictionary<string, IList<WindowsComponent>> PackageAndComponentMap { get; set; }

    public IDictionary<string, IList<MumElement>> PackageAndMumMap { get; set; }

    public bool EnableClearResults { get; set; }

    public IList<FoundItem> FoundItemList { get; private set; }

    public FoundItem SelectedFoundItem { get; private set; }

    public FindDialog()
    {
      this.InitializeComponent();
      this.FoundItemList = (IList<FoundItem>) new List<FoundItem>();
      this.PackageAndComponentMap = (IDictionary<string, IList<WindowsComponent>>) new Dictionary<string, IList<WindowsComponent>>();
      this.PackageAndMumMap = (IDictionary<string, IList<MumElement>>) new Dictionary<string, IList<MumElement>>();
      this.listViewFindResults.ListViewItemSorter = (IComparer) this.listViewColumnSorter;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FindDialog));
      this.groupBoxSearchCriteria = new GroupBox();
      this.checkBoxSMISettingsSearch = new CheckBox();
      this.checkBoxPackageInfoSearch = new CheckBox();
      this.checkBoxGenericCommandsSearch = new CheckBox();
      this.checkBoxFileSearch = new CheckBox();
      this.checkBoxComponentSearch = new CheckBox();
      this.checkBoxRegistryKeySearch = new CheckBox();
      this.labelItem = new Label();
      this.listViewFindResults = new ListView();
      this.columnHeaderNumber = new ColumnHeader();
      this.columnHeaderValue = new ColumnHeader();
      this.columnHeaderPath = new ColumnHeader();
      this.copyContextMenuStrip = new ContextMenuStrip(this.components);
      this.copyTextToolStripMenuItem = new ToolStripMenuItem();
      this.buttonClear = new Button();
      this.buttonFind = new Button();
      this.labelFindStatus = new Label();
      this.radioButtonCustomSearch = new RadioButton();
      this.radioButtonSearchAll = new RadioButton();
      this.labelNote2 = new Label();
      this.comboBoxItemToFind = new ComboBox();
      this.groupBoxSearchCriteria.SuspendLayout();
      this.copyContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.groupBoxSearchCriteria.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxSearchCriteria.Controls.Add((Control) this.checkBoxSMISettingsSearch);
      this.groupBoxSearchCriteria.Controls.Add((Control) this.checkBoxPackageInfoSearch);
      this.groupBoxSearchCriteria.Controls.Add((Control) this.checkBoxGenericCommandsSearch);
      this.groupBoxSearchCriteria.Controls.Add((Control) this.checkBoxFileSearch);
      this.groupBoxSearchCriteria.Controls.Add((Control) this.checkBoxComponentSearch);
      this.groupBoxSearchCriteria.Controls.Add((Control) this.checkBoxRegistryKeySearch);
      this.groupBoxSearchCriteria.Location = new Point(236, 62);
      this.groupBoxSearchCriteria.Name = "groupBoxSearchCriteria";
      this.groupBoxSearchCriteria.Size = new Size(302, 94);
      this.groupBoxSearchCriteria.TabIndex = 13;
      this.groupBoxSearchCriteria.TabStop = false;
      this.groupBoxSearchCriteria.Text = "Search for";
      this.checkBoxSMISettingsSearch.AutoSize = true;
      this.checkBoxSMISettingsSearch.Checked = true;
      this.checkBoxSMISettingsSearch.CheckState = CheckState.Checked;
      this.checkBoxSMISettingsSearch.Location = new Point(137, 19);
      this.checkBoxSMISettingsSearch.Name = "checkBoxSMISettingsSearch";
      this.checkBoxSMISettingsSearch.Size = new Size(86, 17);
      this.checkBoxSMISettingsSearch.TabIndex = 17;
      this.checkBoxSMISettingsSearch.Text = "SMI Settings";
      this.checkBoxSMISettingsSearch.UseVisualStyleBackColor = true;
      this.checkBoxPackageInfoSearch.AutoSize = true;
      this.checkBoxPackageInfoSearch.Checked = true;
      this.checkBoxPackageInfoSearch.CheckState = CheckState.Checked;
      this.checkBoxPackageInfoSearch.Location = new Point(137, 42);
      this.checkBoxPackageInfoSearch.Name = "checkBoxPackageInfoSearch";
      this.checkBoxPackageInfoSearch.Size = new Size(162, 17);
      this.checkBoxPackageInfoSearch.TabIndex = 18;
      this.checkBoxPackageInfoSearch.Text = "Package Applicability (MUM)";
      this.checkBoxPackageInfoSearch.UseVisualStyleBackColor = true;
      this.checkBoxGenericCommandsSearch.AutoSize = true;
      this.checkBoxGenericCommandsSearch.Checked = true;
      this.checkBoxGenericCommandsSearch.CheckState = CheckState.Checked;
      this.checkBoxGenericCommandsSearch.Location = new Point(137, 65);
      this.checkBoxGenericCommandsSearch.Name = "checkBoxGenericCommandsSearch";
      this.checkBoxGenericCommandsSearch.Size = new Size(118, 17);
      this.checkBoxGenericCommandsSearch.TabIndex = 19;
      this.checkBoxGenericCommandsSearch.Text = "Generic Commands";
      this.checkBoxGenericCommandsSearch.UseVisualStyleBackColor = true;
      this.checkBoxFileSearch.AutoSize = true;
      this.checkBoxFileSearch.Checked = true;
      this.checkBoxFileSearch.CheckState = CheckState.Checked;
      this.checkBoxFileSearch.Location = new Point(12, 21);
      this.checkBoxFileSearch.Name = "checkBoxFileSearch";
      this.checkBoxFileSearch.Size = new Size(47, 17);
      this.checkBoxFileSearch.TabIndex = 14;
      this.checkBoxFileSearch.Text = "Files";
      this.checkBoxFileSearch.UseVisualStyleBackColor = true;
      this.checkBoxComponentSearch.AutoSize = true;
      this.checkBoxComponentSearch.Checked = true;
      this.checkBoxComponentSearch.CheckState = CheckState.Checked;
      this.checkBoxComponentSearch.Location = new Point(12, 44);
      this.checkBoxComponentSearch.Name = "checkBoxComponentSearch";
      this.checkBoxComponentSearch.Size = new Size(85, 17);
      this.checkBoxComponentSearch.TabIndex = 15;
      this.checkBoxComponentSearch.Text = "Components";
      this.checkBoxComponentSearch.UseVisualStyleBackColor = true;
      this.checkBoxRegistryKeySearch.AutoSize = true;
      this.checkBoxRegistryKeySearch.Checked = true;
      this.checkBoxRegistryKeySearch.CheckState = CheckState.Checked;
      this.checkBoxRegistryKeySearch.Location = new Point(12, 67);
      this.checkBoxRegistryKeySearch.Name = "checkBoxRegistryKeySearch";
      this.checkBoxRegistryKeySearch.Size = new Size(90, 17);
      this.checkBoxRegistryKeySearch.TabIndex = 16;
      this.checkBoxRegistryKeySearch.Text = "Registry Keys";
      this.checkBoxRegistryKeySearch.UseVisualStyleBackColor = true;
      this.labelItem.AutoSize = true;
      this.labelItem.Location = new Point(22, 34);
      this.labelItem.Name = "labelItem";
      this.labelItem.Size = new Size(56, 13);
      this.labelItem.TabIndex = 14;
      this.labelItem.Text = "Find what:";
      this.listViewFindResults.Activation = ItemActivation.TwoClick;
      this.listViewFindResults.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeaderNumber,
        this.columnHeaderValue,
        this.columnHeaderPath
      });
      this.listViewFindResults.ContextMenuStrip = this.copyContextMenuStrip;
      this.listViewFindResults.FullRowSelect = true;
      this.listViewFindResults.GridLines = true;
      this.listViewFindResults.HideSelection = false;
      this.listViewFindResults.Location = new Point(25, 190);
      this.listViewFindResults.MultiSelect = false;
      this.listViewFindResults.Name = "listViewFindResults";
      this.listViewFindResults.Size = new Size(666, 226);
      this.listViewFindResults.TabIndex = 15;
      this.listViewFindResults.UseCompatibleStateImageBehavior = false;
      this.listViewFindResults.View = View.Details;
      this.listViewFindResults.ItemActivate += new EventHandler(this.ListViewFindResults_ItemActivate);
      this.listViewFindResults.ColumnClick += new ColumnClickEventHandler(this.ListViewFindResults_ColumnClick);
      this.columnHeaderNumber.Text = "No.";
      this.columnHeaderNumber.Width = 35;
      this.columnHeaderValue.Text = "Value";
      this.columnHeaderValue.Width = 199;
      this.columnHeaderPath.Text = "Path";
      this.columnHeaderPath.Width = 416;
      this.copyContextMenuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.copyTextToolStripMenuItem
      });
      this.copyContextMenuStrip.Name = "copyContextMenuStrip";
      this.copyContextMenuStrip.Size = new Size(128, 26);
      this.copyTextToolStripMenuItem.Name = "copyTextToolStripMenuItem";
      this.copyTextToolStripMenuItem.Size = new Size((int) sbyte.MaxValue, 22);
      this.copyTextToolStripMenuItem.Text = "Copy Text";
      this.copyTextToolStripMenuItem.Click += new EventHandler(this.CopyTextToolStripMenuItem_Click);
      this.buttonClear.Location = new Point(616, 29);
      this.buttonClear.Name = "buttonClear";
      this.buttonClear.Size = new Size(75, 23);
      this.buttonClear.TabIndex = 19;
      this.buttonClear.Text = "Clear";
      this.buttonClear.UseVisualStyleBackColor = true;
      this.buttonClear.Click += new EventHandler(this.ButtonClear_Click);
      this.buttonFind.Location = new Point(540, 29);
      this.buttonFind.Name = "buttonFind";
      this.buttonFind.Size = new Size(75, 23);
      this.buttonFind.TabIndex = 18;
      this.buttonFind.Text = "Find";
      this.buttonFind.UseVisualStyleBackColor = true;
      this.buttonFind.Click += new EventHandler(this.ButtonFind_Click);
      this.labelFindStatus.AutoSize = true;
      this.labelFindStatus.Location = new Point(25, 167);
      this.labelFindStatus.Name = "labelFindStatus";
      this.labelFindStatus.Size = new Size(43, 13);
      this.labelFindStatus.TabIndex = 20;
      this.labelFindStatus.Text = "Status: ";
      this.radioButtonCustomSearch.AutoSize = true;
      this.radioButtonCustomSearch.Checked = true;
      this.radioButtonCustomSearch.Location = new Point(25, 91);
      this.radioButtonCustomSearch.Name = "radioButtonCustomSearch";
      this.radioButtonCustomSearch.Size = new Size(95, 17);
      this.radioButtonCustomSearch.TabIndex = 23;
      this.radioButtonCustomSearch.TabStop = true;
      this.radioButtonCustomSearch.Text = "Custom search";
      this.radioButtonCustomSearch.UseVisualStyleBackColor = true;
      this.radioButtonCustomSearch.CheckedChanged += new EventHandler(this.RadioButtonCustomSearch_CheckedChanged);
      this.radioButtonSearchAll.AutoSize = true;
      this.radioButtonSearchAll.Location = new Point(25, 67);
      this.radioButtonSearchAll.Name = "radioButtonSearchAll";
      this.radioButtonSearchAll.Size = new Size(199, 17);
      this.radioButtonSearchAll.TabIndex = 22;
      this.radioButtonSearchAll.Text = "Search all entities (files, reg keys etc)";
      this.radioButtonSearchAll.UseVisualStyleBackColor = true;
      this.radioButtonSearchAll.CheckedChanged += new EventHandler(this.RadioButtonSearchAll_CheckedChanged);
      this.labelNote2.AutoSize = true;
      this.labelNote2.Location = new Point(25, 430);
      this.labelNote2.Name = "labelNote2";
      this.labelNote2.Size = new Size(257, 13);
      this.labelNote2.TabIndex = 24;
      this.labelNote2.Text = "Note: Search is confined to inspected packages only";
      this.comboBoxItemToFind.FormattingEnabled = true;
      this.comboBoxItemToFind.Location = new Point(84, 30);
      this.comboBoxItemToFind.Name = "comboBoxItemToFind";
      this.comboBoxItemToFind.Size = new Size(454, 21);
      this.comboBoxItemToFind.TabIndex = 25;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(713, 464);
      this.Controls.Add((Control) this.comboBoxItemToFind);
      this.Controls.Add((Control) this.labelNote2);
      this.Controls.Add((Control) this.radioButtonCustomSearch);
      this.Controls.Add((Control) this.radioButtonSearchAll);
      this.Controls.Add((Control) this.labelFindStatus);
      this.Controls.Add((Control) this.buttonClear);
      this.Controls.Add((Control) this.buttonFind);
      this.Controls.Add((Control) this.listViewFindResults);
      this.Controls.Add((Control) this.labelItem);
      this.Controls.Add((Control) this.groupBoxSearchCriteria);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FindDialog";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Find";
      this.Load += new EventHandler(this.FindDialog_Load);
      this.KeyDown += new KeyEventHandler(this.FindDialog_KeyDown);
      this.groupBoxSearchCriteria.ResumeLayout(false);
      this.groupBoxSearchCriteria.PerformLayout();
      this.copyContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void ButtonFind_Click(object sender, EventArgs e)
    {
      this.BeginSearch();
    }

    private void BeginSearch()
    {
      if (this.comboBoxItemToFind.Text.Length == 0)
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "Please enter an item to find.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      }
      else
      {
        if (!this.comboBoxItemToFind.Items.Contains((object) this.comboBoxItemToFind.Text))
          this.comboBoxItemToFind.Items.Add((object) this.comboBoxItemToFind.Text);
        this.listViewFindResults.Items.Clear();
        this.FoundItemList.Clear();
        this.SelectedFoundItem = (FoundItem) null;
        this.UseWaitCursor = true;
        string[] strArray = this.comboBoxItemToFind.Text.Split(new char[2]
        {
          '+',
          ','
        }, StringSplitOptions.RemoveEmptyEntries);
        IList<string> searchItems = (IList<string>) new List<string>();
        foreach (string str in strArray)
          searchItems.Add(str.Trim());
        this.selectedSearchIdentifiers.Clear();
        if (this.radioButtonCustomSearch.Checked)
        {
          if (this.checkBoxComponentSearch.Checked)
          {
            this.selectedSearchIdentifiers.Add("Component");
            this.selectedSearchIdentifiers.Add("Dependent Component");
          }
          if (this.checkBoxFileSearch.Checked)
            this.selectedSearchIdentifiers.Add("File");
          if (this.checkBoxRegistryKeySearch.Checked)
            this.selectedSearchIdentifiers.Add("Registry Key");
          if (this.checkBoxPackageInfoSearch.Checked)
            this.selectedSearchIdentifiers.Add("Package Applicability (MUM)");
          if (this.checkBoxSMISettingsSearch.Checked)
            this.selectedSearchIdentifiers.Add("SMI Setting");
          if (this.checkBoxGenericCommandsSearch.Checked)
            this.selectedSearchIdentifiers.Add("Generic Command");
        }
        else if (this.radioButtonSearchAll.Checked)
        {
          this.selectedSearchIdentifiers.Add("Component");
          this.selectedSearchIdentifiers.Add("Dependent Component");
          this.selectedSearchIdentifiers.Add("File");
          this.selectedSearchIdentifiers.Add("Registry Key");
          this.selectedSearchIdentifiers.Add("Package Applicability (MUM)");
          this.selectedSearchIdentifiers.Add("SMI Setting");
          this.selectedSearchIdentifiers.Add("Generic Command");
        }
        foreach (string key in (IEnumerable<string>) this.PackageAndComponentMap.Keys)
        {
          FileInfo fileInfo = new FileInfo(key);
          foreach (ISearchProvider searchProvider in (IEnumerable<WindowsComponent>) this.PackageAndComponentMap[key])
          {
            IList<ISearchProvider> searchProviderPath = (IList<ISearchProvider>) new List<ISearchProvider>();
            searchProviderPath.Add(searchProvider);
            this.TraverseSearchProviders(searchProvider, searchItems, fileInfo.Name, searchProviderPath);
          }
        }
        foreach (string key in (IEnumerable<string>) this.PackageAndMumMap.Keys)
        {
          FileInfo fileInfo = new FileInfo(key);
          foreach (ISearchProvider searchProvider in (IEnumerable<MumElement>) this.PackageAndMumMap[key])
          {
            IList<ISearchProvider> searchProviderPath = (IList<ISearchProvider>) new List<ISearchProvider>();
            searchProviderPath.Add(searchProvider);
            this.TraverseSearchProviders(searchProvider, searchItems, fileInfo.Name, searchProviderPath);
          }
        }
        int num2 = 0;
        this.listViewFindResults.ListViewItemSorter = (IComparer) null;
        this.listViewFindResults.BeginUpdate();
        foreach (FoundItem foundItem in (IEnumerable<FoundItem>) this.FoundItemList)
        {
          this.listViewFindResults.Items.Add(new ListViewItem(new string[3]
          {
            (num2 + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture),
            foundItem.Keyword,
            foundItem.Path
          })
          {
            Tag = (object) foundItem
          });
          ++num2;
        }
        this.listViewFindResults.EndUpdate();
        this.listViewFindResults.ListViewItemSorter = (IComparer) this.listViewColumnSorter;
        this.UseWaitCursor = false;
        this.labelFindStatus.Text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Status: {0} item(s) found.", new object[1]
        {
          (object) this.listViewFindResults.Items.Count
        });
      }
    }

    private void TraverseSearchProviders(ISearchProvider searchProvider, IList<string> searchItems, string searchPath, IList<ISearchProvider> searchProviderPath)
    {
      if (searchProvider == null)
        return;
      if (this.selectedSearchIdentifiers.Contains(searchProvider.SearchIdentifier))
      {
        foreach (string keyword in (IEnumerable<string>) searchProvider.Keywords)
        {
          foreach (string searchItem in (IEnumerable<string>) searchItems)
          {
            if (keyword.IndexOf(searchItem, StringComparison.OrdinalIgnoreCase) != -1)
              this.FoundItemList.Add(new FoundItem()
              {
                Keyword = keyword,
                Path = searchPath + " / " + searchProvider.SearchPath,
                SearchProviders = searchProviderPath
              });
          }
        }
      }
      if (searchProvider.SubSearchProviders == null)
        return;
      foreach (ISearchProvider subSearchProvider in (IEnumerable<ISearchProvider>) searchProvider.SubSearchProviders)
      {
        IList<ISearchProvider> searchProviderPath1 = (IList<ISearchProvider>) new List<ISearchProvider>((IEnumerable<ISearchProvider>) searchProviderPath);
        searchProviderPath1.Add(subSearchProvider);
        this.TraverseSearchProviders(subSearchProvider, searchItems, searchPath + " / " + searchProvider.SearchPath, searchProviderPath1);
      }
    }

    private void RadioButtonSearchAll_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButtonSearchAll.Checked)
        return;
      this.groupBoxSearchCriteria.Enabled = false;
    }

    private void RadioButtonCustomSearch_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButtonCustomSearch.Checked)
        return;
      this.groupBoxSearchCriteria.Enabled = true;
    }

    private void ButtonClear_Click(object sender, EventArgs e)
    {
      this.ClearSearch();
    }

    private void FindDialog_Load(object sender, EventArgs e)
    {
      if (!this.EnableClearResults)
        return;
      this.ClearSearch();
      this.EnableClearResults = false;
    }

    private void ClearSearch()
    {
      this.listViewFindResults.Items.Clear();
      this.FoundItemList.Clear();
      this.labelFindStatus.Text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Status: ", new object[0]);
    }

    private void FindDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
      }
      else
      {
        if (e.KeyCode != Keys.Return)
          return;
        this.BeginSearch();
      }
    }

    private void ListViewFindResults_ColumnClick(object sender, ColumnClickEventArgs e)
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

    private void CopyTextToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.listViewFindResults.ContainsFocus)
        return;
      FindDialog.CopyListViewItemsToClipboard(this.listViewFindResults);
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
      Clipboard.SetText(stringBuilder.ToString(), TextDataFormat.Text);
    }

    private void ListViewFindResults_ItemActivate(object sender, EventArgs e)
    {
      if (this.listViewFindResults.SelectedItems.Count != 1)
        return;
      this.SelectedFoundItem = (FoundItem) this.listViewFindResults.SelectedItems[0].Tag;
      this.Hide();
    }
  }
}
