using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectController : MonoBehaviour {

    public Texture background;

    private int connectedControllers = 0;

    private bool player1Ready = false;
    private bool player2Ready = false;


    // unity is super gross and randomly assigns a controller to a joystick. Will have to manually 
    // keep track of which joystick buttons belong to each player

    private int player1Joystick = -1;
    private int player2Joystick = -1;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {



        for (int i = 0; i < 20; i++)
        {

            if (player1Joystick != -1)
            {
                if (Input.GetKeyDown("joystick " + player1Joystick + " button " + i))
                {
                    Debug.Log("player 1 button " + i);
                }
            }

            if (player2Joystick != -1)
            {
                if (Input.GetKeyDown("joystick " + player2Joystick + " button " + i))
                {
                    Debug.Log("player 2 button " + i);
                }

            }
        }




        if (player1Joystick != -1)
        {
            // player1 has pressed the PS4 Options button (joystick button 9)
            if (Input.GetKeyDown("joystick " + player1Joystick + " button 9"))
            {
                player1Ready = !player1Ready;
            }
        }

        if (player2Joystick != -1)
        {
            // player2 has pressed the PS4 Options button (joystick button 9)
            if (Input.GetKeyDown("joystick " + player2Joystick + " button 9"))
            {
                player2Ready = !player2Ready;
            }

        }

        if (player1Ready && player2Ready)
        {
            SceneManager.LoadScene("BaseScene_Annie");
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
            if (controllerNames[x - 1].Equals("Wireless Controller"))
            {

                if (x == player1Joystick)
                {
                    player1Connected = true;
                    continue;
                }

                if (x == player2Joystick)
                {
                    player2Connected = true;
                    continue;
                }

                if (player1Joystick == -1)
                {
                    Debug.Log("Player 1 is connected to joystick number: " + x);
                    player1Connected = true;
                    player1Joystick = x;
                }

                else if (player2Joystick == -1)
                {
                    Debug.Log("Player 2 is connected to joystick number: " + x);
                    player2Connected = true;
                    player2Joystick = x;

                }
            }
            connectedControllers++;
        }

        if (!player1Connected)
        {
            // if not connected, cannot be ready
            player1Ready = false;
            player1Joystick = -1;
            GUI.Label(new Rect(10, 10, 250, 20), "Player 1: Connect controller!");
        }

        else 
        {
            if (!player1Ready)
            {
                GUI.Label(new Rect(10, 10, 250, 20), "Player 1: Press OPTIONS to start");
            }

            else
            {
                GUI.Label(new Rect(10, 10, 250, 20), "Player 1: Ready!");
            }
        }

        if (!player2Connected)
        {
            player2Ready = false;
            player2Joystick = -1;
            GUI.Label(new Rect(Screen.width - 210, 10, 250, 20), "Player 2: Connect controller!");
        }

        else
        {
            if (!player2Ready)
            {
                GUI.Label(new Rect(Screen.width - 210, 10, 250, 20), "Player 2: Press OPTIONS to start");
            }

            else
            {
                GUI.Label(new Rect(Screen.width - 210, 10, 250, 20), "Player 2: Ready!");
            }
        }

    }
}
