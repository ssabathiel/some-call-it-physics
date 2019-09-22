using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;



[CustomEditor(typeof(FSM_naming_manager))]
public class FSM_naming_manager_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();



        if (GUILayout.Button("Restructure state names"))
        {
            GameObject statess = GameObject.Find("@state_data");
            int state_i = 0;
            RenameStates1toN(statess, ref state_i);
            IntermediateFiles2Files(statess, state_i);
        }
        

        if (GUILayout.Button("Add Current Scene"))
        {

            string next_state_string = "";
            AddState(ref next_state_string);

            //Save current scene to corresponding file name
            GameObject go = GameObject.Find("@state_scripts");
            save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));
            string pathy = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States\" + next_state_string;
            myscript.SaveCurrentScene2File(pathy);



        }

        if (GUILayout.Button("Save scene as prefab"))
        {
            string path = "Assets/Resources/Prefabs/whole_scene.prefab";
            GameObject sel_state = Selection.activeGameObject;

            //GameObject prefab = PrefabUtility.CreatePrefab(path, sel_state);
            //Directory.CreateDirectory("Assets/Resources/Prefabs/new_directory");



        }


        if (GUILayout.Button("Copy state"))
        {

            string next_state_string = "";
            AddState(ref next_state_string);

            GameObject sel_state = Selection.activeGameObject;

            Debug.Log("next_state_string " + next_state_string);
            Debug.Log("sel_state.name " + sel_state.name);


            //Copy File
            string sourcefileName = sel_state.name;
            string targetfileName = next_state_string;
            string sourcePath = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States";
            string targetPath = sourcePath;

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, sourcefileName);
            string destFile = System.IO.Path.Combine(targetPath, targetfileName);

            System.IO.File.Copy(sourceFile, destFile, true);


        }



        if (GUILayout.Button("Load Scene"))
        {

            GameObject sel_state = Selection.activeGameObject;

            //Save current scene to corresponding file name
            GameObject go = GameObject.Find("@state_scripts");
            save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));
            string pathy = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States\" + sel_state.name;


            myscript.LoadSceneIntoGame(pathy);
            myscript.active_state = sel_state.name;


        }



        if (GUILayout.Button("Load Scene dynamically"))
        {

            GameObject sel_state = Selection.activeGameObject;

            //Save current scene to corresponding file name
            GameObject go = GameObject.Find("@state_scripts");
            save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));
            string pathy = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States\" + sel_state.name;

            myscript.LoadObjectsIntoGame_dynamically(pathy);

            myscript.active_state = sel_state.name;

        }


        if (GUILayout.Button("Delete Scene"))
        {

            GameObject sel_state = Selection.activeGameObject;

            DestroyGameObject(sel_state);


        }



    }

    public void DestroyGameObject(GameObject go)
    {
        
        string sourcefileName = go.name;
        string sourcePath = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States";
        string sourceFile = System.IO.Path.Combine(sourcePath, sourcefileName);
        System.IO.File.Delete(sourceFile);
        DestroyImmediate(go);

        GameObject statess = GameObject.Find("@state_data");
        int state_i = 0;
        RenameStates1toN(statess, ref state_i);
        IntermediateFiles2Files(statess, state_i);

    }

    public static void AddState(ref string added_file_path)
    {
        int num_of_digits = 11;
        GameObject sel_state = Selection.activeGameObject;

        Transform sel_state_parent = sel_state.transform.parent;

        //When state inbetween states is created, all statenames larger then that must be increased
        GameObject statess = GameObject.Find("@state_data");
        int nextSiblingNumber_var = GetNextSiblingNumber(sel_state);

        
        ShiftUpcomingNames(statess, nextSiblingNumber_var);
        IntermediateFiles2Files(statess, nextSiblingNumber_var);

        //Create new object with prefab (added script as component)
        string prefab_path = "Prefabs/state_prefab";
        GameObject prefab = (GameObject)Resources.Load(prefab_path);
        GameObject newObject1 = (GameObject)Instantiate(prefab);

        // Now new state can be renamed
        nextSiblingNumber_var += 1;
        string next_state_string = "state_" + nextSiblingNumber_var.ToString(new String('0', num_of_digits));
        newObject1.name = next_state_string;

        // Put new state in the right level in the hierarchy (same as selected state)
        newObject1.transform.parent = sel_state.transform.parent;

        SortChildrenByName(sel_state);

        added_file_path = next_state_string;

    }


    public static string nextStateString(string stateName)
    {
        int num_of_digits = 11;
        string nextStateName;
        nextStateName = stateName.Substring(stateName.Length - num_of_digits);
        int x = Convert.ToInt32(nextStateName);
        x += 1;
        nextStateName = "state_" + x.ToString(new String('0', num_of_digits));
        //State_000010000
        return nextStateName;
    }

    
    public static int getStateNumber(GameObject state)
    {
        int num_of_digits = 11;
        string stateName = state.name.Substring(state.name.Length - num_of_digits);
        int stateNumber = Convert.ToInt32(stateName);

        return stateNumber;
    }
    



    public static void SortChildrenByName(GameObject go)
    {
        //https://gist.github.com/AShim3D/d76e2026c5655b3b34e2
        //Debug.Log("Here to sort");
        //foreach (Transform child in sel_state_parent)
        Transform statess = go.transform.parent;
        //foreach (Transform child1 in statess.transform)
        //{
        //GameObject obj = child1.gameObject;
        GameObject obj = statess.gameObject;
            List<Transform> children = new List<Transform>();
            for (int i = obj.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = obj.transform.GetChild(i);
                children.Add(child);
                child.parent = null;
            }
            children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
            foreach (Transform child in children)
            {
                child.parent = obj.transform;
            }
        //}

    }


    public static int GetNextSiblingNumber(GameObject go)
    {
        int stateNumber = getStateNumber(go);
        int nextSiblingNumber = stateNumber;
        foreach(Transform child in go.transform)
        {
            nextSiblingNumber = getStateNumber(child.gameObject);
        }

        return nextSiblingNumber;
    }

    public static void ShiftUpcomingNames(GameObject go, int sel_state_number)
    {
        foreach (Transform child in go.transform)
        {
            int stateNumber = getStateNumber(child.gameObject);
            if (stateNumber > sel_state_number)
            {
                string sourcefileName = child.gameObject.name;
                child.gameObject.name = nextStateString(child.gameObject.name);


                string sourcePath = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States";
                string targetPath = sourcePath;
                string targetfileName = nextStateString(sourcefileName) + "_";

                string sourceFile = System.IO.Path.Combine(sourcePath, sourcefileName);
                string destFile = System.IO.Path.Combine(targetPath, targetfileName);
                System.IO.File.Copy(sourceFile, destFile, true); // Copy, since Move does not work on networks: see stackoverfl: "Rename a file in C#"
            }

            ShiftUpcomingNames(child.gameObject, sel_state_number);
        }


    }



    public static void IntermediateFiles2Files(GameObject go, int sel_state_number)
    {

        //int initial_i = 00000100000;
        //int num_of_digits = 11;

        foreach (Transform child in go.transform)
        {
            int stateNumber = getStateNumber(child.gameObject);
            if (stateNumber > sel_state_number)
            {
                string sourcefileName = child.gameObject.name + "_";
                string sourcePath = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States";
                string targetPath = sourcePath;
                string targetfileName = child.gameObject.name;

                string sourceFile = System.IO.Path.Combine(sourcePath, sourcefileName);
                string destFile = System.IO.Path.Combine(targetPath, targetfileName);
                System.IO.File.Copy(sourceFile, destFile, true); // Copy, since Move does not work on networks: see stackoverfl: "Rename a file in C#"
                System.IO.File.Delete(sourceFile);
            }

            IntermediateFiles2Files(child.gameObject, sel_state_number);
        }


    }




    public static void RenameStates1toN(GameObject statess, ref int state_i_)
    {
        //https://gist.github.com/AShim3D/d76e2026c5655b3b34e2

        int initial_i = 00000100000;
        int num_of_digits = 11;

        foreach (Transform child1 in statess.transform)
        {
            GameObject obj = child1.gameObject;
            string sourcefileName = obj.name;

            int curr_i = initial_i + state_i_;
            obj.name = "state_" + curr_i.ToString(new String('0', num_of_digits));

            // FILE management
            
            string sourcePath = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States";
            string targetPath = sourcePath;
            string targetfileName = obj.name + "_";

            string sourceFile = System.IO.Path.Combine(sourcePath, sourcefileName);
            string destFile = System.IO.Path.Combine(targetPath, targetfileName);
            System.IO.File.Copy(sourceFile, destFile, true); // Copy, since Move does not work on networks: see stackoverfl: "Rename a file in C#"
            System.IO.File.Delete(sourceFile);


            state_i_++;
            RenameStates1toN(obj, ref state_i_);

            
        }

        

    }



    public void ShiftDownAfterDestroy()
    {
        Debug.Log("Should shift down here ");
    }



}





