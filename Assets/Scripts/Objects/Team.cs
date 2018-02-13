using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int health;
    public Player player1;
    public Player player2;
    public bool hasEnded = false;
    public Canvas losing;

    // Use this for initialization
    void Start()
    {
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasEnded)
        {
            if (health <= 0)
            {
                FindObjectOfType<Ranking>().enabled = false;
                FindObjectOfType<StageScript>().enabled = false;
                Instantiate(losing, Vector3.zero, Quaternion.identity);
                hasEnded = true;
            }
        }
    }

    // TODO: should disable hearts in UI as well
    public void attackedByBoss()
    {
        health = health - 1;
    }
}
