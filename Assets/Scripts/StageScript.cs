using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour {

    public BeatMapReader beatMapReader;
    public double timer = 0;
    public GameObject noteObject;
    public List<string> notes_str;

    void parseJson(string filePath)
    {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatMapReader = JsonUtility.FromJson<BeatMapReader>(beatMapJson);
        notes_str = new List<string>(beatMapReader.notes_str);
    }

    void getNoteByTime(double time)
    {
        int numNotes = notes_str.Count;
        for (int i=0; i<numNotes; i++)
        {
            string note = notes_str[i];
            string[] note_str = note.Split(',');
            double note_time;
            double.TryParse(note_str[0], out note_time);
            if (time > note_time)
            {
                GameObject newNote = GameObject.Instantiate(noteObject);
                newNote.GetComponent<Note>().time = note_time;
                newNote.GetComponent<Note>().key = note_str[1];
                notes_str.RemoveAt(i);
                return;
            }
        }
    }
	// Use this for initialization
	void Start () {
        parseJson("demo_level");
	}
	
	// Update is called once per frame
	void Update () {
        timer = Time.time;
        getNoteByTime(timer);
	}
}
