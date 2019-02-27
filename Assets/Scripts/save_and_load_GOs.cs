using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
//using Newtonsoft.Json;

//Test commit2

public class save_and_load_GOs : MonoBehaviour
{
    string json;
    GameObjectInScene gameObjectInScene;
    
    public GameObjectList gameObjectList_new;
    public bool save = true;



    void Awake()
    {
        //gameObjectList = new List<GameObjectInScene>();
        gameObjectList_new = new GameObjectList();
    }

    void Start()
    {




        // From Objects to Json
        json = CurrentScene2Json();
        string path = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\test_state.txt";
        //System.IO.File.WriteAllText(path, json);
        Debug.Log("Well well");

        // From Json to Objects
        gameObjectList_new = GetObjectsFromJsonFile(path);


        // Load Objects to game
        //LoadObjectsIntoGame(path);








    }

    
    public void SaveCurrentScene2File(string path)
    {
        string json = CurrentScene2Json();
        System.IO.File.WriteAllText(path, json);
    }


    public static void own_GameObject2UnityGameObject(GameObjectInScene own_GO, GameObject GO)
    {
        GameObject parenty = GameObject.Find("Protagonists");
        GO.name = own_GO.name;
        GO.transform.localScale = own_GO.scale;
        GO.transform.position = own_GO.position;
        GO.transform.rotation = own_GO.rotation;
        GO.transform.parent = parenty.transform;
    }
    


    public void LoadObjectsIntoGame(string path)
    {
        Debug.Log("At least one worked out 0");
        //json = JsonUtility.ToJson(ser_able_obj_list, true);
        //string jsonString = JsonUtility.ToJson(gameObjectList_new.Listy[0], true);
        // Instantiate a Creature, assuming you have a `GenericCreature` prefab in your Resources folder

        /*
        GameObject prefab = (GameObject)Resources.Load("Prefabs/sheep");
        GameObject newObject = (GameObject)Instantiate(prefab);
        GameObjectInSceneMono newSheep = newObject.GetComponent<GameObjectInSceneMono>();
        // Assign the Archer JSON values to the Creature
        JsonUtility.FromJsonOverwrite(jsonString, newSheep);
        // All Objects
        */

        gameObjectList_new = GetObjectsFromJsonFile(path);
        foreach (GameObjectInScene gObject in gameObjectList_new.Listy)
        {

            

            json = JsonUtility.ToJson(gObject, true);
            //string prefab_path = "Prefabs/" + gObject.name;
            string prefab_path = "Prefabs/" + gObject.name;
            GameObject prefab = (GameObject)Resources.Load(prefab_path);
            Debug.Log("prefab_path: " + prefab_path);
            GameObject newObject1 = (GameObject)Instantiate(prefab);
            own_GameObject2UnityGameObject(gObject, newObject1);

            //GameObjectInSceneMono newSerObject = newObject1.GetComponent<GameObjectInSceneMono>();
            //JsonUtility.FromJsonOverwrite(json, newSerObject);
        }


    }


    GameObjectList GetObjectsFromJsonFile(string path)
    {
        GameObjectList gameObjectList_new;
        using (StreamReader r = new StreamReader(path))
        {
            string json_new = r.ReadToEnd();
            gameObjectList_new = JsonUtility.FromJson<GameObjectList>(json_new);
        }

        return gameObjectList_new;
    }


    static string CurrentScene2Json()
    {
        string json;
        List<GameObjectInScene> gameObjectList = new List<GameObjectInScene>();
        json = "hello";
        GameObject Protagonists = GameObject.Find("Protagonists");

        //Vector3 pos = GameObject.Find("ObjectX").transform.position;
        foreach (Transform gObject in Protagonists.transform)
        //foreach (Transform gObject in GameObject.Find("sheep") );
        //foreach (GameObject gObject in GameObject.FindObjectsOfType<GameObject>())
        {
            GameObjectInScene gameObjectInScene = new GameObjectInScene(gObject.name, gObject.transform.localScale, gObject.transform.position, gObject.transform.rotation);
            gameObjectList.Add(gameObjectInScene);


        }
        var ser_able_obj_list = new GameObjectList() { Listy = gameObjectList };
        json = JsonUtility.ToJson(ser_able_obj_list, true);
        // Last 4 lines active

        //json = JsonConvert.SerializeObject(aList);
        //Debug.Log("gameObjectList.Count: " + gameObjectList.Count);

        return json;

    }

}

[Serializable]
public class GameObjectInSceneMono : MonoBehaviour
{
    public string name;
    public Vector3 scale;
    public Vector3 position;
    public Quaternion rotation;


    public GameObjectInSceneMono(string name, Vector3 scale, Vector3 position, Quaternion rotation)
    {
        this.name = name;
        this.scale = scale;
        this.position = position;
        this.rotation = rotation;
    }




}


[Serializable]
public class GameObjectInScene
{
    public string name;
    public Vector3 scale;
    public Vector3 position;
    public Quaternion rotation;


    public GameObjectInScene(string name, Vector3 scale, Vector3 position, Quaternion rotation)
    {
        this.name = name;
        this.scale = scale;
        this.position = position;
        this.rotation = rotation;

    }






}











[Serializable]
public class GameObjectList
{
    public List<GameObjectInScene> Listy;
}











