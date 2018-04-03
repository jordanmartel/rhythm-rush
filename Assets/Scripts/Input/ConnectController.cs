using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ConnectController : MonoBehaviour {

    private int connectedControllers = 0;
    public AudioClip confirm;
    public AudioClip cancel;
    public AudioSource SFXPlayer1;
    public AudioSource SFXPlayer2;

    public Text Player1Text;
    public Text Player2Text;

    public string firstStage;

    private bool player1Ready = false;
    private bool player2Ready = false;

    // unity is super gross and randomly assigns a controller to a joystick. Will have to manually 
    // keep track of which joystick buttons belong to each player

    // Use this for initialization

    IEnumerator playConfirm(AudioSource player) {
        player.Play();
        yield return new WaitForSeconds(1.0f);

        if (player1Ready && player2Ready || Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(firstStage);
        }
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {

        // uncomment this to see what button the player is pushing
        /*
        for (int i = 0; i < 20; i++)
        {

            if (PlayerObject.player1Joystick != -1)
            {
                if (Input.GetKeyDown("joystick " + PlayerObject.player1Joystick + " button " + i))
                {
                    Debug.Log("player 1 button " + i);
                }
            }

            if (PlayerObject.player2Joystick != -1)
            {
                if (Input.GetKeyDown("joystick " + PlayerObject.player2Joystick + " button " + i))
                {
                    Debug.Log("player 2 button " + i);
                }

            }
        }*/

        if (Metadata.player1Joystick != -1)
        {
            // player1 has pressed the PS4 Options button (joystick button 9)
            if (Input.GetKeyDown("joystick " + Metadata.player1Joystick + " button 9"))
            {
                player1Ready = !player1Ready;
                if (player1Ready)
                {
                    SFXPlayer1.clip = confirm;
                    StartCoroutine(playConfirm(SFXPlayer1));
                }
                else
                {
                    SFXPlayer1.clip = cancel;
                    StartCoroutine(playConfirm(SFXPlayer1));
                }
            }
        }

        if (Metadata.player2Joystick != -1)
        {
            // player2 has pressed the PS4 Options button (joystick button 9)
            if (Input.GetKeyDown("joystick " + Metadata.player2Joystick + " button 9"))
            {
                player2Ready = !player2Ready;
                if (player2Ready)
                {
                    SFXPlayer2.clip = confirm;
                    StartCoroutine(playConfirm(SFXPlayer2));
                }
                else
                {
                    SFXPlayer2.clip = cancel;
                    StartCoroutine(playConfirm(SFXPlayer2));
                }
            }

        }

        

        bool player1Connected = false;
        bool player2Connected = false;

        connectedControllers = 0;
        string[] controllerNames = Input.GetJoystickNames();
        for (int x = 1; x < Input.GetJoystickNames().Length + 1; x++) {
            if (controllerNames[x - 1].Equals("Wireless Controller") || controllerNames[x - 1].Contains("Xbox One")) {

                if (x == Metadata.player1Joystick) {
                    player1Connected = true;
                    continue;
                }

                if (x == Metadata.player2Joystick) {
                    player2Connected = true;
                    continue;
                }

                if (Metadata.player1Joystick == -1) {
                    Debug.Log("Player 1 is connected to joystick number: " + x);
                    player1Connected = true;
                    Metadata.player1Joystick = x;
                }

                else if (Metadata.player2Joystick == -1) {
                    Debug.Log("Player 2 is connected to joystick number: " + x);
                    player2Connected = true;
                    Metadata.player2Joystick = x;

                }
            }
            connectedControllers++;
        }

        if (!player1Connected) {
            // if not connected, cannot be ready
            player1Ready = false;

            Metadata.player1Joystick = -1;
            Player1Text.text = "Player 1: Connect controller!";
        }

        else {
            if (!player1Ready) {
                Player1Text.text =  "Player 1: Press OPTIONS to start";
            }

            else {
                Player1Text.text = "Player 1: Ready!";
            }
        }

        if (!player2Connected) {
            player2Ready = false;

            Metadata.player2Joystick = -1;
            Player2Text.text = "Player 2: Connect controller!";
        }

        else {
            if (!player2Ready) {
                Player2Text.text = "Player 2: Press OPTIONS to start";
            }

            else {
                Player2Text.text = "Player 2: Ready!";
            }
        }



    }
}
