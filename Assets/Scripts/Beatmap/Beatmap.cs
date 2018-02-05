using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[Serializable]
public class Beatmap
{
    public int bpm;
    public int beat_split;
    public double offset;

    public Dictionary<string, string> player1Notes = null;
    public Dictionary<string, string> player2Notes = null;

    public Beatmap(int bpm, int beat_split, double offset) {
        this.bpm = bpm;
        this.beat_split = beat_split;
        this.offset = offset;

        player1Notes = new Dictionary<string, string>();
        player2Notes = new Dictionary<string, string>();
    }

    public void addP1Note(int index, String key) {
        player1Notes.Add(index.ToString(), key);
    }

    public void addP2Note(int index, String key) {
        player2Notes.Add(index.ToString(), key);
    }

    

}
