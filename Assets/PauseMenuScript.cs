using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour {

    public GameObject UIResume;
    public GameObject UIRestart;
    public GameObject UIQuit;

    public GameObject P1Resume;
    public GameObject P1Restart;
    public GameObject P1Quit;

    public GameObject P2Resume;
    public GameObject P2Restart;
    public GameObject P2Quit;

    public AudioSource SFXPlayer1;
    public AudioSource SFXPlayer2;
    public AudioSource SFXConfirm;

    public AudioSource musicPlayer;

    public GameObject PausePanel;

    private List<GameObject> UIOptionList = new List<GameObject>();
    private List<GameObject> P1Positions = new List<GameObject>();
    private List<GameObject> P2Positions = new List<GameObject>();

    public GameObject P1;
    public GameObject P2;
    public string currentScene;

    private int selectedJoystick;
    private bool paused = false;
    private int currentPosition = 0;
    private GameObject activeIndicator;
    private List<GameObject> activePositions;
    private AudioSource activeSFX;

    private bool resume=false;
    private bool restart=false;
    private bool quit=false;
    private bool optionSelected=false;
    private float confirmTimer = 1.0f;

    private bool dpadPushed = false;

    // Use this for initialization
    void Start () {

        UIOptionList.Add(UIResume);
        UIOptionList.Add(UIRestart);
        UIOptionList.Add(UIQuit);

        P1Positions.Add(P1Resume);
        P1Positions.Add(P1Restart);
        P1Positions.Add(P1Quit);

        P2Positions.Add(P2Resume);
        P2Positions.Add(P2Restart);
        P2Positions.Add(P2Quit);

    }

    void resetPauseMenu()
    {
        resume = false;
        restart = false;
        quit = false;
        optionSelected = false;
        confirmTimer = 1.0f;
        P2.SetActive(false);
        P1.SetActive(false);
        paused = false;
        Time.timeScale = 1.0f;
        musicPlayer.Play();
        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        if (optionSelected)
        {
            if (confirmTimer <= 0)
            {
                // resume
                if (resume)
                {

                    resetPauseMenu();
                }

                else if (restart)
                {

                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene(currentScene);
                }

                else
                {

                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene("ConnectController");

                }
            }
            else
            {
                confirmTimer = confirmTimer - Time.unscaledDeltaTime;
            }
        }

        if (!paused)
        {
            //player 1 pauses
            if (Input.GetKeyDown("joystick " + Metadata.player1Joystick + " button 9"))
            {

                Time.timeScale = 0f;


                selectedJoystick = Metadata.player1Joystick;

                PausePanel.SetActive(true);
                P1.SetActive(true);
                paused = true;
                activeIndicator = P1;
                activePositions = P1Positions;
                activeSFX = SFXPlayer1;

                musicPlayer.Pause();



            }

            //player2 pauses
            else if (Input.GetKeyDown("joystick " + Metadata.player2Joystick + " button 9"))
            {

                Time.timeScale = 0f;

                selectedJoystick = Metadata.player2Joystick;

                PausePanel.SetActive(true);
                P2.SetActive(true);
                paused = true;
                activeIndicator = P2;
                activePositions = P2Positions;
                activeSFX = SFXPlayer2;



                musicPlayer.Pause();


            }
        }

        else
        {
            float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + selectedJoystick + "-Axis8");

            if (dpadPushed && dpadVertical == 0)
            {
                dpadPushed = false;
            }

            // only check the dpad if it has not already been pushed
            if (!dpadPushed)
            {

                if (dpadVertical == -1 && currentPosition < UIOptionList.Count - 1)
                {
                    dpadPushed = true;
                    currentPosition++;
                    activeIndicator.GetComponent<RectTransform>().transform.position = new Vector3(activePositions[currentPosition].transform.position.x, P1Positions[currentPosition].transform.position.y, 0);
                    activeSFX.Play();
                }

                else if (dpadVertical == 1 && currentPosition > 0)
                {
                    dpadPushed = true;
                    currentPosition--;
                    activeIndicator.GetComponent<RectTransform>().transform.position = new Vector3(activePositions[currentPosition].transform.position.x, P1Positions[currentPosition].transform.position.y, 0);
                    activeSFX.Play();

                }
            }           


            // player selected an option
            if (Input.GetKeyDown("joystick " + selectedJoystick + " button 1")) {

                optionSelected = true;

                // resume
                if (currentPosition == 0)
                {

                    SFXConfirm.Play();
                    resume = true;

                }

                else if (currentPosition == 1)
                {
                    SFXConfirm.Play();
                    restart = true;
                }

                else
                {
                    SFXConfirm.Play();
                    quit = true;
                }

            }
        }
    }
}
