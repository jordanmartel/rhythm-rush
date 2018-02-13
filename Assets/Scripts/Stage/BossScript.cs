using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    public int maxhp = 100000;
    public int dmg = 0;
    private int hp;
    public Scrollbar healthBar;
    public Canvas winning;

    public bool hasEnded;
	// Use this for initialization
	void Start () {
        hp = maxhp;
	}
	
	// Update is called once per frame
	void Update () {
        if (!hasEnded)
        {
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
                        winning.transform.Find("SSRank").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        break;
                    case "S":
                        winning.transform.Find("SRank").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        break;
                    case "A":
                        winning.transform.Find("ARank").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        break;
                    case "B":
                        winning.transform.Find("BRank").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        break;
                    case "C":
                        winning.transform.Find("CRank").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        break;
                    default:
                        winning.transform.Find("DRank").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        break;
                }
                hasEnded = true;
            }
        }
	}

    public void giveDamage(int dmg) {
        hp -= dmg;
        healthBar.size=  (1.0f * hp / maxhp);
    }
}
