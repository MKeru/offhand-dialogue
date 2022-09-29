using System;
using System.Windows;
using System.Windows.Controls;

public class AddFields
{
    public static void AddFieldsToPanel(StackPanel panel, AdLib[] adLibs)
    {
        // special character arrays to be removed from ad libs for label usage
        char[] startChars = new char[] { '[', '*' };
        char[] endChars = new char[] { ']', '*' };

        AdLibField? entry;
        for (int i = 0; i < adLibs.Length; i++)
        {
            entry = new AdLibField(adLibs[i].adLib.TrimStart(startChars).TrimEnd(endChars));

            // check if label content is empty
            if (entry.textBlock.Text == "")
            {
                // throw exception
                throw new Exception("Error in AddFieldsToPanel: Text block content is empty.");
            }

            // make first character of entry textBlock uppercase
            entry.textBlock.Text = entry.textBlock.Text.Substring(0, 1).ToUpper() + entry.textBlock.Text.Substring(1);

            panel.Children.Add(entry.textBlock);
            panel.Children.Add(entry.textBox);
        }
    }
}
