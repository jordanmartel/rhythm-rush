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
    public AudioClip anyKeyOnAudio;

    [Header("Pet")]
    public bool petUsed = false;
    public bool petActive = false;
    public int petHelpCount = 5;
    public Animator petAnimator;
    public GameObject petObject;
    public AudioClip petHelpAudio;

    [Header("HitArea")]
    public bool hitAreaUsed = false;

    [Header("Bomb")]
    public bool bombUsed = false;
    public bool bombActive = false;
    public GameObject bombObject;
    public AudioClip bombAudio;

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
        if (petUsed)
        {
            if (petAnimator.GetBool("ReallyGone"))
            {
                petObject.SetActive(false);
            }
        }
        if (bombUsed)
        {
            if (bombActive)
            {
                activateBomb();
            }
            else
            {
                deactivateBomb();
            }
        }
	}

    internal void anyKeyOn()
    {
        anyKeyActive = true;
        GetComponent<AudioSource>().clip = anyKeyOnAudio;
        GetComponent<AudioSource>().Play();
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
            petObject.SetActive(true);
            GetComponent<AudioSource>().clip = anyKeyOnAudio;
            GetComponent<AudioSource>().Play();
            petHelpCount = 5;
        }
    }

    internal void petLeave()
    {
        petActive = false;
        petAnimator.SetBool("Gone", true);
        petHelpCount = 5;
    }

    internal void petHelp()
    {
        GetComponent<AudioSource>().clip = petHelpAudio;
        GetComponent<AudioSource>().Play();
        petHelpCount -= 1;
        petAnimator.SetTrigger("Help");
        if (petHelpCount <= 0)
        {
            petLeave();
        }
    }

    internal void activateBomb()
    {
        if (bombObject != null)
        {
            bombObject.transform.Find("Bomb anim").gameObject.SetActive(true);
        }
    }
        

    internal void deactivateBomb()
    {
        if (bombObject != null)
        {
            bombObject.transform.Find("Bomb anim").gameObject.SetActive(false);
        }
    }

    internal void throwBomb()
    {
        ParticleSystem particles = bombObject.GetComponentInChildren<ParticleSystem>();
        GetComponent<AudioSource>().clip = bombAudio;
        GetComponent<AudioSource>().Play();
        if (particles != null)
        {
            particles.Play();
        }
        else
        {
            print("bomb particle not found");
        }
        bombActive = false;
        GameObject[] notes = GameObject.FindGameObjectsWithTag("note");
        foreach (GameObject note in notes)
        {
            if (player == note.GetComponent<NoteScript>().player)
            {
                player.stats.updateScore(50);
                player.activeNotes.Remove(note);
                FindObjectOfType<BossScript>().giveDamage(50);
                note.GetComponent<NoteScript>().DestroyWithShockwave();
            }
        }
    }
}
