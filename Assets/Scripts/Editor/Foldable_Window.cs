using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Foldable_Window : EditorWindow
{

    bool showPosition = true;
    string status = "Select a GameObject";
    bool isSelected = false;
    string selectedStateName = "";
    public static List<string> stateNames = new List<string>();

    public static List<bool> isSelectedList = new List<bool>();

    public static string initialStateName = "State_000010000";
    public static string initialStateName2 = "State_000010001";

    public static bool initialBool = false;
    public static bool initialBool2 = false;


    int selectedSize = 1;
    string[] names = new string[] { "Normal", "Double", "Quadruple" };
    int[] sizes = { 1, 2, 4 };





    [MenuItem("MyTools/Foldout Usage")]
    static void Init()
    {
        Foldable_Window window = GetWindow<Foldable_Window>(); //(Foldable_Window)GetWindow(typeof(Foldable_Window));
        window.Show();

        stateNames.Clear();
        isSelectedList.Clear();

        stateNames.Add(initialStateName);
        stateNames.Add(initialStateName2);

        isSelectedList.Add(initialBool);
        isSelectedList.Add(initialBool2);
        // helllo



    }

    public void OnGUI()
    {


        selectedSize = EditorGUILayout.IntPopup("Resize Scale: ", selectedSize, names, sizes);

        string stateName = "";


        
        for (int i = 0; i < stateNames.Count; i++ )
        {
            stateName = stateNames[i];
            isSelectedList[i] = EditorGUILayout.Toggle(stateName, isSelectedList[i]);
            isSelectedList[i] = isSelectedList[i];
            if(isSelected)
            {
                selectedStateName = stateNames[i];
            }

        }
        
        if (GUILayout.Button("Show state name"))
        {
            for (int i = 0; i < stateNames.Count; i++)
            {
                if (isSelectedList[i])
                {
                    Debug.Log("State name = " + stateNames[i]);
                }
            }
        }




        if (!Selection.activeTransform)
        {
            status = "Select a GameObject";
            showPosition = false;
        }




        //////////////////////////////

        //GUILayout.Button("Btn", GetBtnStyle());
        //GUI.Button("Btn");
        //GUI.backgroundColor = Color.yellow;
        








    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }




    GUIStyle GetBtnStyle()
    {
        var s = new GUIStyle(GUI.skin.button);
        var b = s.border;
        b.left = 0;
        b.top = 0;
        b.right = 0;
        b.bottom = 0;
        //GUI.backgroundColor = Color.yellow;
        //s.normal.background = Color.yellow;

        return s;
    }


    public string nextStateString(string stateName)
    {
        string nextStateName;
        nextStateName = stateName.Substring(stateName.Length - 9);
        int x = Convert.ToInt32(nextStateName);
        x += 1;
        nextStateName = "State_" + x.ToString();
        //State_000010000
        return nextStateName;
    }




}
