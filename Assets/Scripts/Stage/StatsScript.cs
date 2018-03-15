using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour {

    [Header("P1")]
    public Text P1Score;
    public Text P1MaxCombo;
    public Text P1PerfectCount;

    [Header("P2")]
    public Text P2Score;
    public Text P2MaxCombo;
    public Text P2PerfectCount;


    [Header("Ratings")]
    public Sprite SS;
    public Sprite S;
    public Sprite A;
    public Sprite B;
    public Sprite C;

    [Header("Result")]
    public UnityEngine.UI.Image image;

    [Header("Stats")]
    private int P1maxCombo;
    private int P1failCount;
    private int P1perfectCount;
    private int P1stunCount;
    private int P1reviveCount;
    private int P1score;

    private int P2maxCombo;
    private int P2failCount;
    private int P2perfectCount;
    private int P2stunCount;
    private int P2reviveCount;
    private int P2score;

    int Joystick1;
    int Joystick2;
    public bool P1Proceed = false;
    public bool P2Proceed = false;

    string nextScene;

    // Use this for initialization
    void Start () {

        P1maxCombo = Metadata.P1maxCombo;
        P1failCount = Metadata.P1failCount;
        P1perfectCount = Metadata.P1perfectCount;
        P1stunCount = Metadata.P1stunCount;
        P1reviveCount = Metadata.P1reviveCount;
        P1score = Metadata.P1score;

        P2maxCombo = Metadata.P2maxCombo;
        P2failCount = Metadata.P2failCount;
        P2perfectCount = Metadata.P2perfectCount;
        P2stunCount = Metadata.P2stunCount;
        P2reviveCount = Metadata.P2reviveCount;
        P2score = Metadata.P2score;

        Joystick1 = Metadata.player1Joystick;
        Joystick2 = Metadata.player2Joystick;

        nextScene = Metadata.nextStage;

        setStatsOnScreen();

        image.sprite = calculateRating(Metadata.currentStage);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("joystick " + Joystick1 + " button 1")) {
            P1Proceed = true;
        }
        if (Input.GetKeyDown("joystick " + Joystick2 + " button 1")) { 
            P2Proceed = true;
        }

        if (P1Proceed && P2Proceed) {
            SceneManager.LoadScene(nextScene);
        }

	}


    private void setStatsOnScreen() {
        P1Score.text = P1score.ToString();
        P1MaxCombo.text = P1maxCombo.ToString();
        P1PerfectCount.text = P1perfectCount.ToString();

        P2Score.text = P2score.ToString();
        P2MaxCombo.text = P2maxCombo.ToString();
        P2PerfectCount.text = P2perfectCount.ToString();
    }

    private Sprite calculateRating(string lvlName) {
        int rating = (P1score * P1maxCombo) + (P2score * P2maxCombo); //-(heartsLost * 100)

        Debug.Log(lvlName);
        switch (lvlName) {
            case "GrassStage":
                if (rating >= 4000) {
                    return SS;
                } if (rating >= 3000) {
                    return S;
                } if (rating >= 2000) {
                    return A;
                }
                if (rating >= 1000) {
                    return B;
                }
                if (rating >= 500) {
                    return C;
                }
                break;

            case "WaterStage":
                if (rating >= 8000) {
                    return SS;
                }
                if (rating >= 5000) {
                    return S;
                }
                if (rating >= 3000) {
                    return A;
                }
                if (rating >= 2000) {
                    return B;
                }
                if (rating >= 1000) {
                    return C;
                }
                break;
            case "FireStage":
                if (rating >= 8000) {
                    return SS;
                }
                if (rating >= 5000) {
                    return S;
                }
                if (rating >= 3000) {
                    return A;
                }
                if (rating >= 2000) {
                    return B;
                }
                if (rating >= 1000) {
                    return C;
                }
                break;

            case "ElectricStage":
                if (rating >= 23000) {
                    return SS;
                }
                if (rating >= 15000) {
                    return S;
                }
                if (rating >= 10000) {
                    return A;
                }
                if (rating >= 5000) {
                    return B;
                }
                if (rating >= 2000) {
                    return C;
                }
                break;
            default:
                return A;
        }

        return A;

    }

}
