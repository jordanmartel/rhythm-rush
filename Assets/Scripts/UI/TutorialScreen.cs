using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialScreen : MonoBehaviour {

    public string firstStage = "PickPowerScene";

    public Image currentScreen;
    public Sprite firstScreen;
    public Sprite secondScreen;
    public Sprite thirdScreen;
    public Sprite fourthScreen;
    public Sprite fifthScreen;
    public Sprite sixthScreen;
    public Sprite seventhScreen;
    public Sprite eigthScreen;

    public AudioSource audioSource;
    public AudioClip confirmClip;
    public AudioClip nextSound;
    public AudioClip previousSound;


    private List<Sprite> tutorialScreens = new List<Sprite>();
    private int position = 0;


	// Use this for initialization
	void Start () {

        tutorialScreens.Add(firstScreen);
        tutorialScreens.Add(secondScreen);
        tutorialScreens.Add(thirdScreen);
        tutorialScreens.Add(fourthScreen);
        tutorialScreens.Add(fifthScreen);
        tutorialScreens.Add(sixthScreen);
        tutorialScreens.Add(seventhScreen);
        tutorialScreens.Add(eigthScreen);
    }


    IEnumerator playConfirmClip()
    {
        audioSource.clip = confirmClip;
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        SceneManager.LoadScene(firstStage);
    }

    // Update is called once per frame
    void Update () {

        // exit
        if (Input.GetKeyDown("joystick " + Metadata.player1Joystick + " button 1") || Input.GetKeyDown("joystick " + Metadata.player2Joystick + " button 1"))
        {
            StartCoroutine(playConfirmClip());

        }

        // square/except that it's circle --> next button
        else if ((Input.GetKeyDown("joystick " + Metadata.player1Joystick + " button 2") || Input.GetKeyDown("joystick " + Metadata.player2Joystick + " button 2"))) {

            // any position except the final position
            if (position < tutorialScreens.Count - 1)
            {
                
                position++;
                currentScreen.sprite = tutorialScreens[position];
                audioSource.clip = previousSound;
                audioSource.Play();

            }
        }

        // circle/except that it's square --> back button
        else if ((Input.GetKeyDown("joystick " + Metadata.player1Joystick + " button 0") || Input.GetKeyDown("joystick " + Metadata.player2Joystick + " button 0"))) {

            // any position except the first position
            if (position > 0)
            {
                position--;
                currentScreen.sprite = tutorialScreens[position];
                audioSource.clip = nextSound;
                audioSource.Play();

            }
        }
    }
}
