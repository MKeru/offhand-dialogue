using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Markdig;
using Microsoft.Win32;

namespace offhand_dialogue_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Story? story;
        public AdLib[]? adLibs;
        public string? finalStory, title, category, htmlString, simpleFinalStory, rawStory;
        private AdLib[]? adLibTypes;

        // window

        public MainWindow()
        {
            
            InitializeComponent();

            // make webBrowser invisible
            webBrowser.Visibility = Visibility.Hidden;

            // set background gradient
            Rectangle rect = new Rectangle();
            LinearGradientBrush lgb = new LinearGradientBrush();
            // make rect fill the window
            rect.Width = this.Width;
            rect.Height = this.Height;
            // set gradient
            lgb.StartPoint = new Point(0, 0);
            lgb.EndPoint = new Point(0, 1);
            lgb.GradientStops.Add(new GradientStop(Colors.Purple, 0.0));
            lgb.GradientStops.Add(new GradientStop(Colors.Black, 0.2));
            lgb.GradientStops.Add(new GradientStop(Colors.Black, 0.8));
            lgb.GradientStops.Add(new GradientStop(Colors.Green, 1.0));

            rect.Fill = lgb;

            // add rect to window
            this.Background = lgb;
        }

        private void selectFileButton_Click(object sender, RoutedEventArgs e)
        {
            // open file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                try
                {
                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        // assign first line of text from file to variable
                        category = reader.ReadLine();
                        if (category == null || category.Length >= 50)
                        {
                            MessageBox.Show("There must be a category on the first line in the file. Please submit a valid file.");
                            // throw exception
                            throw new Exception("There must be a category on the first line in the file. Please submit a valid file.");
                        }
                        // delete leading or trailing whitespace
                        category = category.Trim();

                        // assign second line of text from file to variable
                        title = reader.ReadLine();
                        if (title == null || title.Length >= 50)
                        {
                            MessageBox.Show("There must be a title on the second line in the file. Please submit a valid file.");
                            // throw exception
                            throw new Exception("There must be a title on the second line in the file. Please submit a valid file.");
                        }
                        // delete leading or trailing whitespace
                        title = title.Trim();

                        // read rest of file to string
                        rawStory = reader.ReadToEnd();

                        // check if rawStory is null
                        if (reader.ReadToEnd() == null)
                        {
                            MessageBox.Show("There must be a story in the file. Please submit a valid file.");
                            // throw exception
                            throw new Exception("There must be a story in the file. Please submit a valid file.");
                        }

                        // regex for any ad lib in the story
                        Regex normRegex = new Regex("\\[.+?\\]");
                        MatchCollection matches = normRegex.Matches(rawStory);

                        adLibTypes = new AdLib[matches.Count];
                        for (int i = 0; i < matches.Count; i++)
                        {
                            // add ad lib to array
                            adLibTypes[i] = new AdLib(matches[i].Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            else
            {
                return;
            }

            // create new story object
            story = new Story(category, title, rawStory, adLibTypes);

            // get ad libs from story
            adLibs = story.GetAdlibsArray();

            // clear inputPanel
            inputPanel.Children.Clear();
            // add ad libs to inputPanel
            AddFields.AddFieldsToPanel(inputPanel, adLibs);

            // enable submit button
            submitInputButton.IsEnabled = true;
        }

        private void submitInputButton_Click(object sender, RoutedEventArgs e)
        {
            if (story == null)
            {
                MessageBox.Show("Please select a file first.");
                return;
            }
            
            // get title and category from story
            title = story.GetTitle();
            category = story.GetCategory();

            // get finalStory
            finalStory = story.GetFinalStory(inputPanel);

            if (finalStory == "")
            {
                return;
            }

            // FOR SAVING TO FILE
            // get simpleFinalStory
            simpleFinalStory = finalStory;
            // strip html tags from simpleFinalStory
            simpleFinalStory = Regex.Replace(simpleFinalStory, "<.*?>", String.Empty);
            // strip markdown from simpleFinalStory
            simpleFinalStory = Markdig.Markdown.ToPlainText(simpleFinalStory);

            // not final solution
            StringBuilder html = new StringBuilder();
            html.Append("<html><head><style>");
            html.Append("body {font-family: \"Times New Roman\", Times, serif;}");
            html.Append("h1 {font-size: 20px;}");
            html.Append("h2 {font-size: 60px;}");
            html.Append("p {font-size: 35px;}");
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
            htmlString = html.ToString();

            // set htmlString background to black
            htmlString = htmlString.Replace("<body>", "<body style=\"background-color:black;\">");
            // set all text in htmlString to white
            htmlString = htmlString.Replace("<p>", "<p style=\"color:white;\">");
            htmlString = htmlString.Replace("<h1>", "<h1 style=\"color:white;\">");
            htmlString = htmlString.Replace("<h2>", "<h2 style=\"color:white;\">");

            // set webBrowser to display htmlString
            webBrowser.NavigateToString(htmlString);

            // make webBrowser visible
            webBrowser.Visibility = Visibility.Visible;

            // enable save button
            saveButton.IsEnabled = true;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog? saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            saveFileDialog.OverwritePrompt = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                try
                {
                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        writer.WriteLine(category);
                        writer.WriteLine(title);
                        writer.WriteLine(simpleFinalStory);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            // delete savefiledialog
            saveFileDialog = null;
        }
    }
}
