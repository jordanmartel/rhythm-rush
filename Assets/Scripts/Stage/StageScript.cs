using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class StageScript : MonoBehaviour {

    [Header("Beat Info")]
    public GameObject noteObject;
    public Dictionary<string, string> notes;
    public List<NoteScript> notesOnScreen;
    public int noteIndex;
    public int noteCreateIndex;
    public double nextBeatTime;
    public double playerOffset;
    public double beatInterval;
    public double noteTravelDistance;
    public int noteHitIndex;
    public double noteTravelSpeed;

    public GameObject hitBox;
    public GameObject feedbackText; 

    [Header("Input Materials")]
    public Material triangle;
    public Material circle;
    public Material square;
    public Material cross;
    public Material dUp;
    public Material dLeft;
    public Material dRight;
    public Material dDown;
    public int player;

    private float timer;
    private float successTimer;

    [Header("Controller Values")]
    private int joystick;
    private string previousButton;
    private float previousDpadHorizontal;
    private float previousDpadVertical;

    private Beatmap beatmap;
    public string placement = "left";

    void parseJson(string filePath)
    {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatmap = JsonConvert.DeserializeObject<Beatmap>(beatMapJson);

        if (placement == "left")
        {
            notes = beatmap.player1Notes;
        }
        else
        {
            notes = beatmap.player2Notes;
        }
    }

    double BeatInterval(int bpm, int beat_split)
    {
        return 60.0 / bpm / beat_split;
    }

    void createNote(string key)
    {
        Vector3 position = transform.position;
        GameObject newNote = Instantiate(noteObject, new Vector3(position.x,position.y + 3,position.z), new Quaternion(0,180,0,0));
        newNote.GetComponent<NoteScript>().key = key;
        newNote.GetComponent<NoteScript>().index = noteIndex;
        newNote.GetComponent<NoteScript>().placement = placement;
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(key);
        newNote.GetComponent<NoteScript>().feedback = feedbackText;
        newNote.SetActive(true);
    }
	// Use this for initialization
	void Start () {
        parseJson("creator_lvl");
        noteTravelSpeed = beatmap.bpm / 20;
        noteTravelDistance = 6;
        playerOffset = 0.05;
        nextBeatTime = beatmap.offset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(beatmap.bpm, beatmap.beat_split);


        if (player == 0)
        {
            joystick = PlayerObject.player1Joystick;
        }

        else
        {
            joystick = PlayerObject.player2Joystick;
        }
    }


    NoteScript getNoteAtIndex(int index)
    {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("note");
        //print(notes.Length);
        foreach (GameObject note in notes)
        {
            NoteScript noteScript = note.GetComponent<NoteScript>();
            if (noteScript.index == index && noteScript.placement == placement)
            {
                return noteScript;
            }
        }
        return null;
    }

    string stringToKey(string beat)
    {
        switch (beat)
        {
            case "triangle":
                return "joystick " + joystick + " button 3";
            case "circle":
                return "joystick " + joystick + " button 2";
            case "square":
                return "joystick " + joystick + " button 0";
            case "cross":
                return "joystick " + joystick + " button 1";

            // for dpad, use literal name to be worked with later
            default:
                return beat.ToString();
                
        }
    }

    Material stringToMesh(string key)
    {
        switch (key)
        {
            case "triangle":
                return triangle;
            case "circle":
                return circle;
            case "square":
                return square;
            case "cross":
                return cross;
            case "up":
                return dUp;
            case "left":
                return dLeft;
            case "down":
                return dDown;
            case "right":
                return dRight;
            default:
                return cross;
        }
    }

    // Update is called once per frame
    void Update () {
        int score = FindObjectOfType<BossScript>().dmg;
        int combo = FindObjectOfType<TeamAttack>().combo;
        timer = Time.time;
        TeamAttack teamAttackController = FindObjectOfType<TeamAttack>();
        bool teamAttack = teamAttackController.isActive;
        // Create beat
        if (teamAttack)
        {
            
            // Destroy all notes
            GameObject[] allNotes = GameObject.FindGameObjectsWithTag("note");
            foreach (GameObject note in allNotes)
            {
                Destroy(note);
            }
            if (Input.anyKeyDown)
            {
                int attack = teamAttackController.buildTeamAttack();
                if (attack != 0)
                {
                    score += attack;
                    teamAttack = false;
                    teamAttackController.Reset();
                }
            }
        }
        else
        {
            if (timer > nextBeatTime)
            {

                if (notes.ContainsKey((noteCreateIndex).ToString()))
                {

                    /*if (placement.Equals("right"))
                    {
                        Debug.Log("note index is: " + noteCreateIndex);
                        Debug.Log("note index % count is: " + noteCreateIndex % notes.Count);

                    }*/
                    string curBeat = notes[(noteCreateIndex).ToString()];
                    createNote(curBeat);
                    noteIndex++;
                }

                noteCreateIndex++;
                nextBeatTime = beatmap.offset + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
            }
            NoteScript headNote = getNoteAtIndex(noteHitIndex);
            if (headNote)
            {
                bool buttonPressed = false;


                // get the dpad axis orientation
                float dpadHorizontal = Input.GetAxis("Controller Axis-Joystick" + joystick + "-Axis7");
                float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + joystick + "-Axis8");

                // only mark the button as pressed if there has been a change since the last frame and axis is non 0
                if (dpadHorizontal != previousDpadHorizontal)
                {
                    previousDpadHorizontal = dpadHorizontal;
                    if (dpadHorizontal != 0)
                    {
                        buttonPressed = true;
                    }

                }

                //only mark the button as pressed if there has been a change since the last frame, and it is non 0
                if (dpadVertical != previousDpadVertical)
                {
                    previousDpadVertical = dpadVertical;
                    if (dpadVertical != 0)
                    {
                        buttonPressed = true;
                    }
                }

                // check if any of the joystick buttons have been pressed (circle, triangle, square, cross only)
                for (int i = 0; i < 3; i++)
                {
                    string currentButton = "joystick " + joystick + " button " + i;

                    if (Input.GetKeyDown(currentButton) && previousButton != currentButton)
                    {
                        buttonPressed = true;
                        previousButton = currentButton;
                        break;
                    }
                }

                // want to know when player has stopped pressing button. Do not want to allow player to simply hold down a button
                if (!buttonPressed)
                {
                    previousButton = "";
                }

                string keyToHit = stringToKey(headNote.key);

                if (headNote.canHit)
                {
                    if ((keyToHit.Equals("left") && dpadHorizontal == -1) ||
                            (keyToHit.Equals("right") && dpadHorizontal == 1) ||
                            (keyToHit.Equals("up") && dpadVertical == 1) ||
                            (keyToHit.Equals("down") && dpadVertical == -1) ||
                            (Input.GetKeyDown(keyToHit)))
                    {
                        //print("hit successfully");
                        noteHitIndex++;
                        score += headNote.destroyWithFeedback(hitBox, true);
                        if (score < 100)
                        {
                            combo = 0;
                        }
                        else
                        {
                            combo += 1;
                        }
                    }

                    else if (buttonPressed)
                    {
                        //print("note missed!");
                        noteHitIndex++;
                        score += headNote.destroyWithFeedback(hitBox, false);
                        combo = 0;
                    }
                }

                else if (headNote.canMiss)
                {

                    if (buttonPressed)
                    {
                        //print("note missed!");
                        noteHitIndex++;
                        score += headNote.destroyWithFeedback(hitBox, false);
                        combo = 0;
                    }
                }
            }
            if (combo >= 120)
            {
                teamAttack = true;
            }
        }
        FindObjectOfType<BossScript>().dmg = score;
        FindObjectOfType<TeamAttack>().combo = combo;
    }
}
