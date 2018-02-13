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

    private bool preparingAttack = false;
  
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
