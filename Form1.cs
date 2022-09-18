using System.Text;
namespace angry_snowflakes;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

public partial class Form1 : Form
{
    public Button button1;
    public Form1()
    {
        button1 = new Button();
        button1.Size = new Size(100, 40);
        button1.Location = new Point(0, 0);
        button1.Text = "Select Text File";
        this.Controls.Add(button1);
        button1.Click += new EventHandler(this.button1_Click);

        this.AutoSize = true;
        InitializeComponent();
    }

    private void button1_Click(object? sender, EventArgs e)
    {
        // open file browser window
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text Files (*.txt)|*.txt";
        openFileDialog.RestoreDirectory = true;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string file = openFileDialog.FileName;
            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    // assign first line of text from file to variable
                    string? category = reader.ReadLine();
                    if (category == null || category.Length >= 50)
                    {
                        MessageBox.Show("There must be a category on the first line in the file. Please submit a valid file.");
                        // throw exception
                        throw new Exception("There must be a category on the first line in the file. Please submit a valid file.");
                    }

                    // assign second line of text from file to variable
                    string? title = reader.ReadLine();
                    if (title == null || title.Length >= 50)
                    {
                        MessageBox.Show("There must be a title on the second line in the file. Please submit a valid file.");
                        // throw exception
                        throw new Exception("There must be a title on the second line in the file. Please submit a valid file.");
                    }

                    // read rest of file to string
                    string? rawStory = reader.ReadToEnd();

                    // find all regex matches in text
                    Regex regex = new Regex("\\[.+?\\]");
                    MatchCollection matches = regex.Matches(rawStory);
                    
                    // create array of size matches
                    string[] adLibs = new string[matches.Count];
                    for (int i = 0; i < matches.Count; i++)
                    {
                        adLibs[i] = matches[i].Value.TrimStart('[').TrimEnd(']');
                    }

                    if (adLibs.Length == 0)
                    {
                        MessageBox.Show("There must be at least one ad lib in the file. Please submit a valid file.");
                        // throw exception
                        throw new Exception("There must be at least one ad lib in the file. Please submit a valid file.");
                    }

                    // message box listing wordTypes stack
                    /*
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < adLibs.Length; i++) 
                    {
                        sb.Append(adLibs[i]);
                        sb.Append("\r\n");
                    }
                    */

                    // MessageBox.Show(sb.ToString());

                    TextBox newText = new TextBox();
                    newText.BorderStyle = BorderStyle.FixedSingle;
                    newText.Size = new Size(100, 20);
                    var textBoxes = new Dictionary<string, TextBox>();

                    for (int i = 0; i < adLibs.Length; i++)
                    {
                        newText.Text = adLibs[i];
                        newText.Location = new Point(20, (i * 40) + 50);
                        textBoxes.Add(adLibs[i], newText);
                    }


                }
            }
            catch (Exception)
            {
            }
        }
    }
}
