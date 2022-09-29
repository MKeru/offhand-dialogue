using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System;

namespace offhand_dialogue_wpf
{
    public class Story
    {
        private AdLib[]? adLibsArray;
        private string? category, title, rawStory;
        private bool successfulRead = false;
        public Story()
        {
            // clear all fields
            this.adLibsArray = null;
            this.category = null;
            this.title = null;
            this.rawStory = null;
            this.successfulRead = false;

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
                        if (reader.ReadToEnd() == null)
                        {
                            MessageBox.Show("There must be a story in the file. Please submit a valid file.");
                            // throw exception
                            throw new Exception("There must be a story in the file. Please submit a valid file.");
                        }

                        // regex for any ad lib in the story
                        Regex normRegex = new Regex("\\[.+?\\]");
                        MatchCollection matches = normRegex.Matches(rawStory);

                        adLibsArray = new AdLib[matches.Count];
                        for (int i = 0; i < matches.Count; i++)
                        {
                            // add ad lib to array
                            adLibsArray[i] = new AdLib(matches[i].Value);
                        }

                        successfulRead = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            else
            {
                successfulRead = false;
                return;
            }
        }

        public bool IsSuccessful()
        {
            return successfulRead;
        }

        public string GetCategory()
        {
            if (category != null) return category;
            else throw new Exception("Category is null.");
        }

        public string GetTitle()
        {
            if (title != null) return title;
            else throw new Exception("Title is null.");
        }

        public string GetRawStory()
        {
            if (rawStory != null) return rawStory;
            else throw new Exception("Raw Story is null.");
        }

        public AdLib[] GetAdlibsArray()
        {
            if (adLibsArray != null) return adLibsArray;
            else throw new Exception("AdLibs Array is null.");
        }

        public string GetFinalStory(StackPanel panel)
        {
            // check if rawStory is null
            if (rawStory == null)
            {
                MessageBox.Show("Error in GetFinalStory: Raw Story is null.");
                // throw exception
                throw new Exception("Error in GetFinalStory: Raw Story is null.");
            }

            string finalStory = rawStory; // finalStory is the raw story with the ad libs filled in
            string? entry = null; // string to hold the text from the text box
            Regex regex = new Regex("\\[.+?\\]"); // regex for any ad lib in the story

            // create stringbuilder
            StringBuilder sb = new StringBuilder();

            try
            {
                // throw exception if any text box in the panel is empty
                for (int i = 0; i < panel.Children.Count; i++)
                {
                    if (panel.Children[i] is TextBox)
                    {
                        // assign text from text box to entry
                        entry = ((TextBox)panel.Children[i]).Text;
                        if (entry == "" || entry == null)
                        {
                            MessageBox.Show("Please fill in all ad libs.");
                            // exit method
                            return "";
                        }
                        else
                        {
                            entry = Regex.Escape(entry);
                            entry = "<u>" + entry + "</u>";
                            finalStory = regex.Replace(finalStory, entry, 1);
                        }
                    }
                }

                // delete double spaces from finalStory
                finalStory = finalStory.Replace("  ", " ");

                // add finalStory to stringbuilder
                sb.Append(finalStory);

                // return final story
                return sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetPlainFinalStory(string html)
        {
            // strip html tags from html
            string plainText = Regex.Replace(html, "<.*?>", String.Empty);

            // markdig delete any markdown
            plainText = Markdig.Markdown.ToPlainText(plainText);

            // delete double spaces from plainText
            plainText = plainText.Replace("  ", " ");

            // return plain text
            return plainText;
        }
    }
}