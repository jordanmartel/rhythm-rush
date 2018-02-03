using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[Serializable]
public class Beatmap
{
    public int bpm;
    public int beat_split;
    public int offset;

    public Dictionary<string, string> player1Notes;
    public Dictionary<string, string> player2Notes;
}
