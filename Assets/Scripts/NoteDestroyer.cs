using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDestroyer : MonoBehaviour {

	private int score;

	void OnTriggerExit (Collider other) {
		Destroy (other.gameObject);
	}

	// Use this for initialization
	void Start () {
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
