using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Foldable_Window : EditorWindow
{

    bool showPosition = true;
    string status = "Select a GameObject";

    [MenuItem("MyTools/Foldout Usage")]
    static void Init()
    {
        Foldable_Window window = (Foldable_Window)GetWindow(typeof(Foldable_Window));
        window.Show();
    }

    public void OnGUI()
    {
        showPosition = EditorGUILayout.Foldout(showPosition, status);
        if (showPosition)
            if (Selection.activeTransform)
            {
                Selection.activeTransform.position =
                    EditorGUILayout.Vector3Field("Position", Selection.activeTransform.position);
                status = Selection.activeTransform.name;
            }

        if (!Selection.activeTransform)
        {
            status = "Select a GameObject";
            showPosition = false;
        }
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
