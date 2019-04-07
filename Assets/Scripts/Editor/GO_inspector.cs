using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(extra_go_params))]
public class GO_inspector : Editor {

    string[] _appear_sound_choices = new[] { "none", "blop" };
    int _appear_choiceIndex = 0;

    string[] _move_sound_choices = new[] { "none", "swipe" };
    int _move_choiceIndex = 0;

    string[] _disappear_sound_choices = new[] { "none", "swipe" };
    int _disappear_choiceIndex = 0;


    string[] _move_type_choices = new[] { "Time", "Speed" };
    int _move_type_choiceIndex = 0;

    string[] _speed_fct_choices = new[] { "linear", "easeInOutSine" };
    int _speed_fct_choiceIndex = 0;


    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();
        var someClass = target as extra_go_params;

        EditorGUILayout.LabelField("______________________________");

        /////////////
        //// SOUNDS
        /////////////
        EditorGUILayout.LabelField("Sounds", EditorStyles.boldLabel);
        // Appear sounds
        EditorGUILayout.LabelField("  Appear Sound:");
        _appear_choiceIndex = EditorGUILayout.Popup(_appear_choiceIndex, _appear_sound_choices);
        someClass.Appear_Sound = _appear_sound_choices[_appear_choiceIndex];

        // Move sounds
        EditorGUILayout.LabelField("  Move Sound:");
        _move_choiceIndex = EditorGUILayout.Popup(_move_choiceIndex, _move_sound_choices);
        someClass.Move_Sound = _move_sound_choices[_move_choiceIndex];

        // Disappear sounds
        EditorGUILayout.LabelField("  Disappear Sound:");
        _disappear_choiceIndex = EditorGUILayout.Popup(_disappear_choiceIndex, _disappear_sound_choices);
        someClass.Disappear_Sound = _disappear_sound_choices[_disappear_choiceIndex];





        /////////////
        //// MOVETYPES
        /////////////
        EditorGUILayout.LabelField("MOVETYPES", EditorStyles.boldLabel);
        // Time regulated
        EditorGUILayout.LabelField("  By Time or Speed");
        _move_type_choiceIndex = EditorGUILayout.Popup(_move_type_choiceIndex, _move_type_choices);
        someClass.MoveType = _move_type_choices[_move_type_choiceIndex];

        switch (someClass.MoveType)
        {
            case "Time":
                someClass.MoveTime = EditorGUILayout.FloatField(" MoveTime:", someClass.MoveTime);
                break;
            case "Speed":
                EditorGUILayout.LabelField(" Speed Function");
                _speed_fct_choiceIndex = EditorGUILayout.Popup(_speed_fct_choiceIndex, _speed_fct_choices);
                someClass.SpeedFct = _speed_fct_choices[_speed_fct_choiceIndex];
                someClass.Speed = EditorGUILayout.FloatField(" Speed:", someClass.Speed);
                break;
        }



        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
