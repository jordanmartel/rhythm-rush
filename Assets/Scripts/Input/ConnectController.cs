using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectController : MonoBehaviour {

    public Texture background;

    private int connectedControllers = 0;
    public AudioClip confirm;
    public AudioClip cancel;
    public AudioSource SFXPlayer1;
    public AudioSource SFXPlayer2;

    public string firstStage;

    private bool player1Ready = false;
    private bool player2Ready = false;
    GUIStyle guiStyle = new GUIStyle();

    // unity is super gross and randomly assigns a controller to a joystick. Will have to manually 
    // keep track of which joystick buttons belong to each player

    // Use this for initialization
    void Start() {

        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.white;

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
                    SFXPlayer1.Play();
                }
                else
                {
                    SFXPlayer1.clip = cancel;
                    SFXPlayer1.Play();
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
                    SFXPlayer2.Play();
                }
                else
                {
                    SFXPlayer2.clip = cancel;
                    SFXPlayer2.Play();
                }
            }

        }

        if (player1Ready && player2Ready || Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(firstStage);
        }


    }


    private void OnGUI()
    {

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        bool player1Connected = false;
        bool player2Connected = false;

        connectedControllers = 0;
        string[] controllerNames = Input.GetJoystickNames();
        for (int x = 1; x < Input.GetJoystickNames().Length + 1; x++)
        {
            if (controllerNames[x - 1].Equals("Wireless Controller") || controllerNames[x-1].Contains("Xbox One"))
            {

                if (x == Metadata.player1Joystick)
                {
                    player1Connected = true;
                    continue;
                }

                if (x == Metadata.player2Joystick)
                {
                    player2Connected = true;
                    continue;
                }

                if (Metadata.player1Joystick == -1)
                {
                    Debug.Log("Player 1 is connected to joystick number: " + x);
                    player1Connected = true;
                    Metadata.player1Joystick = x;
                }

                else if (Metadata.player2Joystick == -1)
                {
                    Debug.Log("Player 2 is connected to joystick number: " + x);
                    player2Connected = true;
                    Metadata.player2Joystick = x;

                }
            }
            connectedControllers++;
        }

        if (!player1Connected)
        {
            // if not connected, cannot be ready
            player1Ready = false;

            Metadata.player1Joystick = -1;
            GUI.Label(new Rect(10, 10, 250, 20), "Player 1: Connect controller!", guiStyle);
        }

        else 
        {
            if (!player1Ready)
            {
                GUI.Label(new Rect(10, 10, 250, 20), "Player 1: Press OPTIONS to start", guiStyle);
            }

            else
            {
                GUI.Label(new Rect(10, 10, 250, 20), "Player 1: Ready!", guiStyle);
            }
        }

        if (!player2Connected)
        {
            player2Ready = false;

            Metadata.player2Joystick = -1;
            GUI.Label(new Rect(Screen.width - 210, 10, 250, 20), "Player 2: Connect controller!", guiStyle);

        }

        else
        {
            if (!player2Ready)
            {
                GUI.Label(new Rect(Screen.width - 320, 10, 250, 20), "Player 2: Press OPTIONS to start", guiStyle);
            }

            else
            {
                GUI.Label(new Rect(Screen.width - 320, 10, 250, 20), "Player 2: Ready!", guiStyle);
            }
        }

    }
}
