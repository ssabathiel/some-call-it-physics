using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.UI;

public class save_and_load_GOs : MonoBehaviour
{

    void Awake()
    {
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
        Debug.Log("RenderSettings.skybox " + RenderSettings.skybox.name);
    }

    static string CurrentScene2Json()
    {

        // Get current Gameobjects
        List<OwnGameObjectClass> gameObjectList = new List<OwnGameObjectClass>();
        GameObject Protagonists = GameObject.Find("Protagonists");
        int lll = 0;
        System.Random rnd = new System.Random();

        foreach (Transform gObject in Protagonists.transform)
        {
            
            OwnGameObjectClass gameObjectInScene = new OwnGameObjectClass(gObject.name, gObject.transform.localScale, gObject.transform.position, gObject.transform.rotation);

            /*
            lll++;           
            int rand_ID = rnd.Next(1, 100000000);
            //InputField inputfield = gObject.gameObject.AddComponent<InputField>();
            InputField inputfield = gObject.gameObject.GetComponent<InputField>();
            inputfield.text = rand_ID.ToString();
            //inputfield.SetActive(false);
            */

            /*
            extra_go_params id_script = gObject.gameObject.AddComponent<extra_go_params>();
            System.Random rnd = new System.Random();
            int rand_ID = rnd.Next(1, 100000000);
            id_script.go_ID = rand_ID;
            gameObjectInScene.go_ID = id_script.go_ID;
            */

            //System.Random rnd = new System.Random();
            if (gObject.gameObject.GetComponent<extra_go_params>().go_ID_ == 0 )
            {                
                int rand_ID = rnd.Next(1, 100000000);
                //int go_ID = gObject.gameObject.AddComponent<int>();
                //gObject.gameObject.go_ID = rand_ID;
                gObject.gameObject.GetComponent<extra_go_params>().go_ID_ = rand_ID;
            }
            //Debug("gObject.go_ID2: " + gObject.gameObject.GetComponent<extra_go_params>().go_ID);
            gameObjectInScene.go_ID = gObject.gameObject.GetComponent<extra_go_params>().go_ID_;
            


            gameObjectList.Add(gameObjectInScene);

        }
        // Get current Scene-settings
        GameObject cam = GameObject.Find("Main Camera");
        Material mat = RenderSettings.skybox;        
        SceneSettings sceneSettings_ = new SceneSettings(cam, mat);

        var ser_able_obj_list = new SceneData() { GameObjectList = gameObjectList, sceneSettings = sceneSettings_ };
        string json = JsonUtility.ToJson(ser_able_obj_list, true);

        return json;

    }





    ///////////////////////////////
    /// LOAD-SCENE METHODS    
    ///////////////////////////////
    ///

    public void LoadSceneIntoGame(string path)
    {
        SceneData sceneData = GetSceneDataFromJsonFile(path);
        foreach (OwnGameObjectClass gObject in sceneData.GameObjectList)
        {
            string prefab_path = "Prefabs/" + gObject.name;
            GameObject prefab = (GameObject)Resources.Load(prefab_path);
            GameObject newObject1 = (GameObject)Instantiate(prefab);
            own_GameObject2UnityGameObject(gObject, newObject1);
        }

        //// Load SceneSettings: Camera and Skybox
        // Camera
        GameObject cam = GameObject.Find("Main Camera");
        cam.transform.position = sceneData.sceneSettings.camera_pos;
        cam.transform.eulerAngles = sceneData.sceneSettings.camera_rot;
        //Skybox
        //RenderSettings.skybox = ...path + sceneData.sceneSettings.skybox_name... ;
        //RenderSettings.skybox.SetMatrix("_Rotation", sceneData.sceneSettings.skybox_rot) ;



    }

    public void LoadObjectsIntoGame(string path)
    {
        SceneData sceneData = GetSceneDataFromJsonFile(path);
        foreach (OwnGameObjectClass gObject in sceneData.GameObjectList)
        {
            string prefab_path = "Prefabs/" + gObject.name;
            GameObject prefab = (GameObject)Resources.Load(prefab_path);
            GameObject newObject1 = (GameObject)Instantiate(prefab);
            own_GameObject2UnityGameObject(gObject, newObject1);
        }

    }


    public void LoadObjectsIntoGame_dynamically(string path)
    {
        SceneData sceneData = GetSceneDataFromJsonFile(path);
        foreach (OwnGameObjectClass gObject in sceneData.GameObjectList)
        {
            string prefab_path = "Prefabs/" + gObject.name;
            GameObject prefab = (GameObject)Resources.Load(prefab_path);
            GameObject newObject1 = (GameObject)Instantiate(prefab);
            own_GameObject2UnityGameObject(gObject, newObject1);
        }

    }


    SceneData GetSceneDataFromJsonFile(string path)
    {
        SceneData sceneData;
        using (StreamReader r = new StreamReader(path))
        {
            string json_new = r.ReadToEnd();
            sceneData = JsonUtility.FromJson<SceneData>(json_new);
        }

        return sceneData;
    }


    public static void own_GameObject2UnityGameObject(OwnGameObjectClass own_GO, GameObject GO)
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
///


[Serializable]
public class SceneData
{
    public List<OwnGameObjectClass> GameObjectList;
    public SceneSettings sceneSettings;
}



[Serializable]
public class OwnGameObjectClass
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


    public OwnGameObjectClass(string name, Vector3 scale, Vector3 position, Quaternion rotation)
    {
        this.name = name;
        this.scale = scale;
        this.position = position;
        this.rotation = rotation;

    }


}




[Serializable]
public class SceneSettings
{
    public Vector3 camera_pos;
    public Vector3 camera_rot;
    public float camera_fieldOfView;

    public string skybox_name;
    public Matrix4x4 sky_box_rot;

    public Light light;

    public string dummy_comp1;
    public string dummy_comp2;
    public string dummy_comp3;

    public int dummy_comp4;
    public int dummy_comp5;
    public int dummy_comp6;

    public float dummy_comp7;
    public float dummy_comp8;
    public float dummy_comp9;

    public SceneSettings(GameObject camera, Material skybox)
    {
        this.camera_pos = camera.transform.position;
        this.camera_rot = camera.transform.eulerAngles;
        //this.camera_fieldOfView = camera.camera.fieldOfView;

        //this.skybox_name = skybox.name;
        //this.sky_box_rot = skybox.GetMatrix("_Rotation");
    }


}








