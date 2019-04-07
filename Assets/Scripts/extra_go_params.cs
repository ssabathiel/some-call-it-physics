using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extra_go_params : MonoBehaviour {

    public int go_ID_;

    private int go_ID;
    [HideInInspector]
    public string Appear_Sound, Move_Sound, Disappear_Sound;
    [HideInInspector]
    public string MoveType, SpeedFct, RotationType;
    [HideInInspector]
    public float MoveTime, Speed, RotationTime, RotationSpeed; 

    public int LayerNum;
    

    public extra_go_params(int rand_id)
    {
        this.go_ID = rand_id;
    }





}


