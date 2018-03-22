using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorResetCounter : MonoBehaviour {

    public int maxValue = 4;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Animator animator = GetComponent<Animator>();
        if (animator.GetBool("Attack")) {
            StartCoroutine(reset(animator));
        }
	}

    private IEnumerator reset(Animator animator ) {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attack", false);
    }
}
