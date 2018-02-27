using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour {
    double startTimer = 0;
    double startTime = 0;
    bool started = false;

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!started)
        {
            startTimer += Time.deltaTime;
            if (startTimer >= startTime)
            {
                GetComponent<AudioSource>().enabled = true;
            }
        }
	}
}
