using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode()]
public class ObjectBuilderScript : MonoBehaviour
{
    public GameObject obj;
    public Vector3 spawnPoint;


    public void BuildObject()
    {
        Instantiate(obj, spawnPoint, Quaternion.identity);
    }


    void OnDestroy()
    {
        Debug.Log("Sth has been destroyed " + this.gameObject.name);


        ///////////////////////////////////
        ///


        GameObject sel_state = this.gameObject;


        Transform sel_state_parent = sel_state.transform.parent;

        int sel_state_number = getStateNumber(sel_state);



        foreach (Transform child in sel_state_parent)
        {
            int stateNumber = getStateNumber(child.gameObject);
            if (stateNumber > sel_state_number)
            {
                child.gameObject.name = prevStateString(child.gameObject.name);
            }
        }

        

        SortChildrenByName();





        /////////////////////////////////////////////////////////////////////////

        //GameObject obj = Selection.activeGameObject;
        //Debug.Log("Name: " + obj.name);
    }







    public string prevStateString(string stateName)
    {
        int num_of_digits = 11;
        string nextStateName;
        nextStateName = stateName.Substring(stateName.Length - num_of_digits);
        int x = Convert.ToInt32(nextStateName);
        x -= 1;
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















}