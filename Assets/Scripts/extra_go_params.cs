using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extra_go_params : MonoBehaviour 
{

    public int go_ID_;
    //
    private int go_ID;
    [HideInInspector]
    public string Appear_Sound, Move_Sound, Disappear_Sound;
    [HideInInspector]
    public string MoveType, SpeedFct, RotationType, RotationSpeedFct;

    GameObject go = GameObject.Find("@state_scripts");
    public save_and_load_GOs.MoveObject.MoveType moveType;

    //
    public float MoveTime, Speed, RotationTime, RotationSpeed, value; 

    public int LayerNum;
    [HideInInspector]
    public bool Sync;

    

    /*
    void Start()
    {
        
        if(MoveType == "Speed")
        {
            save_and_load_GOs.MoveObject.MoveType moveType = save_and_load_GOs.MoveObject.MoveType.Speed;
        }
        if (MoveType == "Time")
        {
            save_and_load_GOs.MoveObject.MoveType moveType = save_and_load_GOs.MoveObject.MoveType.Time;
        }

    }
    */

    //public myscript.MoveObject moveObject;

    //MoveObject moveObject = (MoveObject)go.GetComponent(typeof(MoveObject));
    // hey
    //public MoveObject moveObject;


    public extra_go_params(int rand_id)
    {
        this.go_ID = rand_id;
    }





}


