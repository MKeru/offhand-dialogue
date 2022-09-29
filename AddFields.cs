﻿using System;
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
            if (entry.label.Content.ToString() == "")
            {
                // throw exception
                throw new Exception("Error in AddFieldsToPanel: Label content is empty.");
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            entry.label.Content = char.ToUpper(entry.label.Content.ToString()[0]) + entry.label.Content.ToString().Substring(1);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            panel.Children.Add(entry.label);
            panel.Children.Add(entry.textBox);
        }
    }
}
