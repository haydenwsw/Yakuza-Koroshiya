using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Custominspector : Editor {

    class Custom
    {
        public int X;
        public int Y;

        public Custom(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    //public Custom c;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        GUILayout.Label("ahh");
    }

}
