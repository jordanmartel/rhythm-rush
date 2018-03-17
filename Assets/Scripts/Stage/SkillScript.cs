using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SkillScript : MonoBehaviour {

    [Header("General")]
    public Player player;

    [Header("AnyKey")]
    public bool anyKeyUsed = false;
    public bool anyKeyActive = false;
    public double anyKeyTimer = 0;
    public double anyKeyMaxTime = 5;

    [Header("Pet")]
    public bool petUsed = false;
    public bool petActive = false;
    public int petHelpCount = 3;

    [Header("HitArea")]
    public bool hitAreaUsed = false;

    [Header("Bomb")]
    public bool bombUsed = false;

	// Use this for initialization
	void Start () {
        string powerUpName = Enum.GetName(typeof(PowerUpHandler.powerUp), player.powerUp);
        switch (powerUpName)
        {
            case "HitArea":
                hitAreaUsed = true;
                break;
            case "Pet":
                petUsed = true;
                break;
            case "Bomb":
                bombUsed = true;
                break;
            case "AnyKey":
                anyKeyUsed = true;
                break;
            default:
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (anyKeyActive)
        {
            anyKeyTimer += Time.deltaTime;
            if (anyKeyTimer > anyKeyMaxTime)
            {
                anyKeyEnd();
            }
        }
	}

    internal void anyKeyOn()
    {
        anyKeyActive = true;
        anyKeyTimer = 0;
        GameObject[] notes = GameObject.FindGameObjectsWithTag("note");
        foreach (GameObject note in notes)
        {
            if (player == note.GetComponent<NoteScript>().player)
            {
                note.GetComponent<NoteScript>().anyKey = true;
            }
        }
    }

    internal void triggerSkill(bool pUpAvailable)
    {
        if (pUpAvailable)
        {
            if (anyKeyUsed)
            {
                anyKeyOn();
            }
            else if (petUsed)
            {
                petSummon();
            }
            else if (bombUsed)
            {
                throwBomb();
            }
            else if (hitAreaUsed)
            {
                print("WARNING: something went wrong (hit area skill is triggered)");
            }
            else
            {
                print("WARNING: something went wrong (no skill selected)");
            }
        }
    }

    internal void anyKeyEnd()
    {
        anyKeyActive = false;
        anyKeyTimer = 0;
    }

    internal void petSummon()
    {
        if (!petActive)
        {
            petActive = true;
            petHelpCount = 3;
        }
    }

    internal void petLeave()
    {
        petActive = false;
        petHelpCount = 3;
    }

    internal void petHelp()
    {
        petHelpCount -= 1;
        if (petHelpCount <= 0)
        {
            petLeave();
        }
    }

    internal void throwBomb()
    {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("note");
        foreach (GameObject note in notes)
        {
            if (player == note.GetComponent<NoteScript>().player)
            {
                player.stats.updateScore(100);
                player.activeNotes.Remove(note);
                note.GetComponent<NoteScript>().DestroyWithShockwave();
            }
        }
    }
}
