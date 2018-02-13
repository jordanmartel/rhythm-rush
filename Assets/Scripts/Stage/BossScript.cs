using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    public int maxhp = 100000;
    public int dmg = 0;
    private int hp;
    public Scrollbar healthBar;
    public Canvas winning, ssRank, sRank, aRank, bRank, cRank, dRank;
  
	// Use this for initialization
	void Start () {
        hp = maxhp;
	}
	
	// Update is called once per frame
	void Update () {

        //Update visual cue on boss
		if (hp <= 0)
        {
            //Winning
            Instantiate(winning, Vector3.zero, Quaternion.identity);
            Ranking ranking = GameObject.FindObjectOfType<Ranking>();
            string rank = ranking.rankingAtTime(ranking.time);
            switch (rank)
            {
                case "SS":
                    Instantiate(ssRank, Vector3.zero, Quaternion.identity);
                    break;
                case "S":
                    Instantiate(sRank, Vector3.zero, Quaternion.identity);
                    break;
                case "A":
                    Instantiate(aRank, Vector3.zero, Quaternion.identity);
                    break;
                case "B":
                    Instantiate(bRank, Vector3.zero, Quaternion.identity);
                    break;
                case "C":
                    Instantiate(cRank, Vector3.zero, Quaternion.identity);
                    break;
                default:
                    Instantiate(dRank, Vector3.zero, Quaternion.identity);
                    break;
            }
        }
        
        // if player health <= 0, endStatus = -1
        // then if endStatus != 0, wait until the end of the current attack/damage animations are done
        // and show the endScene
	}

    public void giveDamage(int dmg) {
        hp -= dmg;
        healthBar.size=  (1.0f * hp / maxhp);
    }
}
