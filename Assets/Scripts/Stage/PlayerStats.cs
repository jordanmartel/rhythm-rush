using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    public int maxCombo;
    public int failCount;
    public int perfectCount;
    public int stunCount;
    public int reviveCount;
    public int score;

    public Text scoreText;
    public Text comboText;
    public Vector3 comboInitial;
    public Vector3 scoreInitial;

    // Use this for initialization
    void Start () {
        maxCombo = 0;
        failCount = 0;
        perfectCount = 0;
        stunCount = 0;
        reviveCount = 0;
        score = 0;
        comboInitial = comboText.transform.localScale;
        scoreInitial = scoreText.transform.localScale;
}
	
    internal void updateMaxCombo(int combo)
    {
        if (combo > maxCombo)
        {
            maxCombo = combo;
        }
        StartCoroutine("comboTextJump");
    }

    internal void updateScore(int score) {
        print("update score");
        this.score += score;
        StartCoroutine("scoreTextJump");
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

    IEnumerator scoreTextJump()
    {
        float multiplier = 1.4f;
        scoreText.transform.localScale = new Vector3(scoreInitial.x * multiplier, scoreInitial.y * multiplier, scoreInitial.z * multiplier);
        while (Mathf.Abs(multiplier - 1f) > 1e-3)
        {
            multiplier -= 0.05f;
            scoreText.transform.localScale = new Vector3(scoreInitial.x * multiplier, scoreInitial.y * multiplier, scoreInitial.z * multiplier);
            yield return null;
        }
    }

    IEnumerator comboTextJump()
    {
        float multiplier = 1.4f;
        comboText.transform.localScale = new Vector3(comboInitial.x * multiplier, comboInitial.y * multiplier, comboInitial.z * multiplier);
        while (Mathf.Abs(multiplier - 1f) > 1e-3)
        {
            multiplier -= 0.05f;
            comboText.transform.localScale = new Vector3(comboInitial.x * multiplier, comboInitial.y * multiplier, comboInitial.z * multiplier);
            yield return null;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
