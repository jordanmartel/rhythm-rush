using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [Header("Music Staff References")]
    public GameObject leftTrack;
    public GameObject centreTrack;
    public GameObject rightTrack;
    public GameObject leftMissBox;
    public GameObject centreMissBox;
    public GameObject rightMissBox;
    public GameObject feedback;
    public GameObject leftHitArea;
    public GameObject centreHitArea;
    public GameObject rightHitArea;


    [Header("UI Elements")]
    public Text comboText;

    [Header("Other")]
    public int accumulatedDamage;
    public int combo;
    public int joystick;

    public float previousDpadHorizontal;
    public float previousDpadVertical;
    public string previousButton;

    public bool failedPhase = false;

    public Dictionary<string, string> notes;
    public List<GameObject> activeNotes;

    private int comboCount = 0;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int calculateComboDamage(int comboThreshold)
    {
        return accumulatedDamage + (((combo / comboThreshold) - 1) * accumulatedDamage);
    }

    public GameObject getNoteDestination(string key) {

        if ("leftsquare".Contains(key)){
            return leftMissBox;

        } else if ("rightcircle".Contains(key)) {
            return rightMissBox;
        } else {
            return centreMissBox;
        }
    }

    public Vector3 getNoteStart(string key) {

        if ("leftsquare".Contains(key)) {
            return leftTrack.transform.position;

        }
        else if ("rightcircle".Contains(key)) {
            return rightTrack.transform.position;
        }
        else {
            return centreTrack.transform.position;
        }
    }

    //Success indicates correct hit, if false then reset combo counter
    public void updateComboCount (bool success) {

        comboCount = (success) ? comboCount+ 1 : 0;
        comboText.text = "x" + comboCount;
    }

    public GameObject getHitArea(string key) {

        if ("leftsquare".Contains(key)) {
            return leftHitArea;

        }
        else if ("rightcircle".Contains(key)) {
            return rightHitArea;
        }
        else {
            return centreHitArea;
        }
    }
}
