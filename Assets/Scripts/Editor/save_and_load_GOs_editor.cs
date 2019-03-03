using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

//[InitializeOnLoad]

//[CustomEditor(typeof(save_and_load_GOs))]
public class save_and_load_GOs_editor : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        LoadObjectsIntoGame_editor();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    [MenuItem("MyTools/CreateGameObjects2")]
    static void LoadObjectsIntoGame_editor()
    {
        GameObject go = GameObject.Find("@state_scripts");
        save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));
        string path = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\test_state.txt";
        myscript.LoadObjectsIntoGame(path);
    }

    //[MenuItem("MyTools/CreateGameObjects")]
    static void Create()
    {
        Debug.Log("Yeah baby");
        for (int x = 0; x != 10; x++)
        {
            string prefab_path = "Prefabs/sheep";
            GameObject prefab = (GameObject)Resources.Load(prefab_path);
            GameObject go = (GameObject)Instantiate(prefab);
            go.transform.position = new Vector3(x, 0, 0);
        }
    }

}



