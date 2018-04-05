using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamAttack : MonoBehaviour {

    [Header("Boss Stats")]
    public bool isActive;
    public int numberOfHits;
    public int maximumNumberOfHits = 60;
    public int attackHits = 50;
    public int recoverHits = 60;
    public int damagePerHit = 20;
    public GameObject boss;

    public float allotedTime = 5f;
    private float remainingTime = 0f;
    private Vector3 startSize;

    [Header("UI Elements")]
    public Slider energyBar;
    public GameObject textPrompts;
    public PlayerFeedback player1Feedback;
    public PlayerFeedback player2Feedback;
    public Sprite countDown5;
    public Sprite countDown4;
    public Sprite countDown3;
    public Sprite countDown2;
    public Sprite countDown1;
    public Image countDownNumber;
    public Canvas countDownCanvas;

    [Header("AttackChildren")]
    private GameObject mainAttractor;
    private GameObject sideAttractor1;
    private GameObject sideAttractor2;
    private GameObject fireball;
    private Vector3 initialTransform;

    [Header("Others")]
    public AudioClip buildTeamAttackSFX;

    private bool isAttackPhase = false;

	// Use this for initialization
	void Start () {
        energyBar.maxValue = maximumNumberOfHits;
        energyBar.gameObject.SetActive(false);
        startSize = transform.localScale;
        initialTransform = transform.position;

        //Child Indexes {0 = particle revolver, 1 = attractor 1, 2 = attractor 2, 3 = fireball}
        mainAttractor = transform.GetChild(0).gameObject;
        sideAttractor1 = transform.GetChild(1).gameObject;
        sideAttractor2 = transform.GetChild(2).gameObject;
        damagePerHit = 20;
        GetComponent<AudioSource>().clip = buildTeamAttackSFX;
    }

    public void startTeamAttack()
    {
        isActive = true;
        GetComponent<MeshRenderer>().enabled = true;

        mainAttractor.SetActive(true);
        sideAttractor1.SetActive(true);
        sideAttractor2.SetActive(true);

        textPrompts.SetActive(true);
        textPrompts.GetComponent<Animation>().Play();

        //energyBar.gameObject.SetActive(true);
        //GetComponentInChildren<Text>().text = "Mash the buttons!";
        remainingTime = allotedTime;
    }

    public void buildTeamAttack()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 0.5f + (float) numberOfHits * 0.1f;
        print("Pitch");
        print(audioSource.pitch);
        audioSource.Play();
        if (numberOfHits < maximumNumberOfHits) numberOfHits++;   
    }

    public int unleashTeamAttack()
    {

        if (numberOfHits > maximumNumberOfHits)
        {
            numberOfHits = maximumNumberOfHits;
        }

        int damageDone = numberOfHits * damagePerHit;
        if (numberOfHits >= recoverHits)
        {
            FindObjectOfType<Team>().recoverHealth();
        }
    
        energyBar.gameObject.SetActive(false);
        isActive = false;
        numberOfHits = 0;

        DeactivateStuff();
        isAttackPhase = true;

        return damageDone;
    }

    public bool timerExpired()
    {
        return remainingTime <= 0;
    }

    private void DeactivateStuff () {
        mainAttractor.SetActive(false);
        sideAttractor1.SetActive(false);
        sideAttractor2.SetActive(false);
        textPrompts.SetActive(false);
    }

	// Update is called once per frame
	void Update () {

        if (isActive) {
            remainingTime = remainingTime - Time.deltaTime;
            countDownCanvas.gameObject.SetActive(true);
            if (remainingTime > 4f)
            {
                countDownNumber.sprite = countDown5;
                
            }
            else if (remainingTime > 3f)
            {
                countDownNumber.sprite = countDown4;
            }
            else if (remainingTime > 2f)
            {
                countDownNumber.sprite = countDown3;
            }
            else if (remainingTime > 1f)
            {
                countDownNumber.sprite = countDown2;
            }
            else
            {
                countDownNumber.sprite = countDown1;
            }

            if (numberOfHits > 0) {
                transform.localScale = new Vector3(startSize.x + numberOfHits / 5.0f, startSize.y + numberOfHits / 5.0f, startSize.z + numberOfHits / 5.0f);
                //GetComponentInChildren<Text>().text = numberOfHits.ToString();
            }
            else
            {
                transform.localScale = new Vector3(startSize.x, startSize.y, startSize.z);
            }
        }
        if (isAttackPhase) {
            transform.position = Vector3.MoveTowards(transform.position, boss.transform.position, 0.5f /*Speed*/);
        }
        //energyBar.value = Mathf.Min(numberOfHits, maximumNumberOfHits);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag.Contains("boss")){
            GetComponent<MeshRenderer>().enabled = false;
            isAttackPhase = false;
            transform.localScale = startSize;
            transform.position = initialTransform;
        }
    }
}
