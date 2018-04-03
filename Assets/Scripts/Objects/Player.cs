using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [Header("Player Health")]
    public int health;
    public int maxHealth;

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
    public Text scoreText;

    [Header("Other")]
    public PlayerStats stats;
    public PowerUpHandler.powerUp powerUp;
    public SkillScript skillController;
    public string attackType;
    public int accumulatedDamage;
    public int combo;
    public int joystick;
    public AudioSource SFXPlayer;
    public AudioClip miss;
    public AudioClip chargeComplete;
    public Animator weaponAnimation;
    private bool buzzed;

    public float previousDpadHorizontal;
    public float previousDpadVertical;
    public string previousButton;

    public bool failedPhase = false;

    public Dictionary<string, string> notes;
    public List<GameObject> activeNotes;

    private int comboCount = 0;
    public int pUpCombo = 0;
    public bool pUpAvailable = false;
    private bool isDown = false;

    public Animator anim;
    public Canvas reviveIndicator;

    public void attackedByBoss()
    {
        health = health - 1;
        anim.SetTrigger("Damage");
    }
    public void recoverHealth()
    {
        health = Mathf.Min(maxHealth, health + 1);
    }

    public bool IsDown {
        get {
            return isDown;
        }

        set {
            isDown = value;
        }
    }

    public void KnockDownPlayer()
    {
        //gameObject.GetComponent<Animation>().Play("hurt_player");
        anim.SetTrigger("Stun");
        anim.SetBool("Stunned", true);
        IsDown = true;
        stats.incrementStun();
    }

    // Use this for initialization
    void Start()
    {
        pUpCombo = Metadata.playerManaLevels[joystick];
        anim = GetComponentInChildren<Animator>();
        buzzed = false;
        if (skillController.hitAreaUsed)
        {
            BoxCollider leftHitCollider = leftHitArea.gameObject.GetComponent<BoxCollider>();
            BoxCollider centreHitCollider = centreHitArea.gameObject.GetComponent<BoxCollider>();
            BoxCollider rightHitCollider = rightHitArea.gameObject.GetComponent<BoxCollider>();
            leftHitCollider.size = new Vector3((float) leftHitCollider.size.x * 1.5f,
                (float)leftHitCollider.size.y * 1.5f, (float)leftHitCollider.size.z * 1.5f);
            centreHitCollider.size = new Vector3((float)centreHitCollider.size.x * 1.5f,
                (float)centreHitCollider.size.y * 1.5f, (float)centreHitCollider.size.z * 1.5f);
            rightHitCollider.size = new Vector3((float)rightHitCollider.size.x * 1.5f,
                (float)rightHitCollider.size.y * 1.5f, (float)rightHitCollider.size.z * 1.5f);
        }
        pUpAvailable = PowerUpHandler.checkAvailablePower(powerUp, pUpCombo);
        if (pUpAvailable)
        {
            buzzed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        pUpAvailable = PowerUpHandler.checkAvailablePower(powerUp, pUpCombo);
        if (pUpAvailable)
        {
            if (skillController.bombUsed)
            {
                skillController.bombActive = true;
                skillController.activateBomb();
            }
        }
    }

    public void resetCombo() {
        combo = 0;
        pUpCombo = 0;

    }

    public void updateCombo() {
        combo++;
        if (!pUpAvailable)
        {
            pUpCombo++;
            pUpAvailable = PowerUpHandler.checkAvailablePower(powerUp, pUpCombo);
        }
        else if (!skillController.hitAreaUsed)
        {
            if (!buzzed)
            {
                SFXPlayer.clip = chargeComplete;
                SFXPlayer.Play();
                buzzed = true;
            }
            if (skillController.bombUsed)
            {
                skillController.bombActive = true;
            }
        }
    }
    
    public void triggerSkill()
    {
        skillController.triggerSkill(pUpAvailable);
        pUpAvailable = false;
        buzzed = false;
        pUpCombo = 0;
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
    public void updateComboCount (bool success, int score) {

        if (!success)
        {
            SFXPlayer.clip = miss;
            SFXPlayer.Play();
            skillController.bombActive = false;
        }
        comboCount = (success) ? comboCount+ 1 : 0;
        comboText.text = "x" + comboCount;
        scoreText.text = (System.Int32.Parse(scoreText.text) + score).ToString();
        stats.updateMaxCombo(comboCount);
        stats.updateScore(score);
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
