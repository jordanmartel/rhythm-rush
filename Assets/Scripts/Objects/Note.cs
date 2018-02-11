using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[Serializable]
public class Note
{
    public string value;
    public string type;

    public Note(string value, string type) {

        this.value = value;
        this.type = type;
    }
}
