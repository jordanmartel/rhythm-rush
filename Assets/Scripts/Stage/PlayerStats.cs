using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    public int maxCombo;
    public int failCount;
    public int perfectCount;
    public int stunCount;
    public int reviveCount;
    public int score;

	// Use this for initialization
	void Start () {
        maxCombo = 0;
        failCount = 0;
        perfectCount = 0;
        stunCount = 0;
        reviveCount = 0;
        score = 0;
	}
	
    internal void updateMaxCombo(int combo)
    {
        if (combo > maxCombo)
        {
            maxCombo = combo;
        }
    }

    internal void updateScore(int score) {
        this.score += score;
    }

    internal void incrementFail()
    {
        failCount++;
    }

    internal void incrementPerfect()
    {
        perfectCount++;
    }

    internal void incrementStun()
    {
        stunCount++;
    }

    internal void incrementRevive()
    {
        reviveCount++;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
