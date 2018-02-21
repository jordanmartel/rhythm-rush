using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject leftTrack;
    public GameObject centreTrack;
    public GameObject rightTrack;
    public GameObject hitArea;
    public GameObject leftMissBox;
    public GameObject centreMissBox;
    public GameObject rightMissBox;
    public GameObject feedback;

    public int accumulatedDamage;
    public int combo;
    public int joystick;

    public float previousDpadHorizontal;
    public float previousDpadVertical;
    public string previousButton;

    public bool failedPhase = false;

    public Dictionary<string, string> notes;
    public List<GameObject> activeNotes;

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



}
