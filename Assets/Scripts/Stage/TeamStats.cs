using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamStats : MonoBehaviour {
    public double completionTime;
   // public int heartsRemaining;
    public string ranking;

	// Use this for initialization
	void Start () {
        completionTime = 0;
        //heartsRemaining = 3;
        ranking = "SS";
	}
	
	// Update is called once per frame
	void Update () {
        //heartsRemaining = FindObjectOfType<Team>().health;
	}

    internal void updateRanking(string rank, double time)
    {
        ranking = rank;
        completionTime = time - 4;
    }
}
