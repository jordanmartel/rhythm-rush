using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour {

    public string key;
    public bool canHit;
    public bool canMiss;
    public int index;
    public bool isCreator;

    public Player player;
    public GameObject feedback;
    public GameObject stage;
    public GameObject destination;

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider collider)
    {

        if (collider.tag == "miss")
        {
            // note was missed, so this player has failed the phase
            player.failedPhase = true;

            player.updateComboCount(false);
            destroyWithFeedback(null, true);

            // forcefully reset the player combo when a note is missed
            player.combo = 0;
        }
        else if (collider.tag == "hit")
        {
            canHit = true;
            Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
            halo.enabled = true;
        }

        else if (collider.tag == "wrong_hit")
        {
            canMiss = true;
        }

        if (isCreator && collider.tag == "destroyer_creator") {
            Destroy(gameObject);
        }
    }

    public int destroyWithFeedback(GameObject hitArea, bool correct)
    {
        int score = 0;

        if (hitArea == null) {
            feedback.GetComponent<PlayerFeedback>().GiveFeedback(100, correct);
            player.activeNotes.Remove(gameObject);

        } else {
            float distance = Vector3.Distance(hitArea.transform.position, transform.position);
            score = feedback.GetComponent<PlayerFeedback>().GiveFeedback(distance, correct);
        }

        Destroy(gameObject);

        return score;
    }


    // Update is called once per frame
    void Update () {
        StageScript stage = FindObjectOfType<StageScript>();
        float speed;

        //Debug.Log(stage.noteTravelSpeed);
        //if stage null(like in creator mode) default to speed 3
        speed = (stage == null) ? 3f : (float)stage.noteTravelSpeed;
        
        transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, speed * Time.deltaTime);
    }
}
