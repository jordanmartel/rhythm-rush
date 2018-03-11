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
    public PlayerStats stats;
    public PowerUpHandler.powerUp powerUp;
    public SkillScript skillController;
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
    private int pUpCombo = 0;
    private bool pUpAvailable = false;
    private bool isDown = false;


    public bool IsDown {
        get {
            return isDown;
        }

        set {
            isDown = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (skillController.hitAreaUsed)
        {
            leftHitArea.transform.localScale = new Vector3((float) leftHitArea.transform.localScale.x * 1.25f,
                (float) leftHitArea.transform.localScale.y * 1.25f, (float) leftHitArea.transform.localScale.z * 1.25f);
            centreHitArea.transform.localScale = new Vector3((float)centreHitArea.transform.localScale.x * 1.25f,
                (float)centreHitArea.transform.localScale.y * 1.25f, (float)centreHitArea.transform.localScale.z * 1.25f);
            rightHitArea.transform.localScale = new Vector3((float)rightHitArea.transform.localScale.x * 1.25f,
                (float)rightHitArea.transform.localScale.y * 1.25f, (float)rightHitArea.transform.localScale.z * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetCombo() {
        combo = 0;
        pUpCombo = 0;

    }

    public void updateCombo() {
        combo++;
        if (!pUpAvailable)  pUpCombo++;
        pUpAvailable = PowerUpHandler.checkAvailablePower(powerUp, pUpCombo);
        skillController.triggerSkill(pUpAvailable);
        if (pUpAvailable)
        {
            pUpCombo = 0;
        }
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
        stats.updateMaxCombo(comboCount);
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
