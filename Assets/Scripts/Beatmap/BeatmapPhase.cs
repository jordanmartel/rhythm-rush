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

    public double getStartTime()
    {
        return startTime;
    }

    public double getEndTime()
    {
        return endTime;
    }
}
