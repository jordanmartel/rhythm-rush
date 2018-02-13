using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    public int maxhp = 150000;
    public int dmg = 0;
    public int endStatus = 0;
    private int hp;
    public Scrollbar healthBar;
  
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
            endStatus = 1;
        }
	}


    private IEnumerator FlickerDamage () {
        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        mesh.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        mesh.material.color = Color.white;
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
        healthBar.size=  (1.0f * hp / maxhp);
        StartCoroutine("FlickerDamage");
    }
}
