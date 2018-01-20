using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Beatmap
{

    public Note[] notes;

    // go through all the notes and find the next note that has not happened yet
    Note GetNextNote(float time)
    {
        for (int i = 0; i < notes.Length; i ++) {

            if (notes[i].time > time)
            {
                return notes[i];
            }
        }
        return null;
    }
}
