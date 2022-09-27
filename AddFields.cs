using System.Text;
namespace offhand_dialogue;
using System.Linq;
public class AddFields
{
    public static Panel CreateFieldsPanel(AdLib[] adLibsArray)
    {
        // create panel
        Panel panel = new TableLayoutPanel();

        // special character arrays to be removed from ad libs for label usage
        char[] startChars = new char[] { '[', '*' };
        char[] endChars = new char[] { ']', '*' };

        adLibField entry;
        for (int i = 0; i < adLibsArray.Count(); i++)
        {
            // trim special characters from ad lib in adLibsArray
            adLibsArray[i].adLib = adLibsArray[i].adLib.TrimStart(startChars).TrimEnd(endChars);

            // capitalize first letter of key at element i in adLibsArray
            adLibsArray[i].adLib = char.ToUpper(adLibsArray[i].adLib[0]) + adLibsArray[i].adLib.Substring(1);

            entry = new adLibField(adLibsArray[i].adLib);

            panel.Controls.Add(entry);
        }

        return panel;
    }
}