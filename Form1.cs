using System.Text;
namespace offhand_dialogue;
using System.Text.RegularExpressions;


public partial class Form1 : Form
{
    public Button button1, button2;

    // collection of ad libs with specifications (italics, capitalization, etc.)

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
        button1.Click += new EventHandler(this.button1_Click);
        this.Controls.Add(button1);

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

    AdLib[]? adLibsArray;
    public Story? story;
    public Panel? panel;
    private void button1_Click(object? sender, EventArgs e)
    {
        // Story object which opens a file dialog and reads the file
        story = new Story();
        // if the story was unable to be read, do not continue
        if (!story.IsSuccessful())
        {
            return;
        }

        adLibsArray = story.GetAdlibsArray();

        // delete panel if it exists
        if (panel != null)
        {
            this.Controls.Remove(panel);
        }

        panel = AddFields.CreateFieldsPanel(adLibsArray);

        // set panel max size to 180 by computer screen height
        panel.MaximumSize = new Size(180, Screen.PrimaryScreen.Bounds.Height - 100);

        // set panel location to right of button1
        panel.Location = new Point(button1.Location.X + button1.Width + 20, button1.Location.Y);

        // set panel height and width to fit all controls
        panel.Height = panel.PreferredSize.Height;
        panel.Width = panel.PreferredSize.Width + SystemInformation.VerticalScrollBarWidth;

        if (panel.Controls.Cast<Control>().Max(c => c.Width) + 10 < panel.Width)
        {
            panel.HorizontalScroll.Enabled = false;
            panel.HorizontalScroll.Maximum = 0;
        }

        // FIXME: if vertical scroll isn't really needed, remove it
        /*
        if (panel.Controls.Cast<Control>().Max(c => c.Height) + 10 < panel.Height)
        {
            panel.VerticalScroll.Enabled = false;
            panel.VerticalScroll.Maximum = 0;
        }
        */

        // autoscroll panel
        panel.AutoScroll = true;

        // add visual boundary to panel
        panel.BorderStyle = BorderStyle.FixedSingle;

        // add panel to form
        this.Controls.Add(panel);

        // if all was successful, add button2 to form
        this.Controls.Add(button2);
    }

    // button2 click handler
    public RichTextBox finalStoryTextBox = new RichTextBox();
    string? finalStory;
    private void button2_Click(object? sender, EventArgs e)
    {
        // check if story is null
        if (story == null || panel == null)
        {
            return;
        }
        finalStory = story.GetFinalStory(panel);

        try
        {
            // create text box to right of panel with final story
            finalStoryTextBox.WordWrap = true;
            finalStoryTextBox.Multiline = true;
            finalStoryTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;

            // set text box location to right of panel
            finalStoryTextBox.Location = new Point(panel.Location.X + panel.Width + 20, panel.Location.Y);

            finalStoryTextBox.Text = finalStory;

            // style textbox
            finalStoryTextBox.Font = new Font("Times New Roman", 50);

            /*
            // create matchcollection
            MatchCollection matches = Regex.Matches(finalStoryTextBox.Text, @"\*.*?\*");
            foreach (Match match in matches)
            {
                finalStoryTextBox.SelectionStart = match.Index - 1;
                finalStoryTextBox.SelectionLength = match.Length - 1;
                finalStoryTextBox.SelectionFont = new Font(finalStoryTextBox.Font, FontStyle.Italic);
            }
            */

            finalStoryTextBox.Select(0, story.GetCategory().Length);
            // bold first line, change font size to 20, and align to left
            finalStoryTextBox.SelectionFont = new Font("Times New Roman", 20, FontStyle.Bold);
            finalStoryTextBox.SelectionAlignment = HorizontalAlignment.Left;

            // bold second
            finalStoryTextBox.Select(story.GetCategory().Length + 1, story.GetTitle().Length);
            finalStoryTextBox.SelectionFont = new Font("Times New Roman", 60, FontStyle.Bold);

            // select all text
            finalStoryTextBox.SelectAll();
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
        // check if story is null
        if (story != null)
        {
            saveFileDialog1.FileName = story.GetTitle() + ".txt";
        }
        else 
        {
            MessageBox.Show("Story is null.");
            return;
        }
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
                writer.Write(finalStory);
            }
        }

        // close save file dialog
        saveFileDialog1.Dispose();
    }
}