class adLibField : TableLayoutPanel
{
    public Label adLibLabel;
    public TextBox textBox;
    public adLibField(string label)
        : base()
    {
        AutoSize = true;
        adLibLabel = new Label();
        adLibLabel.Text = label;
        adLibLabel.AutoSize = true;

        Controls.Add(adLibLabel);
        // create new text box
        textBox = new TextBox();
        // set text box width to 100px
        textBox.Width = 140;
        Controls.Add(textBox);
    }
}