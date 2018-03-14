using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    //public int health;
    //public int maxHealth = 3;
    public Player player1;
    public Player player2;
    public bool hasEnded = false;
    public Canvas losing;
    public TeamStats stats;

    // Use this for initialization
    void Awake()
    {
        //health = maxHealth;
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
                Instantiate(losing, Vector3.zero, Quaternion.identity);
                hasEnded = true;
                //FindObjectOfType<Ranking>().enabled = false;
                FindObjectOfType<StageScript>().enabled = false;
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
            player1.GetComponent<Animation>().Play("revived_player");
            player2.stats.incrementRevive();
        }
        

        if (player2.IsDown)
        {
            player2.GetComponent<Animation>().Play("revived_player");
            player1.stats.incrementRevive();
        }
        
    }
}
