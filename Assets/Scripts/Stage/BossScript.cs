using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    public int maxhp = 12;
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
                double time = ranking.time;
                string rank = ranking.rankingAtTime(time);
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
                FindObjectOfType<TeamStats>().updateRanking(rank, time);
                FindObjectOfType<Ranking>().enabled = false;
                FindObjectOfType<StageScript>().enabled = false;
            }
        }
	}

    public void setAttackState()
    {
        // TODO: some sort of attack preparation animation

        // dont set this to blue if we are already in the attack state. This will allow damage
        // to still be shown correctly as red
        if (!preparingAttack)
        {
            MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();
            flickerHelper(mesh, Color.blue);
            preparingAttack = true;
        }
        

    }

    public void resetAttackState()
    {
        MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();
        flickerHelper(mesh, Color.white);
        preparingAttack = false;
    }

    private void flickerHelper(MeshRenderer[] bossMesh, Color color) {
        foreach (MeshRenderer mesh in bossMesh) {
            mesh.material.color = color;
        }
    }

    private IEnumerator FlickerDamage () {
        MeshRenderer[] bossMesh = GetComponentsInChildren<MeshRenderer>();
        flickerHelper(bossMesh, Color.red);
        yield return new WaitForSeconds(0.2f);

        if (preparingAttack) {
            flickerHelper(bossMesh, Color.blue);
        }

        else {
            flickerHelper(bossMesh, Color.white);
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

    public void recoverHealth(int heal)
    {
        hp = Mathf.Min(maxhp, hp + heal);
    }

    private void OnTriggerEnter(Collider other) {
       // if (other.tag == "laser") {
       //     StartCoroutine("FlickerDamage");
      //  }
    }
}
