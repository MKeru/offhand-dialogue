using System.Text;
namespace offhand_dialogue;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;


public partial class Form1 : Form
{
    public Button button1, button2;
    public string? rawStory, title, category;
    public Panel panel = new Panel();
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

        // button2
        button2 = new Button();
        button2.Location = new Point(20, 80);
        button2.Text = "Submit Ad\nLibs";
        // set button width to same as button1
        button2.Width = button1.Width;
        // set button height to fit text plus 10 pixels on each side
        button2.Height = TextRenderer.MeasureText(button2.Text, button2.Font).Height + 20;
        // make text fit button size without changin button size
        button2.Click += new EventHandler(this.button2_Click);

        InitializeComponent();
    }

    private void button1_Click(object? sender, EventArgs e)
    {
        // reset panel
        panel.Controls.Clear();
        panel.Dispose();

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
                    category = reader.ReadLine();
                    if (category == null || category.Length >= 50)
                    {
                        MessageBox.Show("There must be a category on the first line in the file. Please submit a valid file.");
                        // throw exception
                        throw new Exception("There must be a category on the first line in the file. Please submit a valid file.");
                    }

                    // assign second line of text from file to variable
                    title = reader.ReadLine();
                    if (title == null || title.Length >= 50)
                    {
                        MessageBox.Show("There must be a title on the second line in the file. Please submit a valid file.");
                        // throw exception
                        throw new Exception("There must be a title on the second line in the file. Please submit a valid file.");
                    }

                    // read rest of file to string
                    rawStory = reader.ReadToEnd();

                    // check if rawStory is null
                    if (rawStory == null)
                    {
                        MessageBox.Show("There must be a story in the file. Please submit a valid file.");
                        // throw exception
                        throw new Exception("There must be a story in the file. Please submit a valid file.");
                    }

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
                    string prettyLabel;
                    for (int i = 0; i < adLibs.Length; i++)
                    {
                        prettyLabel = adLibs[i];
                        // capitalize first letter and add colon to end
                        adLibs[i] = prettyLabel.Substring(0, 1).ToUpper() + prettyLabel.Substring(1) + ": ";
                    }

                    // create panel with text boxes
                    panel = new TableLayoutPanel();

                    var adLibFieldEntry = new adLibField(adLibs[0]);
                    panel.Controls.Add(adLibFieldEntry);

                    for (int i = 1; i < adLibs.Length; i++)
                    {
                        adLibFieldEntry = new adLibField(adLibs[i]);
                        panel.Controls.Add(adLibFieldEntry);
                    }

                    // set panel size to fit all text boxes
                    panel.AutoSize = true;
                    // allow form to scroll
                    this.AutoScroll = true;
                    // set panel location to right of button1
                    panel.Location = new Point(button1.Location.X + button1.Width + 20, button1.Location.Y);
                    // add visual boundary to panel
                    panel.BorderStyle = BorderStyle.FixedSingle;

                    this.Controls.Add(panel);

                    // if all was successful, add button2 to form
                    this.Controls.Add(button2);
                }
            }
            catch (Exception)
            {
            }
        }
    }

    // button2 click handler
    private void button2_Click(object? sender, EventArgs e)
    {
        // get first text box input from panel
        var adLibFieldEntry = (adLibField)panel.Controls[0];

        // create stringbuilder
        StringBuilder sb = new StringBuilder();
        sb.Append(category);
        sb.Append("\r\n");
        sb.Append(title);
        sb.Append("\r\n");

        // regex
        Regex regex = new Regex("\\[.+?\\]");

        try
        {
            // throw exception if any text box in the panel is empty
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                adLibFieldEntry = (adLibField)panel.Controls[i];
                if (adLibFieldEntry.textBox.Text == "")
                {
                    // messagebox with error
                    MessageBox.Show("Please fill out all fields.");
                    throw new Exception("Please fill out all ad libs!");
                }
            }

            // check if rawStory is null
            if (rawStory == null)
            {
                MessageBox.Show("There must be a story in the file. Please submit a valid file.");
                // throw exception
                throw new Exception("There must be a story in the file. Please submit a valid file.");
            }

            string finalStory = rawStory;

            // remove all newlines from finalStory
            finalStory = finalStory.Replace("\r\n", " ");

            // replace regex matches in rawStory with text box inputs from panel in order
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                // get text box input from panel
                adLibFieldEntry = (adLibField)panel.Controls[i];
                // replace regex match with text box input
                finalStory = regex.Replace(finalStory, adLibFieldEntry.textBox.Text, 1);
            }

            // delete double spaces from finalStory
            finalStory = finalStory.Replace("  ", " ");

            // add rawStory to stringbuilder
            sb.Append(finalStory);

            // fit finalStory to message box
            MessageBox.Show(sb.ToString(), "Your Story", MessageBoxButtons.OK);

            // clear fields in panel
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                adLibFieldEntry = (adLibField)panel.Controls[i];
                adLibFieldEntry.textBox.Text = "";
            }

            
        }
        catch (Exception)
        {
        }
    }
}

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
        Controls.Add(textBox);
    }
}
