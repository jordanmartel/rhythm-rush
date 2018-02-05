using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreatorStage : MonoBehaviour {

    [Header("Beat Info")]
    public GameObject noteObject;
    public Dictionary<string, string> notes;
    private int maxBeat;
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
    private float score;

    private int joystick;

    private string previousButton;
    private float previousDpadHorizontal;
    private float previousDpadVertical;

    private Beatmap recordedNotes;
    private Beatmap beatmap;

    [Header("CreatorMode")]
    public string beatmap_filePath;

    public string placement = "left";


    void parseJson(string filePath) {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatmap = JsonConvert.DeserializeObject<Beatmap>(beatMapJson);

        if (placement == "left") {
            notes = beatmap.player1Notes;
            maxBeat = 2;
        }
        else {
            notes = beatmap.player2Notes;
            maxBeat = 8;
        }
    }

    double BeatInterval(int bpm, int beat_split) {
        return 60.0 / bpm / beat_split;
    }

    void createNote(string key) {
        Vector3 position = transform.position;
        GameObject newNote = Instantiate(noteObject, new Vector3(position.x, position.y -3, position.z), new Quaternion(0, 180, 0, 0));
        newNote.GetComponent<NoteScript>().key = key;
        newNote.GetComponent<NoteScript>().index = noteIndex;
        newNote.GetComponent<NoteScript>().placement = placement;
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(key);
        newNote.GetComponent<NoteScript>().feedback = feedbackText;
        newNote.GetComponent<NoteScript>().isCreator = true;
        newNote.SetActive(true);
    }
    // Use this for initialization
    void Start() {

        double bmsoffset = 5;
        int bmsbpm = 142;
        int beat_split = 2;

        noteTravelSpeed = 142 / 20;
        noteTravelDistance = 6;
        playerOffset = 0.05;
        nextBeatTime = bmsoffset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(bmsbpm, beat_split);


        if (player == 0) {
            joystick = PlayerObject.player1Joystick;
        }

        else {
            joystick = PlayerObject.player2Joystick;
        }

        recordedNotes = new Beatmap((int)bmsoffset, beat_split, bmsoffset);

    }

    string stringToKey(string beat_string) {
        switch (beat_string) {
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

    Material stringToMesh(string beat_string) {
        switch (beat_string) {
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
    void Update() {

        timer += Time.deltaTime;
        // Create beat
        if (timer > nextBeatTime) { 

            noteIndex++;
            //5 is offset
            nextBeatTime = 5 + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
        }


            bool buttonPressed = false;

        // get the dpad axis orientation
        float dpadHorizontal = Input.GetAxis("Controller Axis-Joystick" + joystick + "-Axis7");
        float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + joystick + "-Axis8");

        // only mark the button as pressed if there has been a change since the last frame and axis is non 0
        if (dpadHorizontal != previousDpadHorizontal) {
            previousDpadHorizontal = dpadHorizontal;
            if (dpadHorizontal != 0) {
                buttonPressed = true;
            }

        }

        //only mark the button as pressed if there has been a change since the last frame, and it is non 0
        if (dpadVertical != previousDpadVertical) {
            previousDpadVertical = dpadVertical;
            if (dpadVertical != 0) {
                buttonPressed = true;
            }
        }

        // check if any of the joystick buttons have been pressed (circle, triangle, square, cross only)
        for (int i = 0; i < 3; i++) {
            string currentButton = "joystick " + joystick + " button " + i;

            if (Input.GetKeyDown(currentButton) && previousButton != currentButton) {
                buttonPressed = true;
                previousButton = currentButton;
                break;
            }
        }

        // want to know when player has stopped pressing button. Do not want to allow player to simply hold down a button
        if (!buttonPressed) {
            previousButton = "";
        }


        //Triangle,Circle,Square, Cross order
        Note nwNote = new Note();

        if (Input.GetKeyDown("joystick " + joystick + " button 3")) {
            nwNote.key = "triangle";
            recordedNotes.addP1Note(noteIndex, nwNote.key);
            createNote(nwNote.key);
            Debug.Log("Index:" + noteIndex);

        } else if (Input.GetKeyDown("joystick " + joystick + " button 2")) {
            nwNote.key = "circle";
            recordedNotes.addP1Note(noteIndex, nwNote.key);
            createNote(nwNote.key);
            Debug.Log("Index:" + noteIndex);
        } else if (Input.GetKeyDown("joystick " + joystick + " button 0")) {
            nwNote.key = "square";
            recordedNotes.addP1Note(noteIndex, nwNote.key);
            createNote(nwNote.key);
            Debug.Log("Index:" + noteIndex);
        } else if (Input.GetKeyDown("joystick " + joystick + " button 1")) {
            nwNote.key = "cross";
            recordedNotes.addP1Note(noteIndex, nwNote.key);
            createNote(nwNote.key);
            Debug.Log("Index:" + noteIndex);
        } else if (Input.GetKeyDown(KeyCode.Space)) {

            //Stops time and writes notes to file 
            Time.timeScale = 0.05f;
            foreach (KeyValuePair<string,string> entry in recordedNotes.player1Notes) {
                Debug.Log("Found Note:" + entry.Value + "At Index " + entry.Key);
            }

            WriteCreatedBeat();
            
        }
    }

    private void WriteCreatedBeat() {
        TextWriter writer = null;
        try {
            var contentsToWriteToFile = JsonConvert.SerializeObject(recordedNotes);
            writer = new StreamWriter(beatmap_filePath);
            writer.Write(contentsToWriteToFile);
        }
        finally {
            if (writer != null)
                writer.Close();
        }
    }

}
