using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BossScript : MonoBehaviour {

    public int maxhp = 12;
    public int dmg = 0;
    private int hp;
    public Slider healthBar;
    public Canvas winningCanvas;
    public bool hasEnded = false;

    private double stageCompleteTimer = 0;
    private double idleChangeTimer = 0;
    public string nextStage = "";

    private bool preparingAttack = false;
    public Animator animator;

    // Use this for initialization
	void Start () {
        hp = maxhp;
	}
	
	// Update is called once per frame
	void Update () {

        // quit on esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("ConnectController");

        }

        // skip to the next scene on N
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(nextStage);
        }

        // restart the scene on R
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (animator != null)
        {
            // every 4 seconds, there is a chance to play a random idle animation
            if (idleChangeTimer > 4)
            {
                idleChangeTimer = 0;
                int num = UnityEngine.Random.Range(1, 5);
                
                // decide whether to switch boss idle animation
                // 25% chance of entering the second idle stage
                if (num > 3)
                {
                    animator.SetBool("Idle 2", true);
                }
            }
            idleChangeTimer += Time.deltaTime;
        }

        if (!hasEnded)
        {
            //Update visual cue on boss
            if (hp <= 0)
            {
                //Winning
                Canvas winning = Instantiate(winningCanvas, Vector3.zero, Quaternion.identity);
                winning.transform.Find("Status").gameObject.SetActive(true);

                /*Ranking ranking = GameObject.FindObjectOfType<Ranking>();
                double time = ranking.time;
                string rank = ranking.rankingAtTime(time);
                switch (rank)
                {
                    case "SS":
                        winning.transform.Find("SSRank").gameObject.SetActive(true);
                        break;
                    case "S":
                        winning.transform.Find("SRank").gameObject.SetActive(true);
                        break;
                    case "A":
                        winning.transform.Find("ARank").gameObject.SetActive(true);
                        break;
                    case "B":
                        winning.transform.Find("BRank").gameObject.SetActive(true);
                        break;
                    case "C":
                        winning.transform.Find("CRank").gameObject.SetActive(true);
                        break;
                    default:
                        winning.transform.Find("DRank").gameObject.SetActive(true);
                        break;
                }
                */
                hasEnded = true;
                //FindObjectOfType<TeamStats>().updateRanking(rank, time);
                //FindObjectOfType<Ranking>().enabled = false;
                FindObjectOfType<StageScript>().enabled = false;
                StartCoroutine("FadeMusic");

                GameObject[] notes = GameObject.FindGameObjectsWithTag("note");
                foreach (GameObject note in notes)
                {
                    Destroy(note);
                }
            }
        }

        // boss is dead, time to move to next stage!
        else {

            if (stageCompleteTimer > 5)
            {
                if (nextStage != "")
                {
                    StageScript sScript = FindObjectOfType<StageScript>();
                    sScript.copyPlayerStats();


                    Metadata.nextStage = nextStage;
                    Metadata.currentStage = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene("StatsScene");
                }

                else
                {
                    SceneManager.LoadScene("ConnectController");

                }
            }
            stageCompleteTimer += Time.deltaTime;
        }
    }

    IEnumerator FadeMusic()
    {
        AudioControl[] audioObjects = FindObjectsOfType<AudioControl>();
        foreach(AudioControl audioObject in audioObjects)
        {
            while (audioObject.GetComponent<AudioSource>().volume != 0)
            {
                print(audioObject.GetComponent<AudioSource>().volume);
                audioObject.GetComponent<AudioSource>().volume = Mathf.Max(audioObject.GetComponent<AudioSource>().volume - 0.001f,0f);
                yield return null;
            }
        }
    }

    /*public void setAttackState()
    {
        // TODO: some sort of attack preparation animation

        // dont set this to blue if we are already in the attack state. This will allow damage
        // to still be shown correctly as red
        if (!preparingAttack)
        {
            //MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();
            //flickerHelper(mesh, Color.blue);
            preparingAttack = true;
        }
        

    }
    */

    /*public void resetAttackState()
    {
        //MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();
        //flickerHelper(mesh, Color.white);
        preparingAttack = false;
    }

    //private void flickerHelper(MeshRenderer[] bossMesh, Color color) {
        //foreach (MeshRenderer mesh in bossMesh) {
            //mesh.material.color = color;
        //}
    //}

    /*
    private IEnumerator FlickerDamage () {
        MeshRenderer[] bossMesh = GetComponentsInChildren<MeshRenderer>();
        flickerHelper(bossMesh, Color.red);
        yield return new WaitForSeconds(0.2f);

        if (preparingAttack) {
            flickerHelper(bossMesh, Color.blue);
        }

        else {
            flickerHelper(bossMesh, Color.white);
        }
    }
    */

    public void giveDamage(int dmg) {

        // plz no negative hp
        if (dmg > hp)
        {
            hp = 0;
        }

        else
        {
            hp -= dmg;
        }
        healthBar.value = (1.0f * hp / maxhp);
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Damaged", true);
        }
        //StartCoroutine("FlickerDamage");
    }

    public void recoverHealth(int heal)
    {
        hp = Mathf.Min(maxhp, hp + heal);
    }

    private void OnTriggerEnter(Collider other) {
        // if (other.tag == "laser") {
        //     StartCoroutine("FlickerDamage");
        //  }
        if (other.tag == "particle hit") {
            noteAttractor nA = other.GetComponentInChildren<noteAttractor>();
            giveDamage(nA.damage);
        }

    }
}
