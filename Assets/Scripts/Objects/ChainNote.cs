using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ChainNote
{
    public NoteScript player1Note;
    public NoteScript player2Note;

    public string player1Status;
    public string player2Status;

    public ChainNote(NoteScript player1Note, NoteScript player2Note)
    {
        this.player1Note = player1Note;
        this.player2Note = player2Note;

        player1Status = "none";
        player2Status = "none";
    }

    public bool contains(GameObject note)
    {
        // in the beginning there are no notes
        if (note == null)
        {
            return false;
        }

        return note.GetComponent<NoteScript>() == player1Note || note.GetComponent<NoteScript>() == player2Note;
    }

    public string getStatus()
    {
        if ((player1Status == "hit") && (player1Status == "hit")) {
            return "hit";
        }

        if (player1Status == "miss" || player2Status == "miss")
        {
            return "miss";
        }

        // note is active, has not been missed or hit yet. 
        else
        {
            return "none";
        }
    }

}
