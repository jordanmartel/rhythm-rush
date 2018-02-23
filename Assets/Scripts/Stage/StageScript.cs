using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class StageScript : MonoBehaviour
{

    [Header("Beat Info")]
    public string stageName = "creator_lvl";
    public Beatmap beatmap;
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

    private int currentSection;
    private int currentPhase;
    private BeatmapPhase beatmapPhase;

    private float timer;
    private float phaseTimer;

    [Header("General Player Attributes")]
    public Team team;

    private BossScript boss;
    private TeamAttack teamAttackController;

    // ==========================
    // Use this for initialization

    void Awake () {

        parseJson(stageName);
        noteTravelSpeed = beatmap.bpm / 20;
        noteTravelDistance = 6;
        playerOffset = 0.05;
        nextBeatTime = beatmap.offset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(beatmap.bpm, beatmap.beat_split);

        team.player1.joystick = Joysticks.player1Joystick;
        team.player2.joystick = Joysticks.player2Joystick;

        boss = FindObjectOfType<BossScript>();
        teamAttackController = FindObjectOfType<TeamAttack>();

    }

    void parseJson(string filePath)
    {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatmap = JsonConvert.DeserializeObject<Beatmap>(beatMapJson);

        beatmapPhase = beatmap.getPhase(currentSection, currentPhase);

        // Create new copies of the dictionaries. This way we can delete values from the player dictionary when 
        // spawning notes and still have access to the original notes in the beatmap dictionary when phases are repeated
        team.player1.notes = new Dictionary<string, string>(beatmapPhase.player1Notes);
        team.player2.notes = new Dictionary<string, string>(beatmapPhase.player2Notes);

        if (beatmapPhase.bothPlayerNotes.Count > 0)
        {
            team.player1.notes = new Dictionary<string, string>(beatmapPhase.bothPlayerNotes);
            team.player2.notes = new Dictionary<string, string>(beatmapPhase.bothPlayerNotes);
        }
    }

    double BeatInterval(int bpm, int beat_split)
    {
        return 60.0 / bpm / beat_split;
    }

    void moveToNextPhase(bool teamAttack)
    {
        
        Debug.Log("Old section and phase: " + currentSection + " " + currentPhase);
        Debug.Log("Phase was failed: " + team.hasFailedPhase());

        if (teamAttack)
        {
            if (!team.hasFailedPhase())
            {
                currentSection++;
                currentPhase = 0;
            }
        }

        else
        {
            // only move to the next phase if the phase has been failed
            if (!team.hasFailedPhase())
            {
                // boss attack phase that has been successful
                if (currentPhase + 1 == beatmap.sections[currentSection].Count)
                {
                    Debug.Log("Boss attack phase successful");

                    boss.resetAttackState();
                    teamAttackController.startTeamAttack();
                    GameObject[] allNotes = GameObject.FindGameObjectsWithTag("note");
                    foreach (GameObject note in allNotes)
                    {
                        Destroy(note); //note.SetActive(false);
                    }
                    team.player1.activeNotes.Clear();
                    team.player2.activeNotes.Clear();
                }

                // regular phase
                else
                {
                    currentPhase++;
                    boss.giveDamage(1);
                }
            }

            // this is a boss attack phase that has been failed
            else if (currentPhase + 1 == beatmap.sections[currentSection].Count)
            {

                team.attackedByBoss();
                boss.resetAttackState();

                if (team.health == 0)
                {
                    // GAME OVER
                    // Press [F] to pay respects
                }

                if (team.player1.failedPhase && team.player2.failedPhase)
                {
                    // Go back a phase when both players "stunned" by boss attack
                    currentPhase--;
                }

                else
                {
                    // REVIVE PHASE
                }
            }
        }
        
        // retrieve the next phase, and corresponding notes
        beatmapPhase = beatmap.getPhase(currentSection, currentPhase);

        // entering a boss attack phase should show the boss as preparing for an attack
        if (currentPhase + 1 == beatmap.sections[currentSection].Count)
        {
            Debug.Log("Entering Boss Attack Phase");
            boss.setAttackState();
        }

        // get the values from the beatmap
        team.player1.notes = new Dictionary<string, string>(beatmapPhase.player1Notes);
        team.player2.notes = new Dictionary<string, string>(beatmapPhase.player2Notes);

        if (beatmapPhase.bothPlayerNotes.Count > 0)
        {
            team.player1.notes = new Dictionary<string, string>(beatmapPhase.bothPlayerNotes);
            team.player2.notes = new Dictionary<string, string>(beatmapPhase.bothPlayerNotes);
        }

        // the beat time is phase dependent, so reset these
        nextBeatTime = beatmap.offset + playerOffset - noteTravelDistance / noteTravelSpeed;
        noteCreateIndex = 0;
        phaseTimer = 0;

        // resets the failed phase status for both players
        team.nextPhaseBegin();

        Debug.Log("New section and phase: " + currentSection + " " + currentPhase);

    }

    GameObject createNote(String note, string placement, Player player)
    {
        bool isFace = isFaceNote(note);

        Vector3 position = player.getNoteStart(note);

        GameObject newNote = Instantiate(noteObject, new Vector3(position.x, position.y, position.z), new Quaternion(0, 180, 0, 0));
        newNote.GetComponent<NoteScript>().key = note;
        newNote.GetComponent<NoteScript>().index = noteIndex;
        newNote.GetComponent<NoteScript>().stage = gameObject;
        newNote.GetComponent<NoteScript>().destination = player.getNoteDestination(note);
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(note);
        newNote.GetComponent<NoteScript>().feedback = player.feedback;
        newNote.GetComponent<NoteScript>().player = player;
        newNote.SetActive(true);

        player.activeNotes.Add(newNote);

        return newNote;

    }

    bool isFaceNote(String note) {
        return note == "triangle" || note == "circle" || 
            note == "cross" || note == "square";
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

            string curNote = player.notes[(noteCreateIndex).ToString()];
            createNote(curNote, placement, player);

            // remove notes from the player notes list after they have been spawned. This way, we can check for the case where 
            // the player has no more notes to play, and where there are no more active notes in play. This would be for the end
            // of a phase
            player.notes.Remove((noteCreateIndex).ToString());

            noteIndex++;
        }
    }

    void checkPlayerAction(Player player, GameObject noteObj)
    {
        NoteScript headNote = noteObj.GetComponent<NoteScript>();
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

        if (headNote.canHit || headNote.canMiss) {
            if ((keyToHit.Equals("left") && dpadHorizontal == -1) ||
                    (keyToHit.Equals("right") && dpadHorizontal == 1) ||
                    (keyToHit.Equals("up") && dpadVertical == 1) ||
                    (keyToHit.Equals("down") && dpadVertical == -1) ||
                    (Input.GetKeyDown(keyToHit))) {
                //print("hit successfully");
                noteHitIndex++;
                int dealtDamage = headNote.destroyWithFeedback(player.getHitArea(headNote.key), true);
                player.updateComboCount(true);

                if (dealtDamage == 0) {
                    player.combo = 0;

                    // fail phase on miss
                    player.failedPhase = true;
                    player.updateComboCount(false);
                }
                else {
                    player.combo += 1;
                }

                player.activeNotes.Remove(noteObj);

            }
            else if (buttonPressed) {
                noteHitIndex++;
                headNote.destroyWithFeedback(player.getHitArea(headNote.key), false);
                player.updateComboCount(false);
                player.activeNotes.Remove(noteObj);
                player.combo = 0;

                // fail phase on miss
                player.failedPhase = true;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        phaseTimer += Time.deltaTime;

        // Create beat
        if (teamAttackController.isActive)
        {

            // even if its in team attack mode, we need to update the indexes to work correctly after team attack ends
            if (phaseTimer > nextBeatTime && phaseTimer - nextBeatTime < 0.1f)
            {
                noteCreateIndex++;
                nextBeatTime = beatmap.offset + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
            }           

            if (teamAttackController.timerExpired())
            {
                int damage = teamAttackController.unleashTeamAttack();
                if (damage == 0)
                {
                    team.player1.failedPhase = true;
                    team.player2.failedPhase = true;
                    moveToNextPhase(true);
                }
                else
                {
                    boss.giveDamage(teamAttackController.unleashTeamAttack());
                    moveToNextPhase(true);
                }
                
            }

            else
            {
                if (Input.anyKeyDown)
                {
                    teamAttackController.buildTeamAttack();
                }
            }   
        }

        else
        {
            // start of the next phase
            if (!team.hasNotesLeft())
            {
                moveToNextPhase(false);
            }


            if (phaseTimer > nextBeatTime && phaseTimer - nextBeatTime < 0.1f)
            {
                createPlayerNote("left", team.player1);
                createPlayerNote("right", team.player2);

                noteCreateIndex++;
                nextBeatTime = beatmap.offset + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
            }

            if (team.player1.activeNotes.Count > 0)
            {
                checkPlayerAction(team.player1, team.player1.activeNotes[0]);
            }

            if (team.player2.activeNotes.Count > 0)
            {
                checkPlayerAction(team.player2, team.player2.activeNotes[0]);
            }
        }
    }
}
