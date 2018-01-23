using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour {

    public BeatMapString beatMapString;
    public double timer = 0;
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

    public Material meshW;
    public Material meshA;
    public Material meshS;
    public Material meshD;
    public Material meshUp;
    public Material meshLeft;
    public Material meshRight;
    public Material meshDown;

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
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(key);
    }
	// Use this for initialization
	void Awake () {
        parseJson("demo_level");
        noteTravelSpeed = beatMapString.bpm / 20;
        noteTravelDistance = 6;
        playerOffset = -.0;
        nextBeatTime = beatMapString.offset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(beatMapString.bpm, beatMapString.beat_split);
	}

    NoteScript getNoteAtIndex(int index)
    {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("note");
        print(notes.Length);
        foreach (GameObject note in notes)
        {
            NoteScript noteScript = note.GetComponent<NoteScript>();
            if (noteScript.index == index)
            {
                return noteScript;
            }
        }
        return null;
    }

    KeyCode stringToKey(string beat_string)
    {
        switch (beat_string)
        {
            case "w":
                return KeyCode.W;
            case "a":
                return KeyCode.A;
            case "s":
                return KeyCode.S;
            case "d":
                return KeyCode.D;
            case "up":
                return KeyCode.UpArrow;
            case "left":
                return KeyCode.LeftArrow;
            case "down":
                return KeyCode.DownArrow;
            case "right":
                return KeyCode.RightArrow;
            default:
                return KeyCode.W;
        }
    }

    Material stringToMesh(string beat_string)
    {
        switch (beat_string)
        {
            case "w":
                return meshW;
            case "a":
                return meshA;
            case "s":
                return meshS;
            case "d":
                return meshD;
            case "up":
                return meshUp;
            case "left":
                return meshLeft;
            case "down":
                return meshDown;
            case "right":
                return meshRight;
            default:
                return meshW;
        }
    }

    // Update is called once per frame
    void Update () {
        timer = Time.time;
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
            if (Input.GetKeyDown(stringToKey(headNote.key)) && headNote.canHit)
            {
                print("hit successfully");
                noteHitIndex++;
                Destroy(headNote.gameObject);
            }
            else if (placement == "left")
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                    Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    print("wasd miss");
                    noteHitIndex++;
                    Destroy(headNote.gameObject);
                }
            }
            else if (placement == "right")
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                    Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    print("arrow miss");
                    noteHitIndex++;
                    Destroy(headNote.gameObject);
                }
            }
        }
    }
}
