using System.Windows;
using System.Windows.Controls;

public class AdLibField
{
    public TextBlock textBlock;
    public TextBox textBox;
    public AdLibField(string labelText)
    {
        textBlock = new TextBlock();
        textBlock.Text = labelText;
        textBox = new TextBox();

        // set textbox width to 200
        textBox.Width = 200;
        // align textbox to left
        textBox.HorizontalAlignment = HorizontalAlignment.Left;

        // allow textblock to word wrap
        textBlock.TextWrapping = TextWrapping.Wrap;
    }
}