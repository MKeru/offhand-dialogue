using System.Windows.Controls;

public class AddFields
{
    public static void AddFieldsToPanel(StackPanel panel, AdLib[] adLibs)
    {
        // special character arrays to be removed from ad libs for label usage
        char[] startChars = new char[] { '[', '*' };
        char[] endChars = new char[] { ']', '*' };

        AdLibField entry;
        for (int i = 0; i < adLibs.Length; i++)
        {
            entry = new AdLibField(adLibs[i].adLib.TrimStart(startChars).TrimEnd(endChars));
            panel.Children.Add(entry.label);
            panel.Children.Add(entry.textBox);
        }
    }
}
