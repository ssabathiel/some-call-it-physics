using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;



[CustomEditor(typeof(ObjectBuilderScript))]
public class ObjectBuilderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectBuilderScript myScript = (ObjectBuilderScript)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.BuildObject();
        }


        if (GUILayout.Button("Initialize Buttons"))
        {
            GameObject statess = GameObject.Find("@state_data");
            foreach (Transform child in statess.transform)
            {
                child.gameObject.AddComponent<ObjectBuilderScript>();
            }
        }




        if (GUILayout.Button("Get Name of selected object"))
        {
            GameObject obj = Selection.activeGameObject;
            Debug.Log("Name: " + obj.name);
        }


        if (GUILayout.Button("New State"))
        {
            GameObject sel_state = Selection.activeGameObject;

            string next_state_string = nextStateString(sel_state.name);

            Transform sel_state_parent = sel_state.transform.parent;

            int sel_state_number = getStateNumber(sel_state);
 


            foreach (Transform child in sel_state_parent)
            {
                int stateNumber = getStateNumber(child.gameObject);
                if (stateNumber > sel_state_number)
                {
                    child.gameObject.name = nextStateString(child.gameObject.name);
                }
            }

            string prefab_path = "Prefabs/state_prefab";
            GameObject prefab = (GameObject)Resources.Load(prefab_path);
            GameObject newObject1 = (GameObject)Instantiate(prefab);
            newObject1.name = next_state_string;

            //GameObject sel_state_parent = sel_state.transform.parent;
            newObject1.transform.parent = sel_state.transform.parent;

            SortChildrenByName();

        }


    }


    public string nextStateString(string stateName)
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

    public int getStateNumber(GameObject state)
    {
        int num_of_digits = 11;
        string stateName = state.name.Substring(state.name.Length - num_of_digits);
        int stateNumber = Convert.ToInt32(stateName);

        return stateNumber;
    }



    public static void SortChildrenByName()
    {
        //https://gist.github.com/AShim3D/d76e2026c5655b3b34e2

        //foreach (Transform child in sel_state_parent)
        GameObject statess = GameObject.Find("@state_data");
        foreach (Transform child1 in statess.transform)
        {
            GameObject obj = child1.gameObject;
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
        }

    }


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






}



/*
public class ObjectChecker : MonoBehavior
{
    //Nothing needed here
}

 
[CustomEditor(typeof(ObjectChecker))]
public class ObjectChecker : Editor
{
    public void OnDestroy()
    {
        if (((ObjectChecker)target) == null)
        {
            Debug.Log("Element destroyed");
        }
    }
}
*/

