using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFeedback : MonoBehaviour {

    public Text text;
    private float displayTime;

    [Header("Distance Limit Values")]
    public float perfectValue = 0.5f;
    public float goodValue = 1.0f;
    public float okayValue = 1.5f;


    [Header("Score Values")]
    public int perfectScore = 50;
    public int goodScore = 30;
    public int okayScore = 10;

    [Header("Feedback Colours")]
    public Color perfectColour;
    public Color goodColour;
    public Color okayColour;
    public Color missColour;

    // Use this for initialization
    void Start() {
        perfectValue = 0.5f;
        goodValue = 1.0f;
        okayValue = 1.5f;
    }

    // Update is called once per frame
    void Update() { 
		 if (Time.time - displayTime > 0.2f) {
            text.text = "";
            text.color = Color.white;
            
        }
	}

    //Sets Feedback text based on distance from perfect hit point and returns score
    public int GiveFeedback(float distance, bool correct)
    {
        return giveTeamAttackFeedback(Mathf.Abs(distance));
    }

    public int giveTeamAttackFeedback(float totalPress) {

        int value = 0;

        if (totalPress <= perfectValue) {
            text.text = "PERFECT!";
            text.color = perfectColour;
            value =  perfectScore;
        }
        else if (totalPress <= goodValue) {
            text.text = "GOOD!";
            text.color = goodColour;
            value = goodScore;
        }
        else if (totalPress <= okayValue) {
            text.text = "OKAY!";
            text.color = okayColour;
            value = okayScore;
        }

        else
        {
            text.text = "BAD";
            text.color = missColour;
        }

        displayTime = Time.time;
        return value;

       // StartCoroutine(waitforFeedback());
    }

    private IEnumerator waitforFeedback() {
        yield return new WaitForSeconds(3f);
        text.text = "";
        text.color = Color.white;
    }
}
