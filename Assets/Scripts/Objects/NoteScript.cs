using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour {

    public string key;
    public bool canHit;
    public bool canMiss;
    public int index;
    public string placement = "left";
    public GameObject failObject;

    public GameObject feedback;

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "miss")
        {
            print("miss");
            if (placement == "left")
            {
                print("miss left");
                GameObject.FindGameObjectWithTag("stage_left").GetComponent<StageScript>().noteHitIndex++;
            }
            else
            {
                print("miss right");
                GameObject.FindGameObjectWithTag("stage_right").GetComponent<StageScript>().noteHitIndex++;
            }
            failObject.SetActive(true);
            Destroy(gameObject);
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
    }

    public float destroyWithFeedback(GameObject hitArea)
    {
        float distance = Vector3.Distance(hitArea.transform.position, transform.position);
        Debug.Log("Distance from HitBox: " + distance);
        float score = feedback.GetComponent<PlayerFeedback>().GiveFeedback(distance);

        Debug.Log(score);
        Destroy(gameObject);

        return score;
    }


    // Update is called once per frame
    void Update () {
        StageScript stage = FindObjectOfType<StageScript>();
        float speed = (float) stage.noteTravelSpeed;
        transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
    }
}
