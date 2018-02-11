using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    public int hp = 100000;
    public int dmg = 0;


    public int endStatus = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Update visual cue on boss
		if (hp <= 0)
        {
            //Winning
            endStatus = 1;
        }
        
        // if player health <= 0, endStatus = -1
        // then if endStatus != 0, wait until the end of the current attack/damage animations are done
        // and show the endScene
	}
}
