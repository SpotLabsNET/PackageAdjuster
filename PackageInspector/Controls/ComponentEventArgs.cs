// Decompiled with JetBrains decompiler
// Type: PackageInspector.Controls.ComponentEventArgs
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;

namespace PackageInspector.Controls
{
  public class ComponentEventArgs : EventArgs
  {
    public WindowsComponent Component { get; set; }

    public ComponentEventArgs(WindowsComponent component)
    {
      this.Component = component;
    }
  }
}
