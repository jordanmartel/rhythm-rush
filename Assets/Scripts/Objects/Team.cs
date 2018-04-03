using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Team : MonoBehaviour
{
    //public int health;
    //public int maxHealth = 3;
    public Player player1;
    public Player player2;
    public bool hasEnded = false;
    public Canvas losing;
    public AudioClip losingClip;
    public TeamStats stats;

    private double loseTimer = 6;

    // Use this for initialization
    void Awake()
    {
        //health = maxHealth;
        loseTimer = 6;
        player1.powerUp = Metadata.P1PowerUp;
        player2.powerUp = Metadata.P2PowerUp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasEnded)
        {
            if (player1.health <= 0 || player2.health <=0)
            {
                player1.anim.SetTrigger("Death");
                player1.anim.SetBool("Ended", true);
                player2.anim.SetTrigger("Death");
                player2.anim.SetBool("Ended", true);
                print("deaddddd!");
                Instantiate(losing, Vector3.zero, Quaternion.identity);
                hasEnded = true;
                //FindObjectOfType<Ranking>().enabled = false;
                FindObjectOfType<StageScript>().enabled = false;
                FindObjectOfType<AudioControl>().GetComponent<AudioSource>().clip = losingClip;
                FindObjectOfType<AudioControl>().GetComponent<AudioSource>().Play();
            }
        }

        else
        {
            loseTimer -= Time.deltaTime;

            if (loseTimer <= 0)
            {
                SceneManager.LoadScene("ConnectController");
            }

        }
    }

    // TODO: should disable hearts in UI as well
    //public void attackedByBoss()
    //{
    //    health = health - 1;
    //}

    public void recoverHealth()
    {
        player1.recoverHealth();
        player2.recoverHealth();
    }

    // returns true if any notes are in play or any notes in phase remain to be played
    public bool hasNotesLeft()
    {
        return ((player1.notes.Count > 0) || 
            (player2.notes.Count > 0) || 
            (player1.activeNotes.Count > 0) || 
            (player2.activeNotes.Count > 0));
    }

    public bool hasFailedPhase()
    {
        return player1.failedPhase || player2.failedPhase;
    }

    public void nextPhaseBegin()
    {
        player1.failedPhase = false;
        player2.failedPhase = false;
    }

    public void revivePlayer() {
        //Plays revive animation when player is fine 
        Debug.Log("Reviving: ");

        if (player1.IsDown)
        {
            //player1.GetComponent<Animation>().Play("revived_player");
            player1.anim.SetBool("Stunned", false);
            player1.anim.SetTrigger("Revive");
            player2.stats.incrementRevive();
            player2.reviveIndicator.enabled = false;
        }
        

        if (player2.IsDown)
        {
            //player2.GetComponent<Animation>().Play("revived_player");
            player2.anim.SetBool("Stunned", false);
            player2.anim.SetTrigger("Revive");
            player1.stats.incrementRevive();
            player1.reviveIndicator.enabled = false;
        }
        
    }

    public void copyStats() {
        Metadata.P1maxCombo = player1.stats.maxCombo;
        Metadata.P1failCount = player1.stats.failCount;
        Metadata.P1perfectCount = player1.stats.perfectCount;
        Metadata.P1stunCount = player1.stats.stunCount;
        Metadata.P1reviveCount = player1.stats.reviveCount;
        Metadata.P1score = player1.stats.score;

        Metadata.P2maxCombo = player2.stats.maxCombo;
        Metadata.P2failCount = player2.stats.failCount;
        Metadata.P2perfectCount = player2.stats.perfectCount;
        Metadata.P2stunCount = player2.stats.stunCount;
        Metadata.P2reviveCount = player2.stats.reviveCount;
        Metadata.P2score = player2.stats.score;
    }
}
