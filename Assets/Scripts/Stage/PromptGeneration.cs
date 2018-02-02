using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PromptGeneration : MonoBehaviour {

    public GameObject note;
    public Beatmap beatmap;

    private List<Note> notes = new List<Note>();

    // Use this for initialization
    void Start () {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Beatmap.json");
        string jsonString = File.ReadAllText(filePath);
        beatmap = JsonUtility.FromJson<Beatmap>(jsonString);

        notes = new List<Note>(beatmap.notes);
    }
	
	// Update is called once per frame
	void Update () {

        float elapsedTime = Time.time;

        // this is bad, probably redo with some sort of hashmap structure
        for (int i = 0; i < notes.Count; i ++)
        {
            if (elapsedTime > notes[i].time)
            {
				Vector3 position = transform.position;
				GameObject newNote = Instantiate(note, position, Quaternion.identity);
                newNote.SetActive(true);
                notes.Remove(notes[i]);
                break;
            }
        }
		
	}
}
