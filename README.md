# offhand-dialogue

This is a .NET WPF application that will take a .txt file as input, replace matching regex with user input, and print the final result with all replaced words.

The default regex is \\[.+?\\] to catch any set of characters between square brackets.  

#### Accepted Format for .txt File Input  
```
Category of Story  
Title of Story  
Full story  
```
The .txt file takes markdown into consideration outside of any bracket pairs.

#### Example of .txt File Input  
```
Comedy
Time for Crab
Oh, how I wish I was a Crab. I would be so *[adjective]*. 
I would **[adverb]** pinch people's ***[part of body (plural)]*** 
and get away before they could even [verb] me. 
```

The form will then list all regex matches in order with a text box next to each one.  

#### Example of empty form  
![image](https://user-images.githubusercontent.com/70172268/194927603-6d00fd0d-64e0-4859-8710-0fd1c34c8f48.png)

#### Example of filled out form  
![image](https://user-images.githubusercontent.com/70172268/194927700-3842bb35-2647-4c77-a96a-5b6518ef1f5e.png)

After filling out all text boxes and clicking the submit button, the application displays the completed story.

#### Example of completed story
![image](https://user-images.githubusercontent.com/70172268/194927798-2b251ad3-4c0a-4934-9759-aaed0fd3b894.png)
