using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour {

    public string key;
    public bool canHit;
    public int index;
    public string placement = "left";

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
                GameObject.FindGameObjectWithTag("stage_left").GetComponent<StageScript>().noteHitIndex++;
            }
            else
            {
                GameObject.FindGameObjectWithTag("stage_right").GetComponent<StageScript>().noteHitIndex++;
            }
            Destroy(gameObject);
        }
        else if (collider.tag == "hit")
        {
            canHit = true;
            Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
            halo.enabled = true;
        }
    }

    // Update is called once per frame
    void Update () {
        StageScript stage = FindObjectOfType<StageScript>();
        float speed = (float) stage.noteTravelSpeed;
        transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
    }
}
