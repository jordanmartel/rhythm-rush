using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BeatMapString{

    public string beat_map_string_left;
    public string beat_map_string_right;
    public int bpm;
    public int beat_split;
    public double offset;

    public static BeatMapReader CreateFromJSON(string filepath)
    {
        // Load the Json string from given filepath
        string levelJson = Resources.Load<TextAsset>(filepath).text;
        return JsonUtility.FromJson<BeatMapReader>(levelJson);
    }
}
