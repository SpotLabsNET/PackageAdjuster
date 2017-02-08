// Decompiled with JetBrains decompiler
// Type: PackageInspector.CloseConfirm
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PackageInspector
{
  public class CloseConfirm : Form
  {
    private IContainer components;
    private Label labelConfirm;
    private Label labelCleanup2;
    private CheckBox checkBoxCleanupScratch;
    private Button buttonNo;
    private Button buttonYes;

    public bool EnableCleanup { get; set; }

    public CloseConfirm()
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CloseConfirm));
      this.labelConfirm = new Label();
      this.labelCleanup2 = new Label();
      this.checkBoxCleanupScratch = new CheckBox();
      this.buttonNo = new Button();
      this.buttonYes = new Button();
      this.SuspendLayout();
      this.labelConfirm.AutoSize = true;
      this.labelConfirm.Location = new Point(12, 24);
      this.labelConfirm.Name = "labelConfirm";
      this.labelConfirm.Size = new Size(158, 13);
      this.labelConfirm.TabIndex = 0;
      this.labelConfirm.Text = "Are you sure you want to close?";
      this.labelCleanup2.AutoSize = true;
      this.labelCleanup2.Location = new Point(31, 66);
      this.labelCleanup2.Name = "labelCleanup2";
      this.labelCleanup2.Size = new Size((int) sbyte.MaxValue, 13);
      this.labelCleanup2.TabIndex = 5;
      this.labelCleanup2.Text = "the inspected CBS packages.";
      this.checkBoxCleanupScratch.AutoSize = true;
      this.checkBoxCleanupScratch.Checked = true;
      this.checkBoxCleanupScratch.CheckState = CheckState.Checked;
      this.checkBoxCleanupScratch.Location = new Point(14, 46);
      this.checkBoxCleanupScratch.Name = "checkBoxCleanupScratch";
      this.checkBoxCleanupScratch.Size = new Size(283, 17);
      this.checkBoxCleanupScratch.TabIndex = 6;
      this.checkBoxCleanupScratch.Text = "Automatically clean up the scratch directory containing";
      this.checkBoxCleanupScratch.UseVisualStyleBackColor = true;
      this.checkBoxCleanupScratch.CheckedChanged += new EventHandler(this.checkBoxCleanupScratch_CheckedChanged);
      this.buttonNo.DialogResult = DialogResult.No;
      this.buttonNo.Location = new Point(218, 96);
      this.buttonNo.Name = "buttonNo";
      this.buttonNo.Size = new Size(75, 23);
      this.buttonNo.TabIndex = 3;
      this.buttonNo.Text = "No";
      this.buttonNo.UseVisualStyleBackColor = true;
      this.buttonYes.DialogResult = DialogResult.Yes;
      this.buttonYes.Location = new Point(137, 96);
      this.buttonYes.Name = "buttonYes";
      this.buttonYes.Size = new Size(75, 23);
      this.buttonYes.TabIndex = 2;
      this.buttonYes.Text = "Yes";
      this.buttonYes.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(309, 135);
      this.Controls.Add((Control) this.checkBoxCleanupScratch);
      this.Controls.Add((Control) this.labelCleanup2);
      this.Controls.Add((Control) this.buttonNo);
      this.Controls.Add((Control) this.buttonYes);
      this.Controls.Add((Control) this.labelConfirm);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "CloseConfirm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Close CBS Package Inspector?";
      this.Load += new EventHandler(this.CloseConfirm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void CloseConfirm_Load(object sender, EventArgs e)
    {
      this.checkBoxCleanupScratch.Enabled = this.EnableCleanup;
      this.labelCleanup2.Enabled = this.EnableCleanup;
    }

    private void checkBoxCleanupScratch_CheckedChanged(object sender, EventArgs e)
    {
      this.EnableCleanup = this.checkBoxCleanupScratch.Enabled && this.checkBoxCleanupScratch.Checked;
    }
  }
}
