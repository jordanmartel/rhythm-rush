using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpSelectStage : MonoBehaviour {

    private PowerUpHandler.powerUp[] powers = { PowerUpHandler.powerUp.HitArea, PowerUpHandler.powerUp.Pet, PowerUpHandler.powerUp.Bomb, PowerUpHandler.powerUp.AnyKey }; 

    public GameObject P1Indicator;
    public GameObject P2Indicator;

    private int currP1Pos = 0;
    private int currP2Pos = 0;

    private int Joystick1;
    private int Joystick2;

    private bool p1InputReset = true;
    private bool p2InputReset = true;

    private PowerUpHandler.powerUp P1selectedPower = PowerUpHandler.powerUp.None;
    private PowerUpHandler.powerUp P2selectedPower = PowerUpHandler.powerUp.None;



    // Use this for initialization
    void Start () {

        Joystick1 = Metadata.player1Joystick;
        Joystick2 = Metadata.player2Joystick;

	}
	
	// Update is called once per frame
	void Update () {

        if (P1selectedPower == PowerUpHandler.powerUp.None || P2selectedPower == PowerUpHandler.powerUp.None) {
            updatePlayer1();
            updatePlayer2();
        } else {
            Debug.Log("New Scene");
            StartCoroutine(changeScene());
        }
    }

    private IEnumerator changeScene() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GrassStage");
    }

    private void updatePlayer1() {
        float mvmnt = 0;

        if (P1selectedPower == PowerUpHandler.powerUp.None) {
            if (Input.GetKeyDown("joystick " + Joystick1 + " button 1")){
                P1selectedPower = powers[currP1Pos];
                Metadata.P1PowerUp = P1selectedPower;
                P1Indicator.GetComponentInChildren<Animation>().Play("selectPower2");
            }

            if (p1InputReset)
                mvmnt = checkPlayerInput(P1Indicator, currP1Pos, Joystick1);

            float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + Joystick1 + "-Axis8");
            moveIndicator(P1Indicator, mvmnt);
            if (dpadVertical == 0) { p1InputReset = true; }
        }
    }

    private void updatePlayer2() {
        float mvmnt = 0;

        if (P2selectedPower == PowerUpHandler.powerUp.None) {
            if (Input.GetKeyDown("joystick " + Joystick2 + " button 1")){
                P2selectedPower = powers[currP2Pos];
                Metadata.P2PowerUp = P2selectedPower;
                P2Indicator.GetComponentInChildren<Animation>().Play("selectPower");
            }

            if (p2InputReset)
                mvmnt = checkPlayerInput(P2Indicator, currP2Pos, Joystick2);

            //Reset input so that you can't hold down/up
            float dpadVertical2 = Input.GetAxis("Controller Axis-Joystick" + Joystick2 + "-Axis8");
            moveIndicator(P2Indicator, mvmnt);
            if (dpadVertical2 == 0) { p2InputReset = true; }
        }
    }

    private float checkPlayerInput(GameObject indicator, int position, int joystick) {

        float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + joystick + "-Axis8");
        float vertMvmnt = 0;

        if (dpadVertical == 1) {
            if (position != 0) {
                vertMvmnt += 100;
                if (indicator == P1Indicator) { p1InputReset = false; currP1Pos--; }
                if (indicator == P2Indicator) { p2InputReset = false; currP2Pos--; }
            }


        } else if (dpadVertical == -1) {
            if (position != powers.Length - 1) {
                vertMvmnt -= 100;
                if (indicator == P1Indicator) { p1InputReset = false; currP1Pos++; }
                if (indicator == P2Indicator) { p2InputReset = false; currP2Pos++; }
            }
        }
        return vertMvmnt;
    }

    private void moveIndicator(GameObject indicator, float mvmnt) {
        indicator.GetComponent<RectTransform>().transform.Translate(0, mvmnt, 0);
    }

}
