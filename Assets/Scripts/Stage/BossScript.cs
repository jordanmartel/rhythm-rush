using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    public int hp = 100000;
    public int dmg = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Update visual cue on boss
		if (dmg >= hp)
        {
            //Winning scene
        }
	}
}
