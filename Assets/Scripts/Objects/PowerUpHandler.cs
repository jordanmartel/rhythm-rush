using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHandler : MonoBehaviour {

    //The number is the combo count needed for power excecution
    public enum powerUp : int { HitArea = 0, Pet = 50, Bomb = 40, AnyKey = 35, None=-1 };


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static bool checkAvailablePower(powerUp power, int combo) {
        return (int)power == combo;
    }

}
