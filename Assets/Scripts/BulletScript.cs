using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public bool canHit = false;
    public string key;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canHit)
        {
            if (Input.GetKeyDown(key))
            {
                print("hit successfully");
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider is CapsuleCollider)
        {
            Destroy(gameObject);
        }
        else if (collider is SphereCollider)
        {
            canHit = true;
            Behaviour halo = (Behaviour) gameObject.GetComponent("Halo");
            halo.enabled = true;
        }
    }
}