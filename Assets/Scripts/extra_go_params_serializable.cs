using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace extra_go_params_namespace
{

    [Serializable]
    public class extra_go_params_serializable // : MonoBehaviour 
    {

        public int go_ID_;
        //
        private int go_ID;
        [HideInInspector]
        public string Appear_Sound, Move_Sound, Disappear_Sound;
        [HideInInspector]
        public string MoveType, SpeedFct, RotationType, RotationSpeedFct;

        //GameObject go = GameObject.Find("@state_scripts");
        public save_and_load_GOs.MoveObject.MoveType moveType;

        [HideInInspector]
        public float RotationTime, RotationSpeed;

        public float value = 8.0f;
        public float MoveTime = 2.0f;
        public float Speed = 2.0f;

        public float testy2 = 8.0f;

        public int LayerNum;
        [HideInInspector]
        public bool Sync;

        public void initialize()
        {
            value = 30.0f;
            MoveTime = 2.0f;
            Speed = 2.0f;
        }

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

        /*
        public extra_go_params(int rand_id)
        {
            this.go_ID = rand_id;
            this.value = 2.0f;
        }
        */





    }
}



