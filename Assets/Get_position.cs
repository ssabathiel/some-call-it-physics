using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OwnGameObject
{
    public string Name { get; set; }
    public int Position{ get; set; }
}


public class Get_position : MonoBehaviour {

    
    


    // Use this for initialization
    void Start () {
        GameObject Protagonists = GameObject.Find("Protagonists");
        List<GameObject> protagonist_list = new List<GameObject>();
        int i = 0;
        //Vector3 pos = GameObject.Find("ObjectX").transform.position;
        foreach (Transform thisObject in Protagonists.transform)
        {
            
            //Add to MediumAreas List
            protagonist_list.Add(thisObject.gameObject);
            Debug.Log("protagonistss: " + i);
            Debug.Log(thisObject.gameObject.transform);
            Debug.Log(thisObject.gameObject.transform.position);
            string json = JsonUtility.ToJson(thisObject);
            Debug.Log("JSON" + json);


            i += 1;
        }

        //string json = JsonUtility.ToJson(myObject);
        //Debug.Log("All protagonists: ", protagonist_list);

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log( GameObject.Find("sheep").transform.position ) ;
    }
}
