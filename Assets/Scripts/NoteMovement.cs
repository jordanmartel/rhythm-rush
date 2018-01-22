using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour {

	public float speed = 3.0f;
	public string direction;
    
    // Use this for initialization

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (direction == "left") {
			transform.Translate (Vector3.left * Time.deltaTime * speed);
		}
		else if (direction == "right") {
			transform.Translate (Vector3.right * Time.deltaTime * speed);
    }
}
