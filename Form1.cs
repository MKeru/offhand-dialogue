using System.Text;
namespace angry_snowflakes;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

public partial class Form1 : Form
{
    public Button button1;
    public Form1()
    {
        this.AutoSize = false;

        button1 = new Button();
        // button1.Size = new Size(100, 40);
        button1.Location = new Point(20, 20);
        button1.Text = "Select New\nText File";
        // set button width to fit text plus 10 pixels on each side
        button1.Width = TextRenderer.MeasureText(button1.Text, button1.Font).Width + 20;
        // set button height to fit text plus 10 pixels on each side
        button1.Height = TextRenderer.MeasureText(button1.Text, button1.Font).Height + 20;

        this.Controls.Add(button1);
        button1.Click += new EventHandler(this.button1_Click);

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

                    StringBuilder sb = new StringBuilder();
                    string prettyLabel;
                    for (int i = 0; i < adLibs.Length; i++)
                    {
                        prettyLabel = adLibs[i];
                        // capitalize first letter and add colon to end
                        adLibs[i] = prettyLabel.Substring(0, 1).ToUpper() + prettyLabel.Substring(1) + ": ";
                    }

                    TextBox newText = new TextBox();
                    // create panel with text boxes
                    var panel = new TableLayoutPanel();

                    var adLibFieldEntry = new adLibField(adLibs[0], new TextBox());
                    panel.Controls.Add(adLibFieldEntry);

                    for (int i = 1; i < adLibs.Length; i++)
                    {
                        adLibFieldEntry = new adLibField(adLibs[i], new TextBox());
                        panel.Controls.Add(adLibFieldEntry);
                    }

                    // set panel size to fit all text boxes
                    panel.AutoSize = true;
                    // allow form to scroll
                    this.AutoScroll = true;
                    // set panel location to right of button1
                    panel.Location = new Point(button1.Location.X + button1.Width + 20, button1.Location.Y);
                    // add scroll bar to panel
                    // add visual boundary to panel
                    panel.BorderStyle = BorderStyle.FixedSingle;

                    this.Controls.Add(panel);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

class adLibField : TableLayoutPanel
    {
        public Label adLibLabel;
        public adLibField(string label, TextBox textBox)
            : base()
        {
            AutoSize = true;
            adLibLabel = new Label();
            adLibLabel.Text = label;
            adLibLabel.AutoSize = true;

            Controls.Add(adLibLabel);
            Controls.Add(textBox);
        }
    }
