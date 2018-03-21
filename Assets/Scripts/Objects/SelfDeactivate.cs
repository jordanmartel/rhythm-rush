using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour {

    public float t;
    private float sTime;

	// Use this for initialization
	void Start () {
        sTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - t >= sTime) {
            gameObject.SetActive(false);
        }
		
	}

    public void resetTimer() {
        sTime = Time.time;
    }
}
