using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


public class save_and_load_GOs : MonoBehaviour
{


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


    }



    ///////////////////////////////
    /// SAVE-SCENE METHODS     
    ///////////////////////////////
    public void SaveCurrentScene2File(string path)
    {
        string json = CurrentScene2Json();
        System.IO.File.WriteAllText(path, json);
    }

    static string CurrentScene2Json()
    {

        // Save current Gameobjects
        List<GameObjectInScene> gameObjectList = new List<GameObjectInScene>();
        GameObject Protagonists = GameObject.Find("Protagonists");
        foreach (Transform gObject in Protagonists.transform)
        {
            GameObjectInScene gameObjectInScene = new GameObjectInScene(gObject.name, gObject.transform.localScale, gObject.transform.position, gObject.transform.rotation);
            gameObjectList.Add(gameObjectInScene);

        }
        var ser_able_obj_list = new GameObjectList() { Listy = gameObjectList };
        string json = JsonUtility.ToJson(ser_able_obj_list, true);

        return json;

    }





    ///////////////////////////////
    /// LOAD-SCENE METHODS    
    ///////////////////////////////

    public void LoadObjectsIntoGame(string path)
    {
        gameObjectList_new = GetObjectsFromJsonFile(path);
        foreach (GameObjectInScene gObject in gameObjectList_new.Listy)
        {
            string prefab_path = "Prefabs/" + gObject.name;
            GameObject prefab = (GameObject)Resources.Load(prefab_path);
            GameObject newObject1 = (GameObject)Instantiate(prefab);
            own_GameObject2UnityGameObject(gObject, newObject1);
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


    public static void own_GameObject2UnityGameObject(GameObjectInScene own_GO, GameObject GO)
    {
        GameObject parenty = GameObject.Find("Protagonists");
        GO.name = own_GO.name;
        GO.transform.localScale = own_GO.scale;
        GO.transform.position = own_GO.position;
        GO.transform.rotation = own_GO.rotation;
        GO.transform.parent = parenty.transform;
    }


}





///////////////////////////////
/// OWN CLASSES     
///////////////////////////////

[Serializable]
public class GameObjectInScene
{
    public string name;
    public Vector3 scale;
    public Vector3 position;
    public Quaternion rotation;
    public int go_ID;

    // Extra components
    public string transition_style;
    public string transition_style2;

    public string animation_style;
    public string animation_style2;
    public string animation_style3;

    public string dummy_component1;
    public string dummy_component2;
    public int dummy_component3;
    public int dummy_component4;
    public double dummy_component5;
    public bool dummy_component6;


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











