namespace offhand_dialogue;
using System.Drawing.Drawing2D;
partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(940, 540);

        // make gradient background example
        /*
        LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), 90F, false);
        ColorBlend cb = new ColorBlend();
        cb.Positions = new[] { 0, 0.3f, 1 };
        cb.Colors = new[] { Color.FromArgb(19, 10, 30), Color.Black, Color.Black };
        brush.InterpolationColors = cb;
        this.Paint += (s, e) => e.Graphics.FillRectangle(brush, this.ClientRectangle);
        */

        this.Text = "Monkey Business";
    }

    #endregion
}
