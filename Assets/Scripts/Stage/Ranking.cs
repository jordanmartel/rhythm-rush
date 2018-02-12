using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour {
    public double SS, S, A, B, C, D;
    public double maxTime;
    public Slider SS_img, S_img, A_img, B_img, C_img, D_img;
    public double time;
    public Slider timeline;
	// Use this for initialization
	void Start () {
        SS = GameObject.FindObjectOfType<StageScript>().beatmap.thresholds[0];
        S = GameObject.FindObjectOfType<StageScript>().beatmap.thresholds[1];
        A = GameObject.FindObjectOfType<StageScript>().beatmap.thresholds[2];
        B = GameObject.FindObjectOfType<StageScript>().beatmap.thresholds[3];
        C = GameObject.FindObjectOfType<StageScript>().beatmap.thresholds[4];
        D = GameObject.FindObjectOfType<StageScript>().beatmap.thresholds[5];
        maxTime = D;
        SS_img.value = (float)(1.0 - SS / maxTime);
        S_img.value = (float)(1.0 - S / maxTime);
        A_img.value = (float)(1.0 - A / maxTime);
        B_img.value = (float)(1.0 - B / maxTime);
        C_img.value = (float)(1.0 - C / maxTime);
        D_img.value = (float)(1.0 - D / maxTime);
    }
	
	// Update is called once per frame
	void Update () {
        time = Time.time;
        timeline.value = (float) (1.0 - time / maxTime);
	}

    string rankingAtTime(double atTime)
    {
        if (atTime <= SS)
        {
            return "SS";
        }
        else if (atTime <= S) {
            return "S";
        }
        else if (atTime <= A)
        {
            return "A";
        }
        else if (atTime <= B)
        {
            return "B";
        }
        else if (atTime <= C)
        {
            return "C";
        }
        else
        {
            return "D";
        }
    }
}
