using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;



[CustomEditor(typeof(FSM_naming_manager))]
public class FSM_naming_manager_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FSM_naming_manager myScript = (FSM_naming_manager)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.BuildObject();
        }



        if (GUILayout.Button("Restructure state names"))
        {
            GameObject statess = GameObject.Find("@state_data");
            int state_i = 0;
            RenameStates1toN(statess, ref state_i);
        }
        

        if (GUILayout.Button("New State"))
        {
            int num_of_digits = 11;
            GameObject sel_state = Selection.activeGameObject;
            int sel_state_number = getStateNumber(sel_state);

            Transform sel_state_parent = sel_state.transform.parent;

            //When state inbetween states is created, all statenames larger then that must be increased
            GameObject statess = GameObject.Find("@state_data");
            int nextSiblingNumber_var = GetNextSiblingNumber(sel_state);
            Debug.Log("sel_state_number " + sel_state_number);
            Debug.Log("nextSiblingNumber_var " + nextSiblingNumber_var);
            ShiftUpcomingNames(statess, nextSiblingNumber_var);

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



        }


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
                child.gameObject.name = nextStateString(child.gameObject.name);
            }

            ShiftUpcomingNames(child.gameObject, sel_state_number);
        }
    }

    public static void RenameStates1toN(GameObject statess, ref int state_i_)
    {
        //https://gist.github.com/AShim3D/d76e2026c5655b3b34e2

        //foreach (Transform child in sel_state_parent)

        int initial_i = 00000100000;
        int num_of_digits = 11;

        foreach (Transform child1 in statess.transform)
        {
            GameObject obj = child1.gameObject;
            int curr_i = initial_i + state_i_;
            obj.name = "state_" + curr_i.ToString(new String('0', num_of_digits));

            state_i_++;
            RenameStates1toN(obj, ref state_i_);

            
        }

        

    }




    /*
    //https://answers.unity.com/questions/287497/how-to-subscribe-onto-hierarchy-change-event-when.html
    void OnEnable()
    {
        EditorApplication.hierarchyWindowChanged += HierarchyChanged;
    }

    void OnDisable()
    {
        EditorApplication.hierarchyWindowChanged -= HierarchyChanged;
    }

    private void HierarchyChanged()
    {
        SortChildrenByName();
    }
    */


    public void ShiftDownAfterDestroy()
    {
        Debug.Log("Should shift down here ");
    }



}





