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
        private AdLib[]? adLibTypes;
        private string? category, title, rawStory;
        public Story(string category, string title, string rawStory, AdLib[] adLibTypes)
        {
            this.category = category;
            this.title = title;
            this.rawStory = rawStory;
            this.adLibTypes = adLibTypes;
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
            if (adLibTypes != null) return adLibTypes;
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
                        // if entry contains brackets, return null and display messagebox
                        else if (entry.Contains("[") || entry.Contains("]"))
                        {
                            MessageBox.Show("Please do not use brackets in your ad libs.");
                            // exit method
                            return "";
                        }
                        else
                        {
                            // delete any brackets from entry
                            // entry = entry.Replace("[", "");
                            // entry = entry.Replace("]", "");
                            
                            entry = Regex.Escape(entry);
                            // replace "\ " with " " in entry
                            entry = entry.Replace("\\ ", " ");
                            
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