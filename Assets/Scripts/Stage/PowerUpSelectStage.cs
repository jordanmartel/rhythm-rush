using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpSelectStage : MonoBehaviour {

    private PowerUpHandler.powerUp[] powers = { PowerUpHandler.powerUp.HitArea, PowerUpHandler.powerUp.Pet, PowerUpHandler.powerUp.Bomb, PowerUpHandler.powerUp.AnyKey }; 

    public GameObject P1Indicator;
    public GameObject P2Indicator;

    public string firstLevel;
    [Header("Selections")]
    public GameObject UIRange;
    public GameObject UIPet;
    public GameObject UIBomb;
    public GameObject UIAnyKey;

    private List<GameObject> UIOptionList = new List<GameObject>();

    private int currP1Pos = 0;
    private int currP2Pos = 0;

    private int Joystick1;
    private int Joystick2;

    private bool p1InputReset = true;
    private bool p2InputReset = true;

    private PowerUpHandler.powerUp P1selectedPower = PowerUpHandler.powerUp.None;
    private PowerUpHandler.powerUp P2selectedPower = PowerUpHandler.powerUp.None;

    public AudioSource SFXPlayer1;
    public AudioSource SFXPlayer2;
    public AudioClip confirm;
    public AudioClip cancel;
    public AudioClip change;

    // Use this for initialization
    void Start () {

        Joystick1 = Metadata.player1Joystick;
        Joystick2 = Metadata.player2Joystick;

        UIOptionList.Add(UIRange);
        UIOptionList.Add(UIPet);
        UIOptionList.Add(UIBomb);
        UIOptionList.Add(UIAnyKey);

    }
	
	// Update is called once per frame
	void Update () {

        if (P1selectedPower == PowerUpHandler.powerUp.None || P2selectedPower == PowerUpHandler.powerUp.None) {
            updatePlayer1();
            updatePlayer2();
        } else { 
            StartCoroutine(changeScene());
        }
    }

    private IEnumerator changeScene() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(firstLevel);
    }

    private void updatePlayer1() {

        if (P1selectedPower == PowerUpHandler.powerUp.None) {
            if (Input.GetKeyDown("joystick " + Joystick1 + " button 1")){
                P1selectedPower = powers[currP1Pos];
                Metadata.P1PowerUp = P1selectedPower;
                SFXPlayer1.clip = confirm;
                SFXPlayer1.Play();
                P1Indicator.GetComponentInChildren<Animation>().Play("selectPower2");
            }

            if (p1InputReset)
                checkPlayerInput(P1Indicator, currP1Pos, Joystick1);

            float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + Joystick1 + "-Axis8");
            moveIndicator(P1Indicator, currP1Pos);
            if (dpadVertical == 0) { p1InputReset = true; }
        }
    }

    private void updatePlayer2() {

        if (P2selectedPower == PowerUpHandler.powerUp.None) {
            if (Input.GetKeyDown("joystick " + Joystick2 + " button 1")){
                P2selectedPower = powers[currP2Pos];
                Metadata.P2PowerUp = P2selectedPower;
                SFXPlayer2.clip = confirm;
                SFXPlayer2.Play();
                P2Indicator.GetComponentInChildren<Animation>().Play("selectPower");
            }

            if (p2InputReset)
                checkPlayerInput(P2Indicator, currP2Pos, Joystick2);

            //Reset input so that you can't hold down/up
            float dpadVertical2 = Input.GetAxis("Controller Axis-Joystick" + Joystick2 + "-Axis8");
            moveIndicator(P2Indicator, currP2Pos);
            if (dpadVertical2 == 0) { p2InputReset = true; }
        }
    }

    private void checkPlayerInput(GameObject indicator, int position, int joystick) {

        float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + joystick + "-Axis8");
        float vertMvmnt = 0;

        int mult = 1;
        if (Screen.height > 900) mult = 2;

        if (dpadVertical == 1) {
            if (position != 0) {
                vertMvmnt += 90 * mult;
                if (indicator == P1Indicator) { p1InputReset = false; currP1Pos--; SFXPlayer1.clip = change; SFXPlayer1.Play(); }
                if (indicator == P2Indicator) { p2InputReset = false; currP2Pos--; SFXPlayer2.clip = change; SFXPlayer2.Play(); }
            }


        } else if (dpadVertical == -1) {
            if (position != powers.Length - 1) {
                vertMvmnt -= 90 * mult;
                if (indicator == P1Indicator) { p1InputReset = false; currP1Pos++; SFXPlayer1.clip = change; SFXPlayer1.Play(); }
                if (indicator == P2Indicator) { p2InputReset = false; currP2Pos++; SFXPlayer2.clip = change; SFXPlayer2.Play(); }
            }
        }
    }

    private void moveIndicator(GameObject indicator, int position) {
        Vector3 pos = indicator.GetComponent<RectTransform>().transform.position;
        indicator.GetComponent<RectTransform>().transform.position = new Vector3(pos.x, UIOptionList[position].transform.position.y, 0);
    }

}
