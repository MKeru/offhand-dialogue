using System.Windows.Controls;

public class AdLibField
{
    public Label label;
    public TextBox textBox;
    public AdLibField(string labelText)
    {
        label = new Label();
        label.Content = labelText;
        textBox = new TextBox();
    }
}