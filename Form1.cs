using System.Text;
namespace offhand_dialogue;
using System.Text.RegularExpressions;


public partial class Form1 : Form
{
    public Button button1, button2;
    public string? rawStory, title, category;
    public Panel panel = new Panel();
    // stringbuilder for the final story
    public StringBuilder sb_final = new StringBuilder();
    public Form1()
    {
        this.AutoSize = false;

        // allow scrolling
        this.AutoScroll = true;

        button1 = new Button();
        button1.Text = "Select New\nText File";
        // set button width to fit text plus 10 pixels on each side
        button1.Width = TextRenderer.MeasureText(button1.Text, button1.Font).Width + 20;
        // set button height to fit text plus 10 pixels on each side
        button1.Height = TextRenderer.MeasureText(button1.Text, button1.Font).Height + 20;
        button1.Location = new Point(20, 20);

        this.Controls.Add(button1);
        button1.Click += new EventHandler(this.button1_Click);

        // button2
        button2 = new Button();
        button2.Text = "Submit Ad\nLibs";
        // set button width to same as button1
        button2.Width = button1.Width;
        // set button height to fit text plus 10 pixels on each side
        button2.Height = TextRenderer.MeasureText(button2.Text, button2.Font).Height + 20;
        button2.Location = new Point(20, 20 + button1.Height + 20);
        button2.Click += new EventHandler(this.button2_Click);

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

                    // from here on the file is valid
                    // reset panel
                    panel.Controls.Clear();
                    panel.Dispose();

                    // makes ad lib labels capitalized
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

                    // set panel max size to 180 by computer screen height
                    panel.MaximumSize = new Size(180, Screen.PrimaryScreen.Bounds.Height - 100);

                    // set panel location to right of button1
                    panel.Location = new Point(button1.Location.X + button1.Width + 20, button1.Location.Y);

                    // set panel height to fit all controls
                    panel.Height = panel.PreferredSize.Height;

                    panel.Width = panel.PreferredSize.Width + SystemInformation.VerticalScrollBarWidth;

                    if (panel.Controls.Cast<Control>().Max(c => c.Width) + 10 < panel.Width)
                    {
                        panel.HorizontalScroll.Maximum = 0;
                    }

                    // panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
                    panel.AutoScroll = true;

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
    public RichTextBox finalStoryTextBox = new RichTextBox();
    private void button2_Click(object? sender, EventArgs e)
    {
        // get first text box input from panel
        var adLibFieldEntry = (adLibField)panel.Controls[0];

        // clear stringbuilder
        sb_final.Clear();

        sb_final.Append(category);
        sb_final.Append("\r\n");
        sb_final.Append(title);
        sb_final.Append("\r\n");

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
            sb_final.Append(finalStory);

            // create text box to right of panel with final story
            finalStoryTextBox.WordWrap = true;
            finalStoryTextBox.Multiline = true;
            finalStoryTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;

            // set text box location to right of panel
            finalStoryTextBox.Location = new Point(panel.Location.X + panel.Width + 20, panel.Location.Y);

            finalStoryTextBox.Text = sb_final.ToString();

            // style textbox
            finalStoryTextBox.Font = new Font("Arial", 50);

            // bold first two lines of text box
            if (category == null || title == null)
            {
                MessageBox.Show("There must be a category and title on the first two lines of the file. Please submit a valid file.");
                // throw exception
                throw new Exception("There must be a category and title on the first two lines of the file. Please submit a valid file.");
            }
            
            finalStoryTextBox.Select(0, category.Length);
            finalStoryTextBox.SelectionFont = new Font(finalStoryTextBox.Font, FontStyle.Bold);
            finalStoryTextBox.Select(category.Length + 1, title.Length);
            finalStoryTextBox.SelectionFont = new Font(finalStoryTextBox.Font, FontStyle.Bold);
            
            // center all text in text box
            finalStoryTextBox.SelectionAlignment = HorizontalAlignment.Center;

            // set text box max size to 180 by computer screen height
            finalStoryTextBox.MaximumSize = new Size(0, Screen.PrimaryScreen.Bounds.Height - 100);

            // set text box width to half of screen
            finalStoryTextBox.Width = Screen.PrimaryScreen.Bounds.Width - finalStoryTextBox.Location.X - 20;
            SizeF size = finalStoryTextBox.CreateGraphics().MeasureString(finalStoryTextBox.Text, finalStoryTextBox.Font, finalStoryTextBox.Width, new StringFormat(0));
            finalStoryTextBox.Height = (int)size.Height;

            // textbox is readonly and unselectable
            finalStoryTextBox.ReadOnly = true;
            finalStoryTextBox.TabStop = false;
            finalStoryTextBox.BorderStyle = BorderStyle.FixedSingle;

            // textbox background color to same as form
            finalStoryTextBox.BackColor = this.BackColor;

            // add save button to right of text box
            Button button3 = new Button();
            button3.Text = "Save";
            // set button location under button2
            button3.Location = new Point(20, button2.Location.Y + button2.Height + 20);
            // set button width to same as button2
            button3.Width = button2.Width;
            // set button height to same as button2
            button3.Height = button2.Height;
            button3.Click += new EventHandler(button3_Click);

            // add text box and save button to form
            this.Controls.Add(finalStoryTextBox);
            this.Controls.Add(button3);
        }
        catch (Exception)
        {
        }
    }

    // button3 click handler to save finalStory to text file
    private void button3_Click(object? sender, EventArgs e)
    {
        // create save file dialog
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        // set default file name
        saveFileDialog1.FileName = title + ".txt";
        // set default file extension
        saveFileDialog1.DefaultExt = "txt";
        // set default file type
        saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        // let user choose save location
        saveFileDialog1.OverwritePrompt = true;

        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            // create file
            using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
            {
                // write finalStory to file
                writer.Write(sb_final.ToString());
            }
        }

        // close save file dialog
        saveFileDialog1.Dispose();
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
        // set text box width to 100px
        textBox.Width = 140;
        Controls.Add(textBox);
    }
}
