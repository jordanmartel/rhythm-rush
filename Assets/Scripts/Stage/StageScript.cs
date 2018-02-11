using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class StageScript : MonoBehaviour
{

    [Header("Beat Info")]
    private Beatmap beatmap;
    public GameObject noteObject;
    public List<NoteScript> notesOnScreen;
    public int noteIndex;
    public int noteCreateIndex;
    public double nextBeatTime;
    public double playerOffset;
    public double beatInterval;
    public double noteTravelDistance;
    public int noteHitIndex;
    public double noteTravelSpeed;

    [Header("Input Materials")]
    public Material triangle;
    public Material circle;
    public Material square;
    public Material cross;
    public Material dUp;
    public Material dLeft;
    public Material dRight;
    public Material dDown;

    private float timer;
    private float successTimer;

    [Header("General Player Attributes")]
    public int comboThreshold = 10;

    [Header("Players")]
    public Player player1;
    public Player player2;

    private BossScript boss;
    private TeamAttack teamAttackController;

    public int teamCombo = 0;

    void parseJson(string filePath)
    {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatmap = JsonConvert.DeserializeObject<Beatmap>(beatMapJson);

        player1.notes = beatmap.player1Notes;
        player2.notes = beatmap.player2Notes;
    }

    double BeatInterval(int bpm, int beat_split)
    {
        return 60.0 / bpm / beat_split;
    }


    void createNote(string key, string placement, Player player)
    {

        Vector3 position = player.track.transform.position;
        GameObject newNote = Instantiate(noteObject, new Vector3(position.x, position.y + 3, position.z), new Quaternion(0, 180, 0, 0));
        newNote.GetComponent<NoteScript>().key = key;
        newNote.GetComponent<NoteScript>().index = noteIndex;
        newNote.GetComponent<NoteScript>().placement = placement;
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(key);
        newNote.GetComponent<NoteScript>().feedback = player.feedback;
        newNote.GetComponent<NoteScript>().stage = gameObject;
        newNote.SetActive(true);

    }

    // Use this for initialization
    void Start()
    {

        parseJson("creator_lvl");
        noteTravelSpeed = beatmap.bpm / 20;
        noteTravelDistance = 6;
        playerOffset = 0.05;
        nextBeatTime = beatmap.offset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(beatmap.bpm, beatmap.beat_split);

        player1.joystick = Joysticks.player1Joystick;
        player2.joystick = Joysticks.player2Joystick;

        boss = FindObjectOfType<BossScript>();
        teamAttackController = FindObjectOfType<TeamAttack>();

    }

    string stringToKey(string beat, int joystick)
    {
        switch (beat)
        {
            case "triangle":
                return "joystick " + joystick + " button 3";
            case "circle":
                return "joystick " + joystick + " button 2";
            case "square":
                return "joystick " + joystick + " button 0";
            case "cross":
                return "joystick " + joystick + " button 1";

            // for dpad, use literal name to be worked with later
            default:
                return beat.ToString();

        }
    }

    Material stringToMesh(string key)
    {
        switch (key)
        {
            case "triangle":
                return triangle;
            case "circle":
                return circle;
            case "square":
                return square;
            case "cross":
                return cross;
            case "up":
                return dUp;
            case "left":
                return dLeft;
            case "down":
                return dDown;
            case "right":
                return dRight;
            default:
                return cross;
        }
    }

    void createPlayerNote(string placement, Player player)
    {
        if (player.notes.ContainsKey((noteCreateIndex).ToString()))
        {

            string curBeat = player.notes[(noteCreateIndex).ToString()];
            createNote(curBeat, placement, player);
            noteIndex++;
        }
    }

    bool checkPlayerAction(List<NoteScript> activeNotes, Player player)
    {
        bool noteSuccessfullyHit = false;

        for (int i = 0; i < activeNotes.Count; i++)
        {

            NoteScript headNote = activeNotes[i];
            bool buttonPressed = false;

            // get the dpad axis orientation
            float dpadHorizontal = Input.GetAxis("Controller Axis-Joystick" + player.joystick + "-Axis7");
            float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + player.joystick + "-Axis8");

            // only mark the button as pressed if there has been a change since the last frame and axis is non 0
            if (dpadHorizontal != player.previousDpadHorizontal)
            {
                player.previousDpadHorizontal = dpadHorizontal;
                if (dpadHorizontal != 0)
                {
                    buttonPressed = true;
                }

            }

            //only mark the button as pressed if there has been a change since the last frame, and it is non 0
            if (dpadVertical != player.previousDpadVertical)
            {
                player.previousDpadVertical = dpadVertical;
                if (dpadVertical != 0)
                {
                    buttonPressed = true;
                }
            }

            // check if any of the joystick buttons have been pressed (circle, triangle, square, cross only)
            for (int j = 0; j < 3; j++)
            {
                string currentButton = "joystick " + player.joystick + " button " + j;

                if (Input.GetKeyDown(currentButton) && player.previousButton != currentButton)
                {
                    buttonPressed = true;
                    player.previousButton = currentButton;
                    break;
                }
            }

            // want to know when player has stopped pressing button. Do not want to allow player to simply hold down a button
            if (!buttonPressed)
            {
                player.previousButton = "";
            }

            string keyToHit = stringToKey(headNote.key, player.joystick);

            if (headNote.canHit)
            {
                if ((keyToHit.Equals("left") && dpadHorizontal == -1) ||
                        (keyToHit.Equals("right") && dpadHorizontal == 1) ||
                        (keyToHit.Equals("up") && dpadVertical == 1) ||
                        (keyToHit.Equals("down") && dpadVertical == -1) ||
                        (Input.GetKeyDown(keyToHit)))
                {
                    //print("hit successfully");
                    noteHitIndex++;
                    int dealtDamage = headNote.destroyWithFeedback(player.hitArea, true);

                    player.accumulatedDamage += dealtDamage;
                    if (dealtDamage == 0)
                    {
                        player.combo = 0;
                    }
                    else
                    {
                        noteSuccessfullyHit = true;
                        player.combo += 1;
                    }

                    if (player.combo % comboThreshold == 0)
                    {
                        // TODO: change how the damage scales
                        boss.giveDamage(player.calculateComboDamage(comboThreshold));
                        player.accumulatedDamage = 0;
                    }
                }

                else if (buttonPressed)
                {
                    //print("note missed!");
                    noteHitIndex++;
                    headNote.destroyWithFeedback(player.hitArea, false);
                    player.combo = 0;
                }
            }

            else if (headNote.canMiss)
            {

                if (buttonPressed)
                {
                    //print("note missed!");
                    noteHitIndex++;
                    headNote.destroyWithFeedback(player.hitArea, false);
                    player.combo = 0;
                }
            }

        }

        // TODO: this should actually use a different combo (i.e,  a chain mode combo)
        if (teamCombo >= 120)
        {
            teamAttackController.isActive = true;
        }

        return noteSuccessfullyHit;
    }

    // Update is called once per frame
    void Update()
    {

        timer = timer + Time.deltaTime;

        // Create beat
        if (teamAttackController.isActive)
        {
            // Destroy all notes
            GameObject[] allNotes = GameObject.FindGameObjectsWithTag("note");
            foreach (GameObject note in allNotes)
            {
                note.SetActive(false);
            }
            if (Input.anyKeyDown)
            {
                int attack = teamAttackController.buildTeamAttack();
                if (attack != 0)
                {
                    //score += attack;
                    teamAttackController.Reset();
                }
            }
        }

        else
        {
            if (timer > nextBeatTime && timer - nextBeatTime < 0.1f)
            {
                createPlayerNote("left", player1);
                createPlayerNote("right", player2);
                noteCreateIndex++;
                nextBeatTime = beatmap.offset + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
            }

            NoteScript[] notesOnTrack = FindObjectsOfType<NoteScript>();
            List<NoteScript> currentPlayer1Notes = new List<NoteScript>();
            List<NoteScript> currentPlayer2Notes = new List<NoteScript>();

            foreach (NoteScript note in notesOnTrack)
            {
                if (note.placement == "left")
                {
                    currentPlayer1Notes.Add(note);
                }
                else
                {
                    currentPlayer2Notes.Add(note);
                }
            }

            checkPlayerAction(currentPlayer1Notes, player1);
            checkPlayerAction(currentPlayer2Notes, player2);
        }
    }
}
