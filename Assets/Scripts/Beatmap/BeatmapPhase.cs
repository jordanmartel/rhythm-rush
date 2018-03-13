using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class BeatmapPhase
{
    public double startTime;
    public double endTime;
    public double offset;
    public Dictionary<string, string> player1Notes;
    public Dictionary<string, string> player2Notes;
    public Dictionary<string, string> bothPlayerNotes;
    public Dictionary<string, string> revivalNotes;

    public BeatmapPhase()
    {
        player1Notes = new Dictionary<string, string>();
        player2Notes = new Dictionary<string, string>();
        bothPlayerNotes = new Dictionary<string, string>();
        revivalNotes = new Dictionary<string, string>();
    }

public double getStartTime()
    {
        return startTime;
    }

    public double getEndTime()
    {
        return endTime;
    }


    public void addNote(int player, int index, String key)
    {
        if (player == 0) {
            addP1Note(index, key);
        } else {
            addP2Note(index, key);
        }
    }

    public void addP1Note(int index, String key)
    {
        if (player1Notes.ContainsKey(index.ToString())) {
            //player1Notes[index.ToString()] += "," + key;
        } else {
            player1Notes.Add(index.ToString(), key);
        }
    }

    public void addP2Note(int index, String key) {
        
        if (player2Notes.ContainsKey(index.ToString())) {
            //player2Notes[index.ToString()] += "," + key;
        }

        else {
            player2Notes.Add(index.ToString(), key);
        }
        
    }
}
