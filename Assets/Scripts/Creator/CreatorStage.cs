using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CreatorStage : MonoBehaviour {

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
    private float score;

    private int joystick;

    private string previousButton;
    private float previousDpadHorizontal;
    private float previousDpadVertical;
    private double previousTime;

    private Beatmap recordedNotes;
    private Beatmap beatmap;

    [Header("CreatorMode")]
    public string beatmap_filePath;
    public double offset;
    public int bpm;
    public int beat_split;
    public Text metadataBox;
    public Text recordingSavedText;

    public string placement = "left";


    void parseJson(string filePath) {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatmap = JsonConvert.DeserializeObject<Beatmap>(beatMapJson);

        if (placement == "left") {
            notes = beatmap.player1Notes;
        }
        else {
            notes = beatmap.player2Notes;
        }
    }

    double BeatInterval(int bpm, int beat_split) {
        return 60.0 / bpm / beat_split;
    }

    void createNote(string key) {
        Vector3 position = transform.position;
        GameObject newNote = Instantiate(noteObject, new Vector3(position.x, position.y -4, position.z), new Quaternion(0, 180, 0, 0));
        newNote.GetComponent<NoteScript>().key = key;
        newNote.GetComponent<NoteScript>().index = noteIndex;
        newNote.GetComponent<NoteScript>().stage = gameObject;
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(key);
        newNote.GetComponent<NoteScript>().feedback = feedbackText;
        newNote.GetComponent<NoteScript>().isCreator = true;
        newNote.SetActive(true);
    }
    // Use this for initialization
    void Start() {

        noteTravelSpeed = 140 / 20;
        noteTravelDistance = 6;
        playerOffset = 0.05;
        nextBeatTime = offset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(bpm, beat_split);


        if (player == 0) {
            joystick = Joysticks.player1Joystick;
        }

        else {
            joystick = Joysticks.player2Joystick;
        }

        recordedNotes = new Beatmap((int)bpm, beat_split, offset);

        // Only change text if not changed by other script already
        if (metadataBox.text == "Beatmap Specifics:") {
            metadataBox.text += "\nOffset:" + offset
                + "\nbpm:" + bpm
                + "\nBeatTravel Speed:" + noteTravelSpeed
                + "\nBeat travel distance:" + noteTravelDistance
                + "\nsaveLocation:" + Application.dataPath + "/Scripts/Resources/" + beatmap_filePath;
        }
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

        // want to know when player has stopped pressing button. Do not want to allow player to simply hold down a button
        if (!buttonPressed) {
            previousButton = "";
        }

        //Directional buttons
        if (buttonPressed) {
            if (dpadHorizontal == -1) {
                recordedNotes.addNote(player, noteIndex - 1, "left");
                createNote("left");
                Debug.Log("Index:" + noteIndex);
            }
            else if (dpadHorizontal == 1) {
                recordedNotes.addNote(player, noteIndex - 1, "right");
                createNote("right");
                Debug.Log("Index:" + noteIndex);
            }
            else if (dpadVertical == 1) {
                recordedNotes.addNote(player, noteIndex - 1, "up");
                createNote("up");
                Debug.Log("Index:" + noteIndex);
            }
            else if (dpadVertical == -1) {
                recordedNotes.addNote(player, noteIndex - 1, "down");
                createNote("down");
                Debug.Log("Index:" + noteIndex);
            }
        }

        //Triangle,Circle,Square, Cross order
         if (Input.GetKeyDown("joystick " + joystick + " button 3")) {
            recordedNotes.addNote(player, noteIndex - 1, "triangle");
            createNote("triangle");
            Debug.Log("Index:" + noteIndex +","+ "triangle");
        } else if (Input.GetKeyDown("joystick " + joystick + " button 2")) {
            recordedNotes.addNote(player, noteIndex - 1, "circle");
            createNote("circle");
            Debug.Log("Index:" + noteIndex);
        } else if (Input.GetKeyDown("joystick " + joystick + " button 0")) {
            recordedNotes.addNote(player, noteIndex - 1, "square");
            createNote("square");
            Debug.Log("Index:" + noteIndex);
        } else if (Input.GetKeyDown("joystick " + joystick + " button 1")) {
            recordedNotes.addNote(player, noteIndex - 1, "cross");
            createNote("cross");
            Debug.Log("Index:" + noteIndex);

        //Finalize Generated Beat
        } else if (Input.GetKeyDown(KeyCode.Space)) {

            //Stops time and writes notes to file 
            Time.timeScale = 0.05f;

            WriteCreatedBeat();
            
        }


        timer += Time.deltaTime;
        // Create beat
        if (timer > nextBeatTime) {

            Debug.Log("note Index:" + noteIndex);

            noteIndex++;
            previousTime = nextBeatTime;
            nextBeatTime = offset + playerOffset + noteIndex * beatInterval;
        }

    }

    private void WriteCreatedBeat() {
        TextWriter writer = null;
        try {
            recordingSavedText.text += "...";

            var contentsToWriteToFile = JsonConvert.SerializeObject(recordedNotes);
            writer = new StreamWriter(Application.dataPath + "/Scripts/Resources/" + beatmap_filePath);
            writer.Write(contentsToWriteToFile);
        } catch (Exception e) {
            recordingSavedText.text += "error";
            Debug.Log(e.Message);
        }
        finally {
            Debug.Log("done");
            if (writer != null)
                writer.Close();

            recordingSavedText.text += " saved!";
        }

    }

}
