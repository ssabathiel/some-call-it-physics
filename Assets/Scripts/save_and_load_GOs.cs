﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.UI;
using System.Reflection;
using extra_go_params_namespace;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using OdinSerializer;


[RequireComponent(typeof(AudioSource))]
public class save_and_load_GOs : MonoBehaviour
{
    public string active_state = "";

    [SerializeField]
    public ParticleSystem compy;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextStateInGame();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevStateInGame();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            


            /*
            string json = JsonUtility.ToJson(compy, true);

            */


            byte[] json = SerializationUtility.SerializeValue(compy, DataFormat.JSON);
            string path = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States\test_object";
            System.IO.File.WriteAllText(path, BitConverter.ToString(json));
        }



    }

    void Awake()
    {

    }

    void Start()
    {

        /*
        GameObject Protagonists = GameObject.Find("Protagonists");
        print("Protagonists[0].name: " + Protagonists.transform.GetChild(0).name);
        //var info = typeof(ParticleSystem).GetProperties();
        //var info2 = Protagonists.transform.GetChild(0).GetComponent(typeof(info[0]));
        print("Start:");
        GameObject sel_state = this.gameObject;
        */



    }

    void OnGUI()
    {


        if (GUI.Button(new Rect(20, 20, 100, 100), "Play audio"))
        {
            //NextStateInGame();
            //PlayOwnAudio("blop");

        }
    }



    public void NextStateInGame()
    {

        GameObject go = GameObject.Find("@state_scripts");
        save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));
        string current_state = myscript.active_state;
        string next_state_name = nextStateString(current_state);
        string pathy = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States\" + next_state_name;
        Debug.Log("next_state_name 0 " + next_state_name);
        myscript.LoadObjectsIntoGame_dynamically(pathy);
        myscript.active_state = next_state_name;
       
    }



    public void PrevStateInGame()
    {

        GameObject go = GameObject.Find("@state_scripts");
        save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));
        string current_state = myscript.active_state;
        string next_state_name = prevStateString(current_state);
        string pathy = @"C:\Users\Silvester\Documents\SomeCallItPhysics_2D\Assets\Scripts\States\" + next_state_name;
        Debug.Log("next_state_name 0 " + next_state_name);
        myscript.LoadObjectsIntoGame_dynamically(pathy);
        myscript.active_state = next_state_name;

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


    public static string prevStateString(string stateName)
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


    ///////////////////////////////
    /// SAVE-SCENE METHODS     
    ///////////////////////////////
    public void SaveCurrentScene2File(string path)
    {
        string pathy = path + "_dir";
        Directory.CreateDirectory(pathy);

        string json = CurrentScene2Json();
        System.IO.File.WriteAllText(path, json);
        Debug.Log("RenderSettings.skybox " + RenderSettings.skybox.name);


    }

    static string CurrentScene2Json()
    {

        // Get current Gameobjects
        List<OwnGameObjectClass> gameObjectList = UnityGameObjectList2own_GameObjectList();
        // Get current Scene-settings
        GameObject cam = GameObject.Find("Main Camera");
        Material mat = RenderSettings.skybox;        
        SceneSettings sceneSettings_ = new SceneSettings(cam, mat);

        //Put SceneSettings and Gameobjects into ONE SERIALIZABLE OBJECT --> SceneData()
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

        DestroyAllGameobjects();

        SceneData sceneData = GetSceneDataFromJsonFile(path);
        own_GameObjectList2UnityGameObjectList(sceneData.GameObjectList);


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
            transferComponentsFromOwnGO2GO(gObject, newObject1);
            newObject1.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.initialize();
        }

    }




    public void LoadObjectsIntoGame_dynamically(string path)
    {

        SceneData sceneData = GetSceneDataFromJsonFile(path);
        //DestroyUnusedOldObjects(sceneData.GameObjectList);

        //own_GameObjectList2UnityGameObjectList(sceneData.GameObjectList);
        TransformAndCreate2UnityGameObjectList(sceneData.GameObjectList);
        

        //// Load SceneSettings: Camera and Skybox
        // Camera
        GameObject cam = GameObject.Find("Main Camera");
        cam.transform.position = sceneData.sceneSettings.camera_pos;
        cam.transform.eulerAngles = sceneData.sceneSettings.camera_rot;

    }



    public static void DestroyUnusedOldObjects(List<OwnGameObjectClass> newList)
    {

        // Destroy whole previous scene before
        // https://stackoverflow.com/questions/38120084/how-can-we-destroy-child-objects-in-edit-modeunity3d
        GameObject parenty = GameObject.Find("Protagonists");
        var tempArray = new GameObject[parenty.transform.childCount];

        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = parenty.transform.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            int go_ID = child.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.go_ID_;
            if (!IsGoID_InGoLIST(go_ID, newList))
            {
                DestroyImmediate(child);
            }
            
        }

    }


    public static bool IsGoID_InGoLIST(int goID, List<OwnGameObjectClass> gameObjectList)
    {
        bool IDinList = false;
        foreach (OwnGameObjectClass gObject in gameObjectList)
        {
            if (gObject.go_ID == goID)
            {
                IDinList = true;
            }
        }

        return IDinList;

    }





    public void DestroyAllGameobjects()
    {

        // Destroy whole previous scene before
        // https://stackoverflow.com/questions/38120084/how-can-we-destroy-child-objects-in-edit-modeunity3d
        GameObject parenty = GameObject.Find("Protagonists");
        var tempArray = new GameObject[parenty.transform.childCount];

        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = parenty.transform.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            DestroyImmediate(child);
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





    public static List<OwnGameObjectClass> UnityGameObjectList2own_GameObjectList()
    {

        // Get current Gameobjects
        List<OwnGameObjectClass> gameObjectList = new List<OwnGameObjectClass>();
        GameObject Protagonists = GameObject.Find("Protagonists");
        int lll = 0;
        System.Random rnd = new System.Random();

        foreach (Transform gObject in Protagonists.transform)
        {

            // Prepare extra_go_params
            //save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));
            extra_go_params go_params = (extra_go_params)gObject.GetComponent(typeof(extra_go_params)); //gObject.gameObject.GetComponent<extra_go_params>();
            
            List<own_component> components_ = GetComponentsAndProperties(gObject.gameObject);
            OwnGameObjectClass gameObjectInScene = new OwnGameObjectClass(gObject.name, gObject.transform.localScale, gObject.transform.position, gObject.transform.rotation, components_, go_params.extra_Go_Params_Serilizable);

            if (gObject.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.go_ID_ == 0)
            {
                int rand_ID = rnd.Next(1, 100000000);
                gObject.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.go_ID_ = rand_ID;
                
            }
            gameObjectInScene.go_ID = gObject.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.go_ID_;


            //gObject.gameObject.GetComponent<extra_go_params>().value = 2.0f;

            gameObjectList.Add(gameObjectInScene);

        }


        return gameObjectList;

    }



    public static void TransformAndCreate2UnityGameObjectList(List<OwnGameObjectClass> newObjectList)
    {
        //List<OwnGameObjectClass> Protagonists = UnityGameObjectList2own_GameObjectList();
        GameObject Protagonists = GameObject.Find("Protagonists");

        // Destroy Unused Objects
        DestroyUnusedOldObjects(newObjectList);

        int county = 0;
        foreach (OwnGameObjectClass newObject in newObjectList)
        {

            bool IDinList = false;
            
            foreach (Transform oldObject in Protagonists.transform)
            {
                if (newObject.go_ID == oldObject.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.go_ID_)
                {
                    IDinList = true;
                    // Transform
                    //oldObject.position = Vector3.Lerp(oldObject.position, newObject.position, (float)0.1 * Time.deltaTime);
                    //IEnumerator coroutine = WholeMoveObject(oldObject, newObject);


                    //IEnumerator coroutine = moveToX(oldObject, newObject.position, 1.0f);
                    GameObject go = GameObject.Find("@state_scripts");
                    //go_params.extra_Go_Params_Serilizable.initialize();
                    oldObject.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.initialize();
                    save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));
                    //myscript.StartCoroutine(myscript.moveToX(oldObject, newObject.position, 10.0f));
                    myscript.StartCoroutine(myscript.WholeMoveObject(oldObject, newObject));
                    PlayOwnAudio("swipe");
                    //myscript.WholeMoveObject(oldObject, newObject);

                    county++;
                    //MoveObject moveObject;
                    //StartCoroutine(MoveObject.use.Translation(oldObject, Vector3.up, 0.5f, MoveObject.MoveType.Time));

                    //StartCoroutine(moveToX(oldObject, newObject.position, 1.0f));

                    //Debug.Log("oldObject.position " + oldObject.position);
                    //Debug.Log("newObject.position " + newObject.position);

                    transferComponentsFromOwnGO2GO(newObject, oldObject.gameObject);

                }

            }

            
            if(IDinList == false)
            {

                string prefab_path = "Prefabs/" + newObject.name;
                GameObject prefab = (GameObject)Resources.Load(prefab_path);
                GameObject newObject1 = (GameObject)Instantiate(prefab);
                GameObject go = GameObject.Find("@state_scripts");
                save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));
                //go.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.initialize();
                //myscript.StartCoroutine(myscript.moveToX(oldObject, newObject.position, 10.0f));

                own_GameObject2UnityGameObject(newObject, newObject1);

                PlayOwnAudio("blop");
                myscript.StartCoroutine(myscript.BlobAppear(prefab));
                    
                
                

            }
            
            


            /*
            if(IsGoID_InGoLIST(gObject.go_ID, Protagonists) )
            {
                GoWithSameIDinList(int goID, List < OwnGameObjectClass > gameObjectList);
                // Transform to
            }
            else
            {
                string prefab_path = "Prefabs/" + gObject.name;
                GameObject prefab = (GameObject)Resources.Load(prefab_path);
                GameObject newObject1 = (GameObject)Instantiate(prefab);
                own_GameObject2UnityGameObject(gObject, newObject1);
            }
            */


        }

    }
    bool isMoving = false;

    IEnumerator moveToX(Transform fromPosition, Vector3 toPosition, float duration)
    {

        //Make sure there is only one instance of this function running
        if (isMoving)
        {
            yield break; ///exit if this is still running
        }
        isMoving = true;

        float counter = 0;

        //Get the current position of the object to be moved
        Vector3 startPos = fromPosition.position;

        while (counter < duration)
        {
            //Debug.Log("In move loop");
            counter += Time.deltaTime;
            fromPosition.position = Vector3.Lerp(startPos, toPosition, counter / duration);
            yield return null;
            
        }

        isMoving = false;
    }

    public static void own_GameObjectList2UnityGameObjectList(List<OwnGameObjectClass> gameObjectList)
    {
        foreach (OwnGameObjectClass gObject in gameObjectList)
        {
            string prefab_path = "Prefabs/" + gObject.name;
            GameObject prefab = (GameObject)Resources.Load(prefab_path);
            GameObject newObject1 = (GameObject)Instantiate(prefab);
            newObject1.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.initialize();

            own_GameObject2UnityGameObject(gObject, newObject1);

            transferComponentsFromOwnGO2GO(gObject, newObject1);
        }

    }




    public static void own_GameObject2UnityGameObject(OwnGameObjectClass own_GO, GameObject GO)
    {
        GameObject parenty = GameObject.Find("Protagonists");
        GO.name = own_GO.name;
        GO.transform.localScale = own_GO.scale;
        GO.transform.position = own_GO.position;
        GO.transform.rotation = own_GO.rotation;
        GO.transform.parent = parenty.transform;

        

        // ID
        GO.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.go_ID_ = own_GO.go_ID;

    }

    public static void transferComponentsFromOwnGO2GO(OwnGameObjectClass own_GO, GameObject GO)
    {
        /*
        string componentname = "UnityEngine.ParticleSystem";
        string whole_componentname = componentname + ", UnityEngine";
        Type typy1 = Type.GetType(whole_componentname, true);
        Debug.Log("typy1= " + typy1);
        GO.AddComponent(typy1);
        */

        // Delete all components that are not in new object
        foreach (var component in GO.GetComponents<Component>())
        {
            //Debug.Log("old component: " + component.GetType().ToString());
            bool component_is_there = false;
            foreach (own_component own_component in own_GO.components)
            {
                //Debug.Log("new component: " + own_component.name);
                
                if(component.GetType().ToString() == own_component.name)
                {
                    component_is_there = true;
                    //Debug.Log("Component before and after there: " + own_component.name);
                }                            
                
            }

            if ((component_is_there == false) && (component.name != "extra_go_params"))
            {
                //Debug.Log("destroyed component: " + component.GetType().ToString());
                Destroy(component);
            }

        }
            //GO.AddComponent<UnityEngine.ParticleSystem>();
        foreach (own_component component in own_GO.components)
        {


            //string whole_componentname = component.name + ", UnityEngine";
            //if ( (GO.GetComponent(compoonent.name) ?? null) && (component.name != "extra_go_params") )
            if ((component.name != "extra_go_params") )
            {
                
                //Debug.Log("CCCComponent not yet there" + component.name);
                string whole_componentname = component.name + ", UnityEngine";
                Type typy1 = Type.GetType(whole_componentname, true);
                //Debug.Log("typy1= " + typy1);
                if(GO.GetComponent(typy1) == null)
                {
                    GO.AddComponent(typy1);
                }


                //Debug.Log("component.name " + component.name);
                // change values of PROPERTIES according to the saved file
                //List<PropertyInfo> properties = component.GetType().GetProperties();
                //foreach (PropertyInfo property in GO.GetComponent(typy1).GetType().GetProperties())
                foreach (own_property property in component.properties)
                //foreach (var property in myComp.GetType().GetProperties())
                {
                    if(component.name == "UnityEngine.ParticleSystem")
                    {
                        Debug.Log("property: " + property.name);
                    }
                    PropertyInfo unity_property = GO.GetComponent(typy1).GetType().GetProperty(property.name);
                    object propertyValue = property.value;
                    //Debug.Log("property: " + property);
                    if (!unity_property.IsDefined(typeof(ObsoleteAttribute), true))
                    {
                        
                        //object propertyValue = property.GetValue(component, null);
                        //component.GetType().GetProperty(property) = propertyValue;
                        //propertyValue = 
                        
                        if (unity_property.CanWrite)
                        {


                            // if you want to serialize Vector3, go for: https://answers.unity.com/questions/1134997/string-to-vector3.html
                            bool isSerializable = IsSerializable_own(unity_property);
                            //if (CanChangeType(propertyValue, unity_property.PropertyType) )
                            //TypeConverter.CanConvertTo
                            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(propertyValue.GetType() );
                            if (converter.CanConvertTo(unity_property.GetType() ) )
                            {
                                //unity_property.SetValue(GO.GetComponent(typy1), Convert.ChangeType(propertyValue, unity_property.PropertyType), null);
                                unity_property.SetValue(GO.GetComponent(typy1), converter.ConvertTo(propertyValue, unity_property.PropertyType), null);
                            }
                        }
                        

                        //Debug.Log("Property Value: " + propertyValue);
                    }

                    //object theRealObject = property.GetValue(component);
                    //Debug.Log("")

                }
                



            }
            //object unity_component = GO.GetComponent(component.name);
        }
    }

    public static bool CanChangeType(object value, Type conversionType)
    {
        if (conversionType == null)
        {
            return false;
        }

        if (value == null)
        {
            return false;
        }

        IConvertible convertible = value as IConvertible;

        if (convertible == null)
        {
            return false;
        }

        return true;
    }

    public static bool IsSerializable_own(object obj)
    {
        Type t = obj.GetType();

        return Attribute.IsDefined(t, typeof(DataContractAttribute)) || t.IsSerializable || (obj is IXmlSerializable);

    }

    public static void PlayOwnAudio(string file)
    {
        GameObject parenty = GameObject.Find("Protagonists");
        GameObject go = GameObject.Find("@state_scripts");
        save_and_load_GOs myscript = (save_and_load_GOs)go.GetComponent(typeof(save_and_load_GOs));

        myscript.StartCoroutine(myscript.playMusic(parenty, file));

    }

    

    public IEnumerator playMusic(GameObject parenty, string file)
    {
        //AUDIO
        WWW www;
        AudioClip myAudioClip;
        string path;

        path = "file://" + Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/")) + "/Assets/Resources/Audio/" + file + ".wav";
        www = new WWW(path);
        yield return www;
        myAudioClip = www.GetAudioClip();

        
        //GameObject tmpGameObject = parenty;
        //parenty.AddComponent<AudioSource>();

        parenty.GetComponent<AudioSource>().clip = myAudioClip;
        parenty.GetComponent<AudioSource>().playOnAwake = true;
        parenty.GetComponent<AudioSource>().volume = 0.999f;
        parenty.GetComponent<AudioSource>().time = 0.0f;
        parenty.GetComponent<AudioSource>().priority = 0;

        //parenty.GetComponent<AudioSource>().Stop();
        parenty.GetComponent<AudioSource>().PlayOneShot(myAudioClip);
        //Destroy(parenty.GetComponent<AudioSource>());
        //parenty.GetComponent<AudioSource>().Play();
        
    }

    IEnumerator WholeMoveObject(Transform oldObject, OwnGameObjectClass newObject)
    {
        var startpos = oldObject.position;
        Vector3 endpos = newObject.position;

        Quaternion startrot = oldObject.rotation;
        Quaternion endrot = newObject.rotation;

        MoveObject moveObject = new MoveObject();
        //Debug.Log("Movetype " + oldObject.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.MoveType);
        while (true)
        {
            //yield return moveObject.Translation(oldObject, startpos, endpos, 0.5f, MoveObject.MoveType.Time);
            yield return moveObject.Translation(oldObject, startpos, endpos, endrot, oldObject.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.value, oldObject.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.MoveType, oldObject.gameObject.GetComponent<extra_go_params>().extra_Go_Params_Serilizable.SpeedFct);
            //yield return moveObject.Rotation(oldObject, startrot, endrot, oldObject.gameObject.GetComponent<extra_go_params>().RotationTime, oldObject.gameObject.GetComponent<extra_go_params>().RotationType, oldObject.gameObject.GetComponent<extra_go_params>().RotationSpeedFct);
            //yield return moveObject.Rotation_old(Transform thisTransform, Vector3 degrees, float time);

            //Rotation_old(Transform thisTransform, Vector3 degrees, float time)
            //yield return StartCoroutine(MoveObject(oldObject, startpos, endpos, 0.5f));

            //yield return StartCoroutine(MoveObject(oldObject, endpos, startpos, 3.0f));
            //yield return StartCoroutine(MoveObject(oldObject.transform, pointB, pointA, 3.0f));
            break;
        }


    }
        IEnumerator BlobAppear(GameObject go)
    {
        
        //float endscale_f = 20.01f;
        //Vector3 endscale = new Vector3(endscale_f, endscale_f, endscale_f);  //go.transform.localScale;
        Vector3 endscale = go.transform.localScale;
        float startscale_f = 0.1f;
        Vector3 startscale = new Vector3(startscale_f, startscale_f, startscale_f);        
        GameObject newObject1 = (GameObject)Instantiate(go);

        

        GameObject parenty = GameObject.Find("Protagonists");
        newObject1.transform.parent = parenty.transform;
        newObject1.transform.localScale = startscale;

        //GameObject parenty = GameObject.Find("Protagonists");
        //GameObject newObject1 = (GameObject)Instantiate(go, parenty.transform, true);

        //own_GameObject2UnityGameObject(newObject, newObject1);
        while (true)
        {
            yield return StartCoroutine(IncreaseSize(newObject1.transform, startscale, endscale, 0.3f));
            //yield return false;
            break;
        }
        
    }

    IEnumerator IncreaseSize(Transform thisTransform, Vector3 startScale, Vector3 endScale, float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        float growFactor = 1.1f;



        while (thisTransform.localScale.magnitude < endScale.magnitude )
        { 

            i += Time.deltaTime * rate;
            thisTransform.localScale += new Vector3(1, 1, 1) * i * growFactor;

            yield return null;
        }
    }


    IEnumerator LinearMoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    public class MoveObject : MonoBehaviour
    {
        // this class is from: http://wiki.unity3d.com/index.php/MoveObject
        public enum MoveType { Time, Speed };
        public static MoveObject use = null;

        void Awake()
        {
            if (use)
            {
                Debug.LogWarning("Only one instance of the MoveObject script in a scene is allowed");
                return;
            }
            use = this;
        }

        public IEnumerator TranslateTo(Transform thisTransform, Vector3 endPos, float value, string moveType)
        {
            //yield return Translation(thisTransform, thisTransform.position, endPos, value, moveType);
            yield return 0;
        }

        public IEnumerator Translation(Transform thisTransform, Vector3 endPos, float value, string moveType)
        {
            //yield return Translation(thisTransform, thisTransform.position, thisTransform.position + endPos, value, moveType);
            yield return 0;
        }


        public IEnumerator Translation(Transform thisTransform, Vector3 startPos, Vector3 endPos, Quaternion endRotation, float value, string moveType, string SpeedFct = "linear")
        {
            // float rate = ... was here before, but want it to make time dependend. 
            Quaternion startRotation = thisTransform.rotation;
            float t = 0.0f;
            while (t < 1.0)
            {
                if (moveType == "Speed")
                {
                    switch (SpeedFct)
                    {
                        case "linear":
                            value = value;
                            break;
                        case "easInOutSine":
                            value = value*(1.0f + (float)Math.Sin((float)Math.PI * t - (float)Math.PI / 2.0f)) / 2.0f;
                            break;
                            //
                    }
                }
                float rate = (moveType == "Time") ? 1.0f / value : 1.0f / Vector3.Distance(startPos, endPos) * value;
                t += Time.deltaTime * rate;
                //Debug.Log("directly before - Vector3.Lerp " + Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, t)));
                //Debug.Log("at t " + t);
                //Debug.Log("Time.deltaTime " + Time.deltaTime);
                //Debug.Log("rate " + rate);
                //Debug.Log("value " + value);
                thisTransform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, t) );
                thisTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                yield return null;
            }
        }


        public IEnumerator Rotation(Transform thisTransform, Quaternion startRotation, Quaternion endRotation, float value, string moveType, string SpeedFct = "linear")
        {
            startRotation = thisTransform.rotation;
            //Quaternion endRotation = thisTransform.rotation * Quaternion.Euler(degrees);
            float t = 0.0f;
            while (t < 1.0)
            {
                if (moveType == "Speed")
                {
                    switch (SpeedFct)
                    {
                        case "linear":
                            value = value;
                            break;
                        case "easInOutSine":
                            value = (1.0f + (float)Math.Sin((float)Math.PI * t - (float)Math.PI / 2.0f)) / 2.0f;
                            break;
                            //
                    }
                }
                float rate = (moveType == "Time") ? 1.0f / value : 1.0f / Quaternion.Angle(startRotation, endRotation) * value;
                t += Time.deltaTime * 0.3f; //rate;
                //thisTransform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, t));
                thisTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                yield return null;
            }
        }



        public IEnumerator Rotation_old(Transform thisTransform, Vector3 degrees, float time)
        {
            Quaternion startRotation = thisTransform.rotation;
            Quaternion endRotation = thisTransform.rotation * Quaternion.Euler(degrees);
            float rate = 1.0f / time;
            float t = 0.0f;
            while (t < 1.0f)
            {
                t += Time.deltaTime * rate;
                thisTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                yield return null;
            }
        }
    }

    public static List<own_component> GetComponentsAndProperties(GameObject go_)
    {

        List<own_component> components_ = new List<own_component>();

        foreach (Component component in go_.GetComponents<Component>())
        {

            List<own_property> properties_ = new List<own_property>();

            //print("Component: " + component);
            Type typy = component.GetType();
            object compy = go_.GetComponent(typy);
            object compy2 = go_.GetComponent("transform");

            /*
            if(component.name == "UnityEngine.ParticleSystem")
            {
                SerializedObject m_Object = new SerializedObject(component);
                Debug.Log("--------" + component.GetType() + "-------");
                try
                {
                    SerializedProperty obj = m_Object.GetIterator();


                    foreach (SerializedProperty property in obj)
                    {
                        Debug.Log(property.name + " : " + property.propertyType);
                    }
                }
                catch (System.Exception e)
                {

                }

            }
            */


            //var info = typeof(component.GetType().GetFields()).GetProperties();
            //Type typy = component.GetType();
            //var info = typeof(component).GetProperties();

            //Debug.Log(component.GetType());
            //print("Particle System Props: " + info[0]);

            /*
            foreach (FieldInfo fi in component.GetType().GetFields())
            {
                System.Object obj = (System.Object)component;
                Debug.Log("fi name " + fi.Name + " val " + fi.GetValue(obj));
            }
            */

                //List<PropertyInfo> properties = component.GetType().GetProperties();
                //foreach (PropertyInfo property in component.GetType().GetProperties())
             foreach (var property in component.GetType().GetFields())
            {
                Debug.Log("property: " + property);
                if (component.name == "UnityEngine.ParticleSystem")
                {
                    Debug.Log("property: " + property);
                }
                if (!property.IsDefined(typeof(ObsoleteAttribute), true))
                {

                    if(property.Name != null)
                    {   
                        /*
                        object propertyValue = property.GetValue(component, null);
                        //component.GetType().GetProperty(property) = propertyValue;
                        if (property.CanWrite && property.Name != null && propertyValue != null && property.CanRead)
                        {

                            property.SetValue(compy, propertyValue);
                            own_property property_ = new own_property(property.Name.ToString(), propertyValue.ToString());
                            properties_.Add(property_);
                        }
                        */

                    }


                    
                    //Debug.Log("Property Value: " + propertyValue);
                    //own_property property_ = new own_property();
                }

                //object theRealObject = property.GetValue(component);
                //Debug.Log("")
            }
            own_component component_ = new own_component(component.GetType().ToString(), properties_);
            components_.Add(component_);

        }

        return components_;
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

    public List<own_component> components;
    public extra_go_params_serializable go_params;

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

    // extra_go_params
    public string Appear_Sound, Move_Sound, Disappear_Sound;
    public string SpeedFct, RotationType, RotationSpeedFct;
    public float MoveTime, Speed, RotationTime, RotationSpeed;

    public int LayerNum;
    public bool Sync;


    public OwnGameObjectClass(string name, Vector3 scale, Vector3 position, Quaternion rotation, List<own_component> components_, extra_go_params_serializable go_params_)
    {
        this.name = name;
        this.scale = scale;
        this.position = position;
        this.rotation = rotation;
        this.components = components_;        
        this.go_params = go_params_;

    }


}

/*
[Serializable]
public static class extra_go_params_static
{
    public int extra_go_params
}
*/

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

[Serializable]
public class own_property
{
    public string name;
    public string value;

    public own_property(string name_, string value_)
    {
        this.name = name_;
        this.value = value_;
    }
}



[Serializable]
public class own_component
{
    public string name;
    public List<own_property> properties;

    public own_component(string name_, List<own_property> properties_)
    {
        this.name = name_;
        this.properties = properties_;
    }


}


















