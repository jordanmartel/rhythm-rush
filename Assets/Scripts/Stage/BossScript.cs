using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    public int maxhp = 100000;
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

    public void giveDamage(int dmg) {
        hp -= dmg;
        healthBar.size=  (1.0f * hp / maxhp);
    }
}
