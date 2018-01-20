using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	public NoteDestroyer p1NoteDestroyer;
	public NoteDestroyer p2NoteDestroyer;

	private void checkControlDestroyer (KeyCode key, NoteDestroyer destroyer)  {

		if (destroyer != null) {
			List<GameObject> notes = destroyer.getNotes ();
			foreach (GameObject note in notes) {
				Destroy (note);
				destroyer.incrementScore (100);
			}
		}
	}


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		// Player1 Input 
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			checkControlDestroyer (KeyCode.LeftArrow, p1NoteDestroyer);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			checkControlDestroyer (KeyCode.RightArrow, p1NoteDestroyer);
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			checkControlDestroyer (KeyCode.UpArrow, p1NoteDestroyer);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			checkControlDestroyer (KeyCode.DownArrow, p1NoteDestroyer);
		}


		//Player 2 Input  
		else if (Input.GetKeyDown(KeyCode.A)) {
			checkControlDestroyer (KeyCode.A, p2NoteDestroyer);
		}
		else if (Input.GetKeyDown(KeyCode.W)) {
			checkControlDestroyer (KeyCode.W, p2NoteDestroyer);
		}
		else if (Input.GetKeyDown(KeyCode.D)) {
			checkControlDestroyer (KeyCode.D, p2NoteDestroyer);
		}
		else if (Input.GetKeyDown(KeyCode.S)) {
			checkControlDestroyer (KeyCode.S, p2NoteDestroyer);
		}


	}
}
