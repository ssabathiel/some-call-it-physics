using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        //GameObject obj = Selection.activeGameObject;
        //Debug.Log("Name: " + obj.name);
    }

}