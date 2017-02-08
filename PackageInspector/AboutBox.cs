// Decompiled with JetBrains decompiler
// Type: PackageInspector.AboutBox
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PackageInspector
{
  internal class AboutBox : Form
  {
    private IContainer components;
    private Button okButton;
    private Label labelProductName;
    private Label labelVersion;
    private Label labelCopyright;
    private Label labelCompanyName;
    private RichTextBox richTextBoxDescription;
    private PictureBox logoPictureBox;

    public static string AssemblyTitle
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
        if (customAttributes.Length > 0)
        {
          AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute) customAttributes[0];
          if (!string.IsNullOrEmpty(assemblyTitleAttribute.Title))
            return assemblyTitleAttribute.Title;
        }
        return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
      }
    }

    public static string AssemblyVersion
    {
      get
      {
        return Assembly.GetExecutingAssembly().GetName().Version.ToString();
      }
    }

    public static string AssemblyDescription
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
        if (customAttributes.Length == 0)
          return "";
        return ((AssemblyDescriptionAttribute) customAttributes[0]).Description;
      }
    }

    public static string AssemblyProduct
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyProductAttribute), false);
        if (customAttributes.Length == 0)
          return "";
        return ((AssemblyProductAttribute) customAttributes[0]).Product;
      }
    }

    public static string AssemblyCopyright
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
        if (customAttributes.Length == 0)
          return "";
        return ((AssemblyCopyrightAttribute) customAttributes[0]).Copyright;
      }
    }

    public static string AssemblyCompany
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
        if (customAttributes.Length == 0)
          return "";
        return ((AssemblyCompanyAttribute) customAttributes[0]).Company;
      }
    }

    public AboutBox()
    {
      this.InitializeComponent();
      this.Text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "About {0}", new object[1]
      {
        (object) AboutBox.AssemblyTitle
      });
      this.labelProductName.Text = AboutBox.AssemblyProduct;
      this.labelVersion.Text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Version {0}", new object[1]
      {
        (object) AboutBox.AssemblyVersion
      });
      this.labelCopyright.Text = AboutBox.AssemblyCopyright;
      this.labelCompanyName.Text = AboutBox.AssemblyCompany;
      this.richTextBoxDescription.Text = AboutBox.AssemblyDescription;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AboutBox));
      this.okButton = new Button();
      this.labelProductName = new Label();
      this.labelVersion = new Label();
      this.labelCopyright = new Label();
      this.labelCompanyName = new Label();
      this.richTextBoxDescription = new RichTextBox();
      this.logoPictureBox = new PictureBox();
      ((ISupportInitialize) this.logoPictureBox).BeginInit();
      this.SuspendLayout();
      this.okButton.DialogResult = DialogResult.Cancel;
      this.okButton.Location = new Point(246, 226);
      this.okButton.Name = "okButton";
      this.okButton.Size = new Size(75, 23);
      this.okButton.TabIndex = 24;
      this.okButton.Text = "&OK";
      this.labelProductName.AutoSize = true;
      this.labelProductName.Location = new Point(119, 13);
      this.labelProductName.Name = "labelProductName";
      this.labelProductName.Size = new Size(75, 13);
      this.labelProductName.TabIndex = 25;
      this.labelProductName.Text = "Product Name";
      this.labelVersion.AutoSize = true;
      this.labelVersion.Location = new Point(119, 39);
      this.labelVersion.Name = "labelVersion";
      this.labelVersion.Size = new Size(42, 13);
      this.labelVersion.TabIndex = 26;
      this.labelVersion.Text = "Version";
      this.labelCopyright.AutoSize = true;
      this.labelCopyright.Location = new Point(119, 65);
      this.labelCopyright.Name = "labelCopyright";
      this.labelCopyright.Size = new Size(51, 13);
      this.labelCopyright.TabIndex = 27;
      this.labelCopyright.Text = "Copyright";
      this.labelCompanyName.AutoSize = true;
      this.labelCompanyName.Location = new Point(119, 93);
      this.labelCompanyName.Name = "labelCompanyName";
      this.labelCompanyName.Size = new Size(51, 13);
      this.labelCompanyName.TabIndex = 28;
      this.labelCompanyName.Text = "Company";
      this.richTextBoxDescription.Location = new Point(13, 124);
      this.richTextBoxDescription.Name = "richTextBoxDescription";
      this.richTextBoxDescription.ReadOnly = true;
      this.richTextBoxDescription.Size = new Size(308, 96);
      this.richTextBoxDescription.TabIndex = 29;
      this.richTextBoxDescription.Text = "";
      this.logoPictureBox.Image = (Image) componentResourceManager.GetObject("logoPictureBox.Image");
      this.logoPictureBox.Location = new Point(13, 13);
      this.logoPictureBox.Name = "logoPictureBox";
      this.logoPictureBox.Size = new Size(96, 96);
      this.logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
      this.logoPictureBox.TabIndex = 30;
      this.logoPictureBox.TabStop = false;
      this.AcceptButton = (IButtonControl) this.okButton;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(335, 262);
      this.Controls.Add((Control) this.logoPictureBox);
      this.Controls.Add((Control) this.richTextBoxDescription);
      this.Controls.Add((Control) this.labelCompanyName);
      this.Controls.Add((Control) this.labelCopyright);
      this.Controls.Add((Control) this.labelVersion);
      this.Controls.Add((Control) this.labelProductName);
      this.Controls.Add((Control) this.okButton);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutBox";
      this.Padding = new Padding(9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "About CBS Package Inspector";
      this.KeyDown += new KeyEventHandler(this.AboutBox_KeyDown);
      ((ISupportInitialize) this.logoPictureBox).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void AboutBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
  }
}
