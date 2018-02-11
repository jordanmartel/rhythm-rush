using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject track;
    public GameObject hitArea;
    public GameObject feedback;

    public int accumulatedDamage;
    public int combo;
    public int joystick;

    public float previousDpadHorizontal;
    public float previousDpadVertical;
    public string previousButton;

    public Dictionary<string, string> notes;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int calculateComboDamage(int comboThreshold)
    { 
        return accumulatedDamage + (((combo / comboThreshold) - 1) * accumulatedDamage);
    }
}
