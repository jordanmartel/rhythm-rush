using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : MonoBehaviour {

    [Header("General")]
    public Player player;

    [Header("AnyKey")]
    public bool anyKeyActive = false;
    public double anyKeyTimer = 0;
    public double anyKeyMaxTime = 5;

    [Header("Pet")]
    public bool petActive = false;
    public int petHelpCount = 3;

    [Header("HitArea")]
    public bool hitAreaActive = false;

    [Header("Bomb")]
    public bool bombActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (anyKeyActive)
        {
            anyKeyTimer += Time.deltaTime;
        }
        if (petHelpCount <= 0)
        {
            petLeave();
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

    internal void anyKeyEnd()
    {
        anyKeyActive = false;
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
    }

    internal void throwBomb()
    {

    }
}
