﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFeedback : MonoBehaviour {

    public Text text;
    private float displayTime;

    [Header("Score Limit Values")]
    public float perfectValue;
    public float goodValue;
    public float okayValue;

    [Header("Feedback Colours")]
    public Color perfectColour;
    public Color goodColour;
    public Color okayColour;
    public Color missColour;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

      //Reset Text to not Visible
        if (Time.time - displayTime > 0.5f) {
            text.text = "";
            text.color = Color.white;
        }
		 
	}

    //Sets Feedback text based on distance from perfect hit point and returns score
    public float GiveFeedback(float distance, bool correct)
    {

        float score = 0f;
        if (!correct) {
            text.text = "BAD!";
            text.color = missColour;
        }
        else {

            distance = Mathf.Abs(distance);
            Debug.Log(distance);
            if (distance <= 0.25f) {
                text.text = "SSS";
                text.color = perfectColour;

            }
            else if (distance <= perfectValue) {
                text.text = "PERFECT!";
                text.color = perfectColour;

            }
            else if (distance < goodValue) {
                text.text = "GOOD!";
                text.color = goodColour;

            }
            else if (distance < okayValue) {
                text.text = "OKAY!";
                text.color = okayColour;

            }
            else if (distance > okayValue && distance < 5) {
                text.text = "BAD!";
                text.color = missColour;

            }
            else {
                text.text = "MISS";
                text.color = missColour;
            }
        }
        displayTime = Time.time;


        return score;
    }

}