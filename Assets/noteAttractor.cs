using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class noteAttractor : MonoBehaviour {
    ParticleSystem ps;
    ParticleSystem.Particle[] m_Particles;
    public Transform target;
    public float speed = 5f;

    public int damage = 0;
    public string attackType = "";

    private bool isAttractActive = false;
    int numParticlesAlive;
    void Start() {
        ps = GetComponent<ParticleSystem>();
        if (!GetComponent<Transform>()) {
            GetComponent<Transform>();
        }

        target = GameObject.FindGameObjectWithTag("boss_target").transform;
    }
    void Update() {

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
    }

    void OnTriggerEnter(Collider other) {
        if ("bossplayer".Contains(other.tag)) {
            Destroy(transform.parent.gameObject);
        }
    }


}
