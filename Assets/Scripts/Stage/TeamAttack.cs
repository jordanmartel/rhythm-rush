using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamAttack : MonoBehaviour {

    public bool isActive;
    public int numberOfHits;
    public int maximumNumberOfHits = 60;
    public int attackHits = 50;
    public int recoverHits = 60;

    public float allotedTime = 5f;
    private float remainingTime = 0f;
    private Vector3 startSize;

    [Header("UI Elements")]
    public Slider energyBar;
    public GameObject textPrompts;
    public PlayerFeedback player1Feedback;
    public PlayerFeedback player2Feedback;

    [Header("AttackChildren")]
    private GameObject mainAttractor;
    private GameObject sideAttractor1;
    private GameObject sideAttractor2;
    private GameObject fireball;

	// Use this for initialization
	void Start () {
        energyBar.maxValue = maximumNumberOfHits;
        energyBar.gameObject.SetActive(false);
        startSize = transform.localScale;

        //Child Indexes {0 = particle revolver, 1 = attractor 1, 2 = attractor 2, 3 = fireball}
        mainAttractor = transform.GetChild(0).gameObject;
        sideAttractor1 = transform.GetChild(1).gameObject;
        sideAttractor2 = transform.GetChild(2).gameObject;
        fireball = transform.GetChild(3).gameObject;
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
        numberOfHits++;
    }

    public int unleashTeamAttack()
    {
        // return team attack damage

        // damage = # hits * damage per hit * 2 to the power of # hits / 10
        // i.e, at a maximum of 50 hits: 
        // 10 hits: 100 * 10 * 2^1-1 = 100 * 10 = 1,000
        // 20 hits: 100 * 20 * 2^2-1 = 100 * 20 * 2 = 4,000
        // 30 hits: 100 * 30 * 2^3-1 = 100 * 30 * 4 = 12,000
        // 40 hits: 100 * 40 * 2^4-1 = 100 * 40 * 8 = 32,000
        // 50 hits: 100 * 50 * 2^5-1 = 100 * 50 * 16 = 80,000

        //int damageDone = Mathf.Min(numberOfHits, maximumNumberOfHits) * damagePerHit * 
        //    (int) Math.Pow(2, (Mathf.Min(numberOfHits, maximumNumberOfHits) / 10) - 1);

        //Debug.Log("num hits: " + (Mathf.Min(numberOfHits, maximumNumberOfHits)));
        //Debug.Log("damage per hit: " + damagePerHit);
        //Debug.Log("pow: " + Math.Pow(2, (Mathf.Min(numberOfHits, maximumNumberOfHits) / 10) - 1));

        // reset

        int damageDone = 2;
        if (numberOfHits >= recoverHits)
        {
            FindObjectOfType<Team>().recoverHealth();
        }
        if (numberOfHits < attackHits)
        {
            damageDone = 0;
        }
        ActivateLazer();
        displayFeedback();
        Debug.Log("Dealt Team Attack Damage to the boss: " + damageDone);
        energyBar.gameObject.SetActive(false);
        isActive = false;
        numberOfHits = 0;
       // GetComponentInChildren<Text>().text = "";
        

        return damageDone;
    }

    private void displayFeedback() {
        Debug.Log("NumHits" + numberOfHits);
        player1Feedback.giveTeamAttackFeedback(numberOfHits);
        player2Feedback.giveTeamAttackFeedback(numberOfHits);
    }

    public bool timerExpired()
    {
        return remainingTime <= 0;
    }

    private void ActivateLazer() {

        GameObject fireballIns = Instantiate(fireball, mainAttractor.transform);
        fireballIns.SetActive(true);

        //Destroy/hide the things
        mainAttractor.SetActive(false);
        sideAttractor1.SetActive(false);
        sideAttractor2.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
        textPrompts.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
        if (isActive) {
            remainingTime = remainingTime - Time.deltaTime;

            if (numberOfHits > 0) {
                transform.localScale = new Vector3(startSize.x + numberOfHits / 10.0f, startSize.y + numberOfHits / 10.0f, startSize.z + numberOfHits / 10.0f);
                //GetComponentInChildren<Text>().text = numberOfHits.ToString();
            }
        }
        //energyBar.value = Mathf.Min(numberOfHits, maximumNumberOfHits);
    }
}
