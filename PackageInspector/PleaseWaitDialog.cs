// Decompiled with JetBrains decompiler
// Type: PackageInspector.PleaseWaitDialog
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PackageInspector
{
  public class PleaseWaitDialog : Form
  {
    private IContainer components;
    private Panel panel1;
    private Label label1;
    private ProgressBar progressBar1;

    public PleaseWaitDialog()
    {
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.label1 = new Label();
      this.progressBar1 = new ProgressBar();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.progressBar1);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(284, 262);
      this.panel1.TabIndex = 0;
      this.panel1.UseWaitCursor = true;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(35, 111);
      this.label1.Name = "label1";
      this.label1.Size = new Size(206, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Analyzing. Please Wait...";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.label1.UseWaitCursor = true;
      this.progressBar1.Location = new Point(39, 134);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(202, 18);
      this.progressBar1.Style = ProgressBarStyle.Marquee;
      this.progressBar1.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(284, 262);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PleaseWaitDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Please Wait...";
      this.TransparencyKey = SystemColors.Control;
      this.UseWaitCursor = true;
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
