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

    void parseJson(string filePath)
    {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatMapString = JsonUtility.FromJson<BeatMapString>(beatMapJson);
        string [] notes_array = beatMapString.beat_map_string.Split(',');
        notes = new List<string>(notes_array);
    }

    double BeatInterval(int bpm, int beat_split)
    {
        return 60.0 / bpm / beat_split;
    }

    void createNote(string key)
    {
        GameObject newNote = Instantiate(noteObject, new Vector3(0,3,0), Quaternion.identity);
        newNote.GetComponent<NoteScript>().key = key;
        newNote.GetComponent<NoteScript>().index = noteIndex;
    }
	// Use this for initialization
	void Awake () {
        parseJson("demo_level");
        noteTravelSpeed = beatMapString.bpm / 20;
        noteTravelDistance = 6;
        playerOffset = -.05;
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
            case "0":
                return KeyCode.Mouse0;
            default:
                return KeyCode.Mouse0;
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
            else if (Input.anyKeyDown)
            {
                print("miss");
                noteHitIndex++;
                Destroy(headNote.gameObject);
            }
        }
    }
}
