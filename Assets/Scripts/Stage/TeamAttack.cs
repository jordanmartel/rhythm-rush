using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamAttack : MonoBehaviour {
    public int combo;
    public int powerLvl;
    public int powerLvlRequirement = 10;
    public int attackDamage = 3500;
    public float timer = 0f;
    public Slider energyBar;

	// Use this for initialization
	void Start () {
        energyBar.maxValue = powerLvlRequirement;
        energyBar.gameObject.SetActive(false);
	}

    internal int buildTeamAttack()
    {
        energyBar.gameObject.SetActive(true);
        GetComponentInChildren<Text>().text = "Mash the buttons!";
        powerLvl++;
        if (powerLvl >= powerLvlRequirement)
        {
            return unleashTeamAttack();
        }
        return 0;
    }

    internal int unleashTeamAttack()
    {
        int damageDone = attackDamage;
        return damageDone;
    }

    internal void Reset()
    {
        combo = 0;
        powerLvl = 0;
        powerLvlRequirement += 10;
        attackDamage += 3500;
    }

	
	// Update is called once per frame
	void Update () {
        GetComponentInChildren<Text>().text = combo.ToString();
        energyBar.value = Mathf.Min(powerLvl,powerLvlRequirement);
	}
}
