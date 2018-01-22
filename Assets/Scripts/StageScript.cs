using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour {

    public BeatMapString beatMapString;
    public double timer = 0;
    public GameObject noteObject;
    public List<string> notes;
    public int noteIndex;
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
    }
	// Use this for initialization
	void Awake () {
        parseJson("demo_level");
        noteTravelSpeed = 3;
        noteTravelDistance = 6;
        playerOffset = 0.1;
        print(noteTravelDistance / noteTravelSpeed);
        nextBeatTime = beatMapString.offset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(beatMapString.bpm, beatMapString.beat_split);
	}
	
	// Update is called once per frame
	void Update () {
        timer = Time.time;
        // Create beat
        if (timer > nextBeatTime)
        {
            string curBeat = notes[noteIndex % notes.Count];
            if (curBeat != "-" && nextBeatTime > 0)
            {
                createNote(curBeat);
            }
            noteIndex++;
            nextBeatTime = beatMapString.offset + playerOffset + noteIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
        }
	}
}
