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

    string[] _rotation_type_choices = new[] { "Time", "Speed" };
    int _rotation_type_choiceIndex = 0;

    string[] _rotationspeed_fct_choices = new[] { "linear", "easeInOutSine" };
    int _rotationspeed_fct_choiceIndex = 0;


    bool RotTranslSyncBtn = true;
    bool foldout_sound = false;
    bool foldout_move = false;

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();
        var someClass = target as extra_go_params;

        EditorGUILayout.LabelField("______________________________");

        /////////////
        //// SOUNDS
        /////////////
        ///
        EditorGUI.BeginChangeCheck ();
        EditorGUILayout.GetControlRect(true, 16f, EditorStyles.foldout);
        Rect foldRect = GUILayoutUtility.GetLastRect();
        
        if (Event.current.type == EventType.MouseUp && foldRect.Contains(Event.current.mousePosition))
        {
            foldout_sound = !foldout_sound;
            GUI.changed = true;
            Event.current.Use();
        }

        foldout_sound = EditorGUI.Foldout(foldRect, foldout_sound, "Sounds");
        if (foldout_sound)
        {
            //EditorGUILayout.LabelField("Sounds", EditorStyles.boldLabel);
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
        }






        /////////////
        //// MOVETYPES
        /////////////
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.GetControlRect(true, 16f, EditorStyles.foldout);
        Rect foldRect_move = GUILayoutUtility.GetLastRect();
        if (Event.current.type == EventType.MouseUp && foldRect_move.Contains(Event.current.mousePosition))
        {
            foldout_move = !foldout_move;
            GUI.changed = true;
            Event.current.Use();
        }

        foldout_move = EditorGUI.Foldout(foldRect_move, foldout_move, "Move");
        if (foldout_move)
        {
            EditorGUILayout.LabelField("MOVETYPES", EditorStyles.boldLabel);
            // Translation
            EditorGUILayout.LabelField("Translation", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("  By Time or Speed");
            _move_type_choiceIndex = EditorGUILayout.Popup(_move_type_choiceIndex, _move_type_choices);
            someClass.MoveType = _move_type_choices[_move_type_choiceIndex];

            switch (someClass.MoveType)
            {
                case "Time":
                    someClass.MoveTime = EditorGUILayout.FloatField(" MoveTime:", someClass.MoveTime);
                    someClass.value = someClass.MoveTime;
                    break;
                case "Speed":
                    EditorGUILayout.LabelField(" Speed Function");
                    _speed_fct_choiceIndex = EditorGUILayout.Popup(_speed_fct_choiceIndex, _speed_fct_choices);
                    someClass.SpeedFct = _speed_fct_choices[_speed_fct_choiceIndex];
                    someClass.Speed = EditorGUILayout.FloatField(" Speed:", someClass.Speed);
                    someClass.value = someClass.Speed;
                    break;
            }


            RotTranslSyncBtn = EditorGUILayout.Toggle("Sync Transl & Rot", RotTranslSyncBtn);
            if (!RotTranslSyncBtn)
            {
                // Rotation
                EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("  By Time or Speed");
                _rotation_type_choiceIndex = EditorGUILayout.Popup(_rotation_type_choiceIndex, _rotation_type_choices);
                someClass.RotationType = _rotation_type_choices[_rotation_type_choiceIndex];

                switch (someClass.RotationType)
                {
                    case "Time":
                        someClass.RotationTime = EditorGUILayout.FloatField(" RotationTime:", someClass.RotationTime);
                        break;
                    case "Speed":
                        EditorGUILayout.LabelField(" Rotation Speed Function");
                        _rotationspeed_fct_choiceIndex = EditorGUILayout.Popup(_rotationspeed_fct_choiceIndex, _rotationspeed_fct_choices);
                        someClass.RotationSpeedFct = _rotationspeed_fct_choices[_rotationspeed_fct_choiceIndex];
                        someClass.RotationSpeed = EditorGUILayout.FloatField(" Speed:", someClass.RotationSpeed);
                        break;
                }

            }
            else
            {
                someClass.RotationType = someClass.MoveType;
                someClass.RotationTime = someClass.RotationTime;
                someClass.RotationSpeedFct = someClass.SpeedFct;
                someClass.RotationSpeed = someClass.Speed;
            }

        } // END Move Foldout

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
