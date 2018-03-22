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

    [Header("Object References")]
    public GameObject feedback;
    public StageScript stage;
    public GameObject destination;
    public GameObject shockwave;
    public GameObject particlesObj;
    public Animator weaponAnimator;


    public Material anyKeyMaterial;
    public Material anyKeyBossMaterial;

    public bool anyKey = false;

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider collider)
    {

        if (collider.tag == "miss")
        {
            player.updateComboCount(false, 0);
            // forcefully reset the player combo when a note is missed
            player.resetCombo();

            if (player.skillController.petActive)
            {
                player.skillController.petHelp();
                player.updateComboCount(false, 0);

                player.resetCombo();
                // destroy with a shockwave so that the player knows their pet saved them
                DestroyWithShockwave();

            }
            else
            {
                // note was missed, so this player has failed the phase

                // if a player misses a note and the boss attack is in progress, enable the revive phase
                if (stage.bossAttackInProgress)
                {
                    stage.isRevival = true;
                    player.KnockDownPlayer();
                    if (stage.bossAnimator != null)
                    {
                        stage.bossAnimator.SetBool("Attacking", true);
                        stage.bossAnimator.SetBool("PreparingAttack", false);
                    }
                    stage.bossAttackInProgress = false;
                }
                
                // failed a revive
                else if (stage.revivalInProgress)
                {
                    stage.isRevival = true;

                    // attack the opposite player that is stunned (bleeding out)
                    if (stage.team.player1 == player)
                    {
                        stage.team.player2.attackedByBoss();
                    }

                    else
                    {
                        stage.team.player1.attackedByBoss();
                    }
                }

                else
                {
                    player.attackedByBoss();
                }

                destroyWithFeedback(null, true);
            }
            
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
            FindObjectOfType<BossScript>().giveDamage(score);
            DestroyWithShockwave();

            if (correct) {
                weaponAnimator.SetBool("Attack",true );
                activateAttack();
                //sendParticles(score);
            }
        }

        Destroy(gameObject);

        return score;
    }

    private void activateAttack() {
        BossScript bs = FindObjectOfType<BossScript>();
        GameObject attackObject = FindObjectOfType<BossScript>().transform.Find(player.attackType).gameObject;
        if (attackObject.activeSelf) {
            attackObject.SetActive(false);
        }
        attackObject.SetActive(true);
        attackObject.GetComponent<SelfDeactivate>().resetTimer();
        bs.updateHealthScreen();
    }

    private void sendParticles(int damage) {
        GameObject particles = Instantiate(particlesObj, this.transform.localPosition, Quaternion.identity);
        particles.GetComponentInChildren<ParticleSystem>().Pause();
        particles.GetComponentInChildren<noteAttractor>().damage = damage;
        particles.GetComponentInChildren<noteAttractor>().attackType = player.attackType;

        //Change for different animation types
        //particles.GetComponentInChildren<Animation>().Play("");

        //Change Below for revive phase
        //particles.GetComponentInChildren<noteAttractor>().target

    }

    public void DestroyWithShockwave() {
        GameObject shockwaveInst = Instantiate(shockwave, this.transform.localPosition, Quaternion.identity);
        Destroy(shockwaveInst, 1f);
        player.activeNotes.Remove(gameObject);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
        StageScript stage = FindObjectOfType<StageScript>();
        float speed;

        //Debug.Log(stage.noteTravelSpeed);
        //if stage null(like in creator mode) default to speed 3
        speed = (stage == null) ? 3f : (float)stage.noteTravelSpeed;
        
        if (destination == null)
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, speed * Time.deltaTime);
        }

        if (anyKey)
        {
            if (FindObjectOfType<StageScript>().bossAttackInProgress)
            {
                GetComponent<MeshRenderer>().material = anyKeyBossMaterial;
            }
            else
            {
                GetComponent<MeshRenderer>().material = anyKeyMaterial;
            }
        }

    }
}
