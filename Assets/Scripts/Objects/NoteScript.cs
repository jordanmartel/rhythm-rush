using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour {

    public string key;
    public bool canHit;
    public bool canMiss;
    public int index;
    public string placement = "left";
    public bool isCreator;
    public GameObject failObject;

    public GameObject feedback;

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider collider)
    {

        Debug.Log(collider.tag + isCreator);
        if (collider.tag == "miss")
        {

            //This part will throw errors in creator mode, it's okay
            if (placement == "left")
            {
                GameObject.FindGameObjectWithTag("stage_left").GetComponent<StageScript>().noteHitIndex++;
            }
            else
            {
                GameObject.FindGameObjectWithTag("stage_right").GetComponent<StageScript>().noteHitIndex++;
            }
            destroyWithFeedback(null, true);
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
        
        transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
    }
}
