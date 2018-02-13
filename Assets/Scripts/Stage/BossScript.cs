﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    public int maxhp = 150000;
    public int dmg = 0;
    private int hp;
    public Scrollbar healthBar;
    public Canvas winning;
    public bool hasEnded = false;

    private bool preparingAttack = false;

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
                Canvas winningCanvas = Instantiate(winning, Vector3.zero, Quaternion.identity);
                Ranking ranking = GameObject.FindObjectOfType<Ranking>();
                string rank = ranking.rankingAtTime(ranking.time);
                switch (rank)
                {
                    case "SS":
                        winningCanvas.transform.Find("SSRank").GetComponent<Image>().color = Color.white;
                        break;
                    case "S":
                        winningCanvas.transform.Find("SRank").GetComponent<Image>().color = Color.white;
                        break;
                    case "A":
                        winningCanvas.transform.Find("ARank").GetComponent<Image>().color = Color.white;
                        break;
                    case "B":
                        winningCanvas.transform.Find("BRank").GetComponent<Image>().color = Color.white;
                        break;
                    case "C":
                        winningCanvas.transform.Find("CRank").GetComponent<Image>().color = Color.white;
                        break;
                    default:
                        winningCanvas.transform.Find("DRank").GetComponent<Image>().color = Color.white;
                        break;
                }
                hasEnded = true;
                FindObjectOfType<Ranking>().enabled = false;
                FindObjectOfType<StageScript>().enabled = false;
            }
        }
	}

    public void setAttackState()
    {

        // dont set this to blue if we are already in the attack state. This will allow damage
        // to still be shown correctly as red
        if (!preparingAttack)
        {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            mesh.material.color = Color.blue;
            preparingAttack = true;
        }
        

    }

    public void resetAttackState()
    {
        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        mesh.material.color = Color.white;
        preparingAttack = false;
    }


    private IEnumerator FlickerDamage () {
        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        mesh.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);

        if (preparingAttack)
        {
            mesh.material.color = Color.blue;
        }

        else
        {
            mesh.material.color = Color.white;
        }
    }

    public void giveDamage(int dmg) {

        // plz no negative hp
        if (dmg > hp)
        {
            hp = 0;
        }

        else
        {
            hp -= dmg;
        }
        healthBar.size = (1.0f * hp / maxhp);
        StartCoroutine("FlickerDamage");
        

    }
}
