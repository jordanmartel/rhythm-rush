using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamAttack : MonoBehaviour {

    public bool isActive;
    public int numberOfHits;
    public int maximumNumberOfHits = 50;
    public int damagePerHit = 100;

    public float allotedTime = 5f;
    private float remainingTime = 0f;

    public Slider energyBar;

	// Use this for initialization
	void Start () {
        energyBar.maxValue = maximumNumberOfHits;
        energyBar.gameObject.SetActive(false);
	}

    public void startTeamAttack()
    {
        isActive = true;
        energyBar.gameObject.SetActive(true);
        GetComponentInChildren<Text>().text = "Mash the buttons!";
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

        int damageDone = Mathf.Min(numberOfHits, maximumNumberOfHits) * damagePerHit * 
            (int) Math.Pow(2, (Mathf.Min(numberOfHits, maximumNumberOfHits) / 10) - 1);

        //Debug.Log("num hits: " + (Mathf.Min(numberOfHits, maximumNumberOfHits)));
        //Debug.Log("damage per hit: " + damagePerHit);
        //Debug.Log("pow: " + Math.Pow(2, (Mathf.Min(numberOfHits, maximumNumberOfHits) / 10) - 1));

        // reset
        energyBar.gameObject.SetActive(false);
        isActive = false;
        numberOfHits = 0;
        GetComponentInChildren<Text>().text = "";
        Debug.Log("Dealt Team Attack Damage to the boss: " + damageDone);

        return damageDone;
    }

    public bool timerExpired()
    {
        return remainingTime <= 0;
    }

	// Update is called once per frame
	void Update () {

        if (isActive)
        {
            remainingTime = remainingTime - Time.deltaTime;

            if (numberOfHits > 0)
            {
                GetComponentInChildren<Text>().text = numberOfHits.ToString();
            }
            energyBar.value = Mathf.Min(numberOfHits, maximumNumberOfHits);
        }
	}
}
