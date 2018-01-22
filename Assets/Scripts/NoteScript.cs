using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour {

    public string key;
    public bool canHit;

	// Use this for initialization
	void Start () {
		
	}

    KeyCode stringToKey(string beat_string)
    {
        switch (beat_string)
        {
            case "0":
                return KeyCode.UpArrow;
            default:
                return KeyCode.DownArrow;
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        print("triggered");
        if (collider.tag == "miss")
        {
            print("miss");
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
        if (canHit)
        {
            if (Input.GetKeyDown(stringToKey(key)))
            {
                print("hit successfully");
                Destroy(gameObject);
            }
        }
    }
}
