using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFeedback : MonoBehaviour {

    public Text text;
    private float displayTime;

    [Header("Score Limit Values")]
    public int perfectValue;
    public int goodValue;
    public int okayValue;

    [Header("Feedback Colours")]
    public Color perfectColour;
    public Color goodColour;
    public Color okayColour;
    public Color missColour;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() { 
		 
	}

    //Sets Feedback text based on distance from perfect hit point and returns score
    public int GiveFeedback(float distance, bool correct)
    {
        return (!correct || distance > 2) ? 0 : 1;
    }

    public void giveTeamAttackFeedback(int totalPress) {
        if (totalPress >= perfectValue) {
            text.text = "PERFECT!";
            text.color = perfectColour;
        }
        else if (totalPress >= goodValue) {
            text.text = "GOOD!";
            text.color = goodColour;
        }
        else if (totalPress >= okayValue) {
            text.text = "OKAY!";
            text.color = okayColour;
        }

        else
        {
            text.text = "BAD";
            text.color = missColour;

        }

        StartCoroutine(waitforFeedback());
    }

    private IEnumerator waitforFeedback() {
        yield return new WaitForSeconds(3f);
        text.text = "";
        text.color = Color.white;
    }
}
