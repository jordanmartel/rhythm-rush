using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFeedback : MonoBehaviour {

    //public Image image;
    private float displayTime;

    [Header("Distance Limit Values")]
    public float perfectValue = 0.5f;
    public float goodValue = 1.0f;
    public float okayValue = 1.5f;


    [Header("Score Values")]
    public int perfectScore = 50;
    public int goodScore = 30;
    public int okayScore = 10;

    /*
    [Header("Feedback Colours")]
    public Color perfectColour;
    public Color goodColour;
    public Color okayColour;
    public Color missColour;
    */

    public GameObject perfectSprite;
    public GameObject goodSprite;
    public GameObject okaySprite;
    public GameObject missSprite;

    // Use this for initialization
    void Start() {
        perfectValue = 0.5f;
        goodValue = 1.0f;
        okayValue = 1.5f;
    }

    // Update is called once per frame
    void Update() {

        if (Time.time - displayTime > 0.5f)
        {
            perfectSprite.SetActive(false);
            goodSprite.SetActive(false);
            okaySprite.SetActive(false);
            missSprite.SetActive(false);
        }
    }

    //Sets Feedback text based on distance from perfect hit point and returns score
    public int GiveFeedback(float distance, bool correct)
    {
        return giveTeamAttackFeedback(Mathf.Abs(distance), correct);
    }

    public int giveTeamAttackFeedback(float totalPress, bool correct) {

        perfectSprite.SetActive(false);
        goodSprite.SetActive(false);
        okaySprite.SetActive(false);
        missSprite.SetActive(false);


        int value = 0;
        if (!correct)
        {
            missSprite.SetActive(true);
            displayTime = Time.time;
            return 0;
        }
        if (totalPress <= perfectValue) {

            perfectSprite.SetActive(true);
            value =  perfectScore;
        }
        else if (totalPress <= goodValue) {

            goodSprite.SetActive(true);
            value = goodScore;
        }
        else if (totalPress <= okayValue) {

            okaySprite.SetActive(true);
            value = okayScore;
        }

        else
        {
            missSprite.SetActive(true);
        }

        displayTime = Time.time;
        return value;

       // StartCoroutine(waitforFeedback());
    }

    private IEnumerator waitforFeedback() {
        yield return new WaitForSeconds(5f);

        perfectSprite.SetActive(false);
        goodSprite.SetActive(false);
        okaySprite.SetActive(false);
        missSprite.SetActive(false);
    }
}
