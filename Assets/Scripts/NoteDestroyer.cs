using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoteDestroyer : MonoBehaviour {

	public Text scoreText;
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
		scoreText.text = score.ToString();
	}

	void OnTriggerExit (Collider note) {
		Destroy (note.gameObject);
		colliderNotes.Remove (note.gameObject);
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
