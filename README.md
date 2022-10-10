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
![image](https://user-images.githubusercontent.com/70172268/191091703-a9f6f9a9-ccf9-4fb9-96ad-22230c32ca98.png)

#### Example of filled out form  
![image](https://user-images.githubusercontent.com/70172268/191092192-f2b6c718-53c6-45f8-b8f6-10a874c7f5b3.png)

After filling out all text boxes and clicking the submit button, the application displays the completed story.

#### Example of completed story
![image](https://user-images.githubusercontent.com/70172268/191851056-853b42da-24d7-4f1b-827d-e98a0b5e4540.png)
