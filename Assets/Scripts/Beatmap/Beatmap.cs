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
    }

    public void addP1Note(int index, String key) {
        if (player1Notes != null) {
            player1Notes.Add(index.ToString(), key);
        } else {
            player1Notes = new Dictionary<string, string>();
        }
    }

    public void addP2Note(int index, String key) {
        if (player2Notes != null) {
            player2Notes.Add(index.ToString(), key);
        } else {
            player2Notes = new Dictionary<string, string>();
        }
    }
}
