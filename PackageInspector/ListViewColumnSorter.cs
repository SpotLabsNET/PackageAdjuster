// Decompiled with JetBrains decompiler
// Type: PackageInspector.ListViewColumnSorter
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;

namespace PackageInspector
{
  public class ListViewColumnSorter : IComparer
  {
    private CaseInsensitiveComparer ObjectComparer;

    public int SortColumn { get; set; }

    public SortOrder Order { get; set; }

    public ListViewColumnSorter()
    {
      this.SortColumn = 0;
      this.Order = SortOrder.None;
      this.ObjectComparer = new CaseInsensitiveComparer(CultureInfo.InvariantCulture);
    }

    public int Compare(object x, object y)
    {
      if (x == null)
        throw new ArgumentNullException("x");
      if (y == null)
        throw new ArgumentNullException("y");
      ListViewItem listViewItem1 = x as ListViewItem;
      if (listViewItem1 == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} is not of type ListViewItem", new object[1]
        {
          (object) x.ToString()
        }));
      ListViewItem listViewItem2 = y as ListViewItem;
      if (listViewItem2 == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} is not of type ListViewItem", new object[1]
        {
          (object) y.ToString()
        }));
      int num = this.ObjectComparer.Compare((object) listViewItem1.SubItems[this.SortColumn].Text, (object) listViewItem2.SubItems[this.SortColumn].Text);
      if (this.Order == SortOrder.Ascending)
        return num;
      if (this.Order == SortOrder.Descending)
        return -num;
      return 0;
    }

    public static void Sort(object sender, int columnIndex)
    {
      if (sender == null)
        return;
      ListView listView = sender as ListView;
      if (listView == null || listView.ListViewItemSorter == null)
        return;
      ListViewColumnSorter listViewItemSorter = listView.ListViewItemSorter as ListViewColumnSorter;
      if (listViewItemSorter == null || columnIndex == 0)
        return;
      if (columnIndex == listViewItemSorter.SortColumn)
      {
        if (listViewItemSorter.Order == SortOrder.Ascending)
          listViewItemSorter.Order = SortOrder.Descending;
        else if (listViewItemSorter.Order == SortOrder.Descending)
          listViewItemSorter.Order = SortOrder.Ascending;
      }
      else
      {
        listViewItemSorter.SortColumn = columnIndex;
        listViewItemSorter.Order = SortOrder.Ascending;
      }
      listView.Sort();
    }
  }
}
