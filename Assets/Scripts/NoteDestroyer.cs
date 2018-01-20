using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDestroyer : MonoBehaviour {

	private int score;
	private List<GameObject> colliderNotes;

	public List<GameObject> getNotes(){
		return colliderNotes;
	}

	public int getScore() {
		return score;
	}

	public void incrementScore(int inc) {
		score += inc;
	}

	void OnTriggerExit (Collider note) {
		Destroy (note.gameObject);
	}

	void OnTriggerEnter (Collider note) {
		colliderNotes.Add(note.gameObject);
	}

	// Use this for initialization
	void Start () {
		score = 0;
		colliderNotes = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
