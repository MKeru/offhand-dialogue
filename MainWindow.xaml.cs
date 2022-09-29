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
        public string? finalStory, title, category, htmlString, simpleFinalStory;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void selectFileButton_Click(object sender, RoutedEventArgs e)
        {
            // create new story object
            story = new Story();
            // check if story was not successfully read
            if (!story.IsSuccessful())
            {
                // do nothing
                return;
            }

            // get ad libs from story
            adLibs = story.GetAdlibsArray();

            // clear inputPanel
            inputPanel.Children.Clear();
            // add ad libs to inputPanel
            AddFields.AddFieldsToPanel(inputPanel, adLibs);

            // make inputScrollViewer visible
            // inputScrollViewer.Visibility = Visibility.Visible;

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

            // get simpleFinalStory
            simpleFinalStory = finalStory;
            // strip html tags from simpleFinalStory
            simpleFinalStory = Regex.Replace(simpleFinalStory, "<.*?>", String.Empty);
            // strip markdown from simpleFinalStory
            simpleFinalStory = Markdig.Markdown.ToPlainText(simpleFinalStory);

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
            htmlString = html.ToString();

            // set webBrowser to display htmlString
            webBrowser.NavigateToString(htmlString);

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
