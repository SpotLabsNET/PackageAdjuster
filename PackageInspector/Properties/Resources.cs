// Decompiled with JetBrains decompiler
// Type: PackageInspector.Properties.Resources
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PackageInspector.Properties
{
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
  [DebuggerNonUserCode]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) PackageInspector.Properties.Resources.resourceMan, (object) null))
          PackageInspector.Properties.Resources.resourceMan = new ResourceManager("PackageInspector.Properties.Resources", typeof (PackageInspector.Properties.Resources).Assembly);
        return PackageInspector.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return PackageInspector.Properties.Resources.resourceCulture;
      }
      set
      {
        PackageInspector.Properties.Resources.resourceCulture = value;
      }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal Resources()
    {
    }
  }
}
