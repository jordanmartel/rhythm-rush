using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BeatMapReader{
    public string[] notes_str;

    public static BeatMapReader CreateFromJSON(string filepath)
    {
        // Load the Json string from given filepath
        string levelJson = Resources.Load<TextAsset>(filepath).text;
        return JsonUtility.FromJson<BeatMapReader>(levelJson);
    }
}
