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

        this.thresholds = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };

        sections = new List<List<BeatmapPhase>>();

    }
}
