using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int health;
    public Player player1;
    public Player player2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO: should disable hearts in UI as well
    public void attackedByBoss()
    {
        health = health - 1;
    }
}
