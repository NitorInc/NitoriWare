using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DatingSimGirl : MonoBehaviour {

    [MenuItem("Tools/Read file")]
    static void ReadString()
    {
        string path = "Assets/Resources/Microgames/DatingSim/Scripts/DatingSimDialogue.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); //change this so it doesn't print out
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }//from here put into a method (or modify into the struct) of temp


    /// <summary>
    /// string text = System.IO.File.ReadAllText("myfile.txt");
    /// use that as well
    /// 
    /// other reading:
    ///       using (theReader)
    ///  {
    ///      line = theReader.ReadLine();
    ///      if(line != null){
    ///              // While there's lines left in the text file, do this:
    ///              do
    ///              {
    ///                     // Do whatever you need to do with the text line, it's a string now
    ///                     // In this example, I split it into arguments based on comma
    ///                     // deliniators, then send that array to DoStuff()
    ///                     string[] entries = line.Split(',');
    ///                     if (entries.Length > 0)
    ///                     DoStuff(entries);
    ///line = theReader.ReadLine();
    ///              }
    ///              while (line != null);
    ///      } 
    ///      // Done reading, close the reader and return true to broadcast success    
    ///      theReader.Close();
    ///      return true;
    ///      }
    ///}
    ///
    /// other reading:
    /// import System.IO;
    ///var fileName = "foo.txt";
    ///
    ///function Start()
    ///{
    ///    var sr = new StreamReader(Application.dataPath + "/" + fileName);
    ///    var fileContents = sr.ReadToEnd();
    ///    sr.Close();
    ///
    ///    var lines = fileContents.Split("\n"[0]);
    ///    for (line in lines)
    ///    {
    ///        print(line);
    ///    }
    ///}
    /// probably use the last one; it looks like what we need
    /// We need to be able to read in the lines, (probably on a line by line basis) modify the input to remove the "*" and ":", separate each set into entries,
    /// then put those entry sets into structs for each girl, these structs would be put into a dyn array to rng select from and choose
    /// </summary>

    // Use this for initialization
    void Start () {
        //read in text from file as string
        //default it
        char[] name = new char[30];
        char[] Dialogue = new char[300];
        char[] choice1 = new char[100];
        char[] choice2 = new char[100];
        char[] choice3 = new char[100];
        char[] choice4 = new char[100];
        //probably need 30-50 sized array for choices to save space
        //replace the loop's instruction with a reader
        for (int i = 0; i < 30; i++)
        {
            name[i] = 'a';
        }
        for (int i = 0; i < 300; i++)
        {
           Dialogue[i] = 'a';
        }
        for (int i = 0; i < 100; i++)
        {
            choice1[i] = 'a';
        }
        for (int i = 0; i < 100; i++)
        {
            choice2[i] = 'a';
        }
        for (int i = 0; i < 100; i++)
        {
            choice3[i] = 'a';
        }
        for (int i = 0; i < 100; i++)
        {
            choice4[i] = 'a';
        }
        //string result = new string(name);
        //work more on it later
        // work on now

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
