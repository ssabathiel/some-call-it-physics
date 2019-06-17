using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

[ExecuteInEditMode()]
public class FSM_naming_manager : MonoBehaviour
{
    public GameObject obj;
    public Vector3 spawnPoint;


    public void BuildObject()
    {
        Instantiate(obj, spawnPoint, Quaternion.identity);
    }


    void OnDestroy()
    {
        //Debug.Log("Sth has been destroyed " + this.gameObject.name);


        GameObject sel_state = this.gameObject;
        Transform sel_state_parent = sel_state.transform.parent;
        int sel_state_number = getStateNumber(sel_state);

        foreach (Transform child in sel_state_parent)  // why loop only through siblings?
        {
            int stateNumber = getStateNumber(child.gameObject);
            if (stateNumber >= sel_state_number)
            {
                //child.gameObject.name = prevStateString(child.gameObject.name);
            }
        }
      
        //SortChildrenByName(sel_state);

        //File management
        string sourcefileName = sel_state.name;
        string sourcePath = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States";
        string sourceFile = System.IO.Path.Combine(sourcePath, sourcefileName);
        //System.IO.File.Delete(sourceFile);    // responsible for deleting all files when running. shit.   
        //DestroyImmediate(sel_state);

        GameObject statess = GameObject.Find("@state_data");
        int state_i = 0;
        RenameStates1toN(statess, ref state_i);
        IntermediateFiles2Files(statess, state_i);

    }






    public string prevStateString(string stateName)
    {
        int num_of_digits = 11;
        string nextStateName;
        nextStateName = stateName.Substring(stateName.Length - num_of_digits);
        int x = Convert.ToInt32(nextStateName);
        x -= 1;
        nextStateName = "state_" + x.ToString(new String('0', num_of_digits));
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

        //foreach (Transform child in sel_state_parent)

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



}