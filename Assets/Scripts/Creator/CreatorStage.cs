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

    public float timer;
    public float phaseTimer;
    private int joystick;

    private string previousButton;
    private float previousDpadHorizontal;
    private float previousDpadVertical;
    private double previousTime;

    private Beatmap recordedNotes;
    private BeatmapPhase currentPhase;

    public int section;
    public int phase;

    [Header("CreatorMode")]
    public string beatmap_filePath;
    public double offset;
    public double noteSpeedQuotient = 20;
    public double phaseOffset = 3;
    public int bpm;
    public int beat_split;
    public Text metadataBox;
    public Text recordingSavedText;

    public string placement = "left";

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

        noteTravelSpeed = bpm / noteSpeedQuotient;
        noteTravelDistance = 11.1;
        playerOffset = -0.2;
        nextBeatTime = 0;
        beatInterval = BeatInterval(bpm, beat_split);

        if (player == 0) {
            joystick = Joysticks.player1Joystick;
        }

        else {
            joystick = Joysticks.player2Joystick;
        }

        recordedNotes = new Beatmap((int)bpm, beat_split, offset);
        currentPhase = new BeatmapPhase();
        currentPhase.offset = phaseOffset;
        currentPhase.startTime = timer;

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
        timer += Time.deltaTime;
        phaseTimer += Time.deltaTime;

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
                currentPhase.addNote(0, noteIndex, "square");
                createNote("circle");
                Debug.Log("Index:" + noteIndex);
            }
            else if (dpadHorizontal == 1) {
                currentPhase.addNote(0, noteIndex, "circle");
                createNote("square");
                Debug.Log("Index:" + noteIndex);
            }
            else if (dpadVertical == 1) {
                currentPhase.addNote(0, noteIndex, "triangle");
                createNote("triangle");
                Debug.Log("Index:" + noteIndex);
            }
            else if (dpadVertical == -1) {
                currentPhase.addNote(0, noteIndex, "cross");
                createNote("cross");
                Debug.Log("Index:" + noteIndex);
            }
        }

        //Triangle,Circle,Square, Cross order
         if (Input.GetKeyDown("joystick " + joystick + " button 3")) {
            currentPhase.addNote(1, noteIndex, "triangle");
            createNote("triangle");
            Debug.Log("Index:" + noteIndex +","+ "triangle");
        } else if (Input.GetKeyDown("joystick " + joystick + " button 2")) {
            currentPhase.addNote(1, noteIndex, "circle");
            createNote("circle");
            Debug.Log("Index:" + noteIndex);
        } else if (Input.GetKeyDown("joystick " + joystick + " button 0")) {
            currentPhase.addNote(1, noteIndex, "square");
            createNote("square");
            Debug.Log("Index:" + noteIndex);
        } else if (Input.GetKeyDown("joystick " + joystick + " button 1")) {
            currentPhase.addNote(1, noteIndex, "cross");
            createNote("cross");
            Debug.Log("Index:" + noteIndex);

        //Finalize Generated Beat
        }

         // next phase
        else if (Input.GetKeyDown("joystick " + joystick + " button 7"))
        {
            currentPhase.endTime = timer;
            recordedNotes.sections[section].Insert(phase, currentPhase);
            currentPhase = new BeatmapPhase();
            currentPhase.offset = phaseOffset;
            currentPhase.startTime = timer;
            phase++;

            noteIndex = 0;
            phaseTimer = 0;
            nextBeatTime = beatInterval;

            Debug.Log("next phase: note index is now reset " + noteIndex);
        }

        // done
        else if (Input.GetKeyDown(KeyCode.Space)) {

            currentPhase.endTime = timer;
            recordedNotes.sections[section].Insert(phase, currentPhase);

            //Stops time and writes notes to file 
            Time.timeScale = 0.05f;

            WriteCreatedBeat();
            
        }


        // Create beat
        if (phaseTimer > nextBeatTime) {

            //Debug.Log("note Index:" + noteIndex);
            if (phaseTimer > phaseOffset)
            {
                noteIndex++;
            }
            previousTime = nextBeatTime;
            nextBeatTime = nextBeatTime + beatInterval;
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
