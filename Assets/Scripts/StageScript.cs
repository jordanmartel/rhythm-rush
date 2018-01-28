using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour {

    public BeatMapString beatMapString;
    public GameObject noteObject;
    public List<string> notes;
    public List<NoteScript> notesOnScreen;
    public int noteIndex;
    public int noteCreateIndex;
    public int noteHitIndex;
    public double nextBeatTime;
    public double playerOffset;
    public double beatInterval;
    public double noteTravelSpeed;
    public double noteTravelDistance;

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
    private int joystick;

    public string placement = "left";

    void parseJson(string filePath)
    {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatMapString = JsonUtility.FromJson<BeatMapString>(beatMapJson);
        string[] notes_array;
        if (placement == "left")
        {
            notes_array = beatMapString.beat_map_string_left.Split(',');
        }
        else
        {
            notes_array = beatMapString.beat_map_string_right.Split(',');
        }
        
        notes = new List<string>(notes_array);
    }

    double BeatInterval(int bpm, int beat_split)
    {
        return 60.0 / bpm / beat_split;
    }

    void createNote(string key)
    {
        float x;
        if (placement == "left")
        {
            x = -2;
        }
        else
        {
            x = 2;
        }
        GameObject newNote = Instantiate(noteObject, new Vector3(x,3,0), new Quaternion(0,180,0,0));
        newNote.GetComponent<NoteScript>().key = key;
        newNote.GetComponent<NoteScript>().index = noteIndex;
        newNote.GetComponent<NoteScript>().placement = placement;
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(key);
    }
	// Use this for initialization
	void Start () {
        parseJson("demo_level");
        noteTravelSpeed = beatMapString.bpm / 20;
        noteTravelDistance = 6;
        playerOffset = 0.05;
        nextBeatTime = beatMapString.offset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(beatMapString.bpm, beatMapString.beat_split);


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

    string stringToKey(string beat_string)
    {
        switch (beat_string)
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
                return beat_string;
                
        }
    }

    Material stringToMesh(string beat_string)
    {
        switch (beat_string)
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

        timer += Time.deltaTime;
        // Create beat
        if (timer > nextBeatTime)
        {
            string curBeat = notes[noteCreateIndex % notes.Count];
            if (curBeat != "-")
            {
                createNote(curBeat);
                noteIndex++;
            }
            noteCreateIndex++;
            nextBeatTime = beatMapString.offset + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
        }
        NoteScript headNote = getNoteAtIndex(noteHitIndex);
        if (headNote)
        {
            // check if any of the joystick buttons have been pressed (circle, triangle, square, cross only)
            bool buttonPressed = false;
            for (int i = 0; i < 3; i++)
            {
                if (Input.GetKeyDown("joystick " + joystick + " button " + i)) {
                    buttonPressed = true;
                }
            }

            string keyToHit = stringToKey(headNote.key);

            float dpadHorizontal = Input.GetAxis("Controller Axis-Joystick" + joystick + "-Axis7");
            float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + joystick + "-Axis8");


            if (headNote.canHit)
            {            
                if ((keyToHit.Equals("left") && dpadHorizontal == -1) ||
                        (keyToHit.Equals("right") && dpadHorizontal == 1) ||
                        (keyToHit.Equals("up") && dpadVertical == 1) ||
                        (keyToHit.Equals("down") && dpadVertical == -1) ||
                        (Input.GetKeyDown(keyToHit)))
                {
                    print("hit successfully");
                    noteHitIndex++;
                    Destroy(headNote.gameObject);

                }

            }

            else if (headNote.canMiss)
            {
                if (buttonPressed || dpadHorizontal != 0 || dpadVertical != 0)
                {
                    print("note missed!");
                    noteHitIndex++;
                    Destroy(headNote.gameObject);
                }
            }
        }
    }
}
