// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.IItemViewer
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

namespace PackageInspector.Controls
{
  public interface IItemViewer
  {
    object Data { get; set; }

    void UpdateView();

    void ClearView();
  }
}
