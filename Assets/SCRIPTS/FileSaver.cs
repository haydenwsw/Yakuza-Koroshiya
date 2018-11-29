using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FileSaver
{
    [MenuItem("Tools/Write file")]
    public static void WriteString(int Score)
    {
        string path = "Assets/ART/AUDIO/Resources/Score.txt";

        string[] file = ReadString().Split('\n');

        int[] Sorted = new int[file.Length];
        for (int i = 0; i < file.Length; i++)
        {
            //Sorted[i]
        }

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(Score);
        writer.Close();

        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load<TextAsset>("Score");
    }

    [MenuItem("Tools/Read file")]
    public static string ReadString()
    {
        string path = "Assets/ART/AUDIO/Resources/Score.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string Score = reader.ReadToEnd();
        reader.Close();

        return Score;
    }
}
