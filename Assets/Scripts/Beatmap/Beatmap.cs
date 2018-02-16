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
    public double[] thresholds;

    public List<List<BeatmapPhase>> sections;

    public BeatmapPhase getPhase(int section, int phase)
    {
        return sections[section][phase];
    }


    public Beatmap(int bpm, int beat_split, double offset) {
        this.bpm = bpm;
        this.beat_split = beat_split;
        this.offset = offset;

        sections = new List<List<BeatmapPhase>>();

        sections.Add(new List<BeatmapPhase>());
        sections.Add(new List<BeatmapPhase>());
        sections.Add(new List<BeatmapPhase>());

    }



    public void addNote(int player, int index, String key) {
        /*
        if (player == 0) {
            addP1Note(index, key);
        } else {
            addP2Note(index, key);
        }


    }

  
    public void addP1Note(int index, String key) {
        /*
        if (player1Notes.ContainsKey(index.ToString())) {
            //player1Notes[index.ToString()] += "," + key;
        } else {
            player1Notes.Add(index.ToString(), key);
        }
        */
    }

    public void addP2Note(int index, String key) {
        /*
        if (player2Notes.ContainsKey(index.ToString())) {
            //player2Notes[index.ToString()] += "," + key;
        }
        else {
            player2Notes.Add(index.ToString(), key);
        }
        */
    }
    


}
