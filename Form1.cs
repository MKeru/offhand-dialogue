using System.Text;
namespace offhand_dialogue;
using System.Text.RegularExpressions;
using Markdig;
using HtmlAgilityPack;


public partial class Form1 : Form
{
    public Button button1, button2, button3;

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
        // start button2 disabled and invisible
        button2.Enabled = false;
        button2.Visible = false;
        this.Controls.Add(button2);

        button3 = new Button();
        button3.Text = "Save";
        // set button location under button2
        button3.Location = new Point(20, button2.Location.Y + button2.Height + 20);
        // set button width to same as button2
        button3.Width = button2.Width;
        // set button height to same as button2
        button3.Height = button2.Height;
        button3.Click += new EventHandler(button3_Click);
        // start button3 disabled and invisible
        button3.Enabled = false;
        button3.Visible = false;
        this.Controls.Add(button3);

        InitializeComponent();
    }

    AdLib[]? adLibsArray;
    public Story? story;
    public Panel? panel;
    WebBrowser webBrowser;
    private void button1_Click(object? sender, EventArgs e)
    {
        // Story object which opens a file dialog and reads the file
        story = new Story();
        // if the story was unable to be read, do not continue
        if (!story.IsSuccessful())
        {
            return;
        }

        // delete web browser if it exists
        if (webBrowser != null)
        {
            this.Controls.Remove(webBrowser);
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

        // make button2 visible and enabled
        button2.Enabled = true;
        button2.Visible = true;
    }

    // button2 click handler
    string? finalStory, title, category;
    private void button2_Click(object? sender, EventArgs e)
    {
        // clear all fields
        finalStory = null;

        // check if story is null
        if (story == null || panel == null)
        {
            return;
        }

        // get title and category
        title = story.GetTitle();
        category = story.GetCategory();
        // finalStory contains only the story
        finalStory = story.GetFinalStory(panel);

        // start building html
        // FORMAT RULES:
        // all text is Times New Roman
        // Title is font 60
        // Category is font 20
        // Story is font 50
        // Title, line break, Category, line break, Story
        // keep all newlines in story
        StringBuilder html = new StringBuilder();
        html.Append("<html><head><style>");
        html.Append("body {font-family: \"Times New Roman\", Times, serif;}");
        html.Append("h1 {font-size: 20px;}");
        html.Append("h2 {font-size: 60px;}");
        html.Append("p {font-size: 50px;}");
        html.Append("</style></head><body>");
        html.Append("<h1>" + category + "</h1>");
        html.Append("<h2>" + title + "</h2>");

        // change newlines to line breaks
        finalStory = finalStory.Replace("\r\n", "<br>");
        // apply markdown to finalStory
        var finalStoryHtml = Markdig.Markdown.ToHtml(finalStory);

        html.Append("<p>" + finalStoryHtml.ToString() + "</p>");
        html.Append("</body></html>");

        // center html
        html.Insert(0, "<center>");
        html.Append("</center>");

        // convert StringBuilder to string
        string htmlString = html.ToString();

        // make web browser if it doesn't exist
        if (webBrowser == null)
        {
            webBrowser = new WebBrowser();
            webBrowser.Location = new Point(panel.Location.X + panel.Width + 20, panel.Location.Y);
            webBrowser.Width = this.Width - webBrowser.Location.X - 60;
            webBrowser.Height = this.Height - webBrowser.Location.Y - 60;
            webBrowser.DocumentText = htmlString;
            this.Controls.Add(webBrowser);
        }
        else
        {
            // set web browser document text to html
            webBrowser.DocumentText = htmlString;
        }

        // enable button3
        button3.Enabled = true;
        button3.Visible = true;
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