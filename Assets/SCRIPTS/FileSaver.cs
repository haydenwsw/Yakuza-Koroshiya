using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text;
using System.Linq;

// <Summary>

// This script handle score saveing

// </Summary>

public class FileSaver
{
    // saves score to .txt
    //[MenuItem("Tools/Write file")]
    public static void WriteString(int Score)
    {
        // get path
        string path = "Assets/ART/AUDIO/Resources/Score.txt";

        // write score to file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(Score);
        writer.Close();

        // saves the file
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load<TextAsset>("Score");
    }

    //[MenuItem("Tools/Read file")]
    public static string ReadString()
    {
        // get path
        string path = "Assets/ART/AUDIO/Resources/Score.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string Score = reader.ReadToEnd();
        reader.Close();

        // return the score
        return Score;
    }
}
