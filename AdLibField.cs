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

        // set language for textbox to english
        textBox.Language = System.Windows.Markup.XmlLanguage.GetLanguage("en-US");
        // enable spell checking
        textBox.SpellCheck.IsEnabled = true;
        // set textbox width to 200
        textBox.Width = 200;
        // align textbox to left
        textBox.HorizontalAlignment = HorizontalAlignment.Left;

        // allow textblock to word wrap
        textBlock.TextWrapping = TextWrapping.Wrap;
        // padding
        textBlock.Padding = new Thickness(5, 4, 0, 1);
        textBlock.FontFamily = new System.Windows.Media.FontFamily("Times New Roman");
        textBlock.FontSize = 16;
    }
}