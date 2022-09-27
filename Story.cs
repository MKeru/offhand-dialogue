// function to read the chosen file and split it into an array of strings
// it will add a string to the array until it reaches an ad lib
// it will then add the ad lib to the array and continue

// first entry in the array is category
// second entry is title
// third entry and beyond are the ad libs

namespace offhand_dialogue;
using System.Text;
using System.Text.RegularExpressions;

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

        if (openFileDialog.ShowDialog() == DialogResult.OK)
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

    public string GetFinalStory(Panel panel)
    {
        // check if rawStory is null
        if (rawStory == null)
        {
            MessageBox.Show("Error in GetFinalStory: Raw Story is null.");
            // throw exception
            throw new Exception("Error in GetFinalStory: Raw Story is null.");
        }
        
        string finalStory = rawStory; // finalStory is the raw story with the ad libs filled in
        string entry; // string to hold the text from the text box
        Regex regex = new Regex("\\[.+?\\]"); // regex for any ad lib in the story

        // create stringbuilder
        StringBuilder sb = new StringBuilder();

        // add category then title to stringbuilder
        // sb.Append(category);
        // sb.Append("\r\n");
        // sb.Append(title);
        // sb.Append("\r\n");

        try
        {
            // throw exception if any text box in the panel is empty
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                entry = ((adLibField)panel.Controls[i]).textBox.Text;
                if (entry == "")
                {
                    // messagebox with error
                    MessageBox.Show("Please fill out all fields.");
                    throw new Exception("Please fill out all ad libs!");
                }
            }

            // replace regex matches in story with text box inputs from panel in order
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                // get text box input from panel
                entry = ((adLibField)panel.Controls[i]).textBox.Text;

                // add escape characters to entry in front of any special characters
                entry = Regex.Escape(entry);

                // add underline tags to entry
                entry = "<u>" + entry + "</u>";

                // replace regex match with text box input
                finalStory = regex.Replace(finalStory, entry, 1);
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
            MessageBox.Show("An error occurred while trying to create the final story. Please try again.");
            throw new Exception("An error occurred while trying to create the final story.");
        }
    }
}