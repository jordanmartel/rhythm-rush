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

    [Header("General Player Attributes")]
    public int comboThreshold = 10;

    public Team team;

    private BossScript boss;
    private TeamAttack teamAttackController;

    private int totalChainSize;
    private int teamCombo = 0;

    Dictionary<string, string> chainNotes = new Dictionary<string, string>();
    List<ChainNote> activeChainNotes = new List<ChainNote>();


    // ==========================
    // Use this for initialization
    void Start()
    {

        parseJson("creator_lvl");
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

    public void resetChainCombo()
    {
        teamCombo = 0;
    }

    // the NoteScript will use this to mark chain notes as missed
    public List<ChainNote> getActiveChainNotes()
    {
        return activeChainNotes;
    }

    void parseJson(string filePath)
    {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatmap = JsonConvert.DeserializeObject<Beatmap>(beatMapJson);

        team.player1.notes = beatmap.player1Notes;
        team.player2.notes = beatmap.player2Notes;
        chainNotes = beatmap.chainNotes;
    }

    double BeatInterval(int bpm, int beat_split)
    {
        return 60.0 / bpm / beat_split;
    }


    GameObject createNote(String note, string placement, Player player)
    {

        Vector3 position = player.track.transform.position;
        GameObject newNote = Instantiate(noteObject, new Vector3(position.x, position.y + 3, position.z), new Quaternion(0, 180, 0, 0));
        newNote.GetComponent<NoteScript>().key = note;
        newNote.GetComponent<NoteScript>().index = noteIndex;
        newNote.GetComponent<NoteScript>().stage = gameObject;
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(note);
        newNote.GetComponent<NoteScript>().feedback = player.feedback;
        newNote.GetComponent<NoteScript>().player = player;
        newNote.SetActive(true);

        player.activeNotes.Add(newNote);

        return newNote;

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
            noteIndex++;
        }
    }

    // creates two notes, one for each player
    void createChainNote()
    {
        if (chainNotes.ContainsKey((noteCreateIndex).ToString()))
        {
            string curNote = chainNotes[(noteCreateIndex).ToString()];
            activeChainNotes.Add(new ChainNote(createNote(curNote, "left", team.player1).GetComponent<NoteScript>(),
                createNote(curNote, "right", team.player2).GetComponent<NoteScript>()));
            noteIndex++;
        }
    }

    string checkPlayerAction(Player player, GameObject noteObj)
    {
        string playerAction = "none";

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
                    playerAction = "miss";
                    player.combo = 0;
                }
                else
                {
                    playerAction = "hit";
                    player.combo += 1;
                }

                if (player.combo % comboThreshold == 0)
                {
                    // TODO: change how the damage scales
                    boss.giveDamage(player.calculateComboDamage(comboThreshold));
                    player.accumulatedDamage = 0;
                }
                player.activeNotes.Remove(noteObj);
        }

        else if (buttonPressed)
            {
                playerAction = "miss";
                noteHitIndex++;
                headNote.destroyWithFeedback(player.hitArea, false);
                player.activeNotes.Remove(noteObj);
                player.combo = 0;
            }
        }

        else if (headNote.canMiss)
        {

            if (buttonPressed)
            {
                playerAction = "miss";
                noteHitIndex++;
                headNote.destroyWithFeedback(player.hitArea, false);
                player.activeNotes.Remove(noteObj);
                player.combo = 0;
            }
        }

        return playerAction;
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime;

        // WIN
        if (boss.endStatus > 0)
        {
            //TODO: win animation. Ranking screen
        }

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
                createPlayerNote("left", team.player1);
                createPlayerNote("right", team.player2);

                createChainNote();

                noteCreateIndex++;
                nextBeatTime = beatmap.offset + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
            }


            string player1Action = "none";
            string player2Action = "none";
            GameObject nextPlayer1Note = null;
            GameObject nextPlayer2Note = null;

            if (team.player1.activeNotes.Count > 0)
            {
                nextPlayer1Note = team.player1.activeNotes[0];
                player1Action = checkPlayerAction(team.player1, nextPlayer1Note);
            }

            if (team.player2.activeNotes.Count > 0)
            {
                nextPlayer2Note = team.player2.activeNotes[0];
                player2Action = checkPlayerAction(team.player2, nextPlayer2Note);
            }

            if (activeChainNotes.Count > 0)
            {
                ChainNote nextChainNote = activeChainNotes[0];

                if (nextChainNote.contains(nextPlayer1Note)) {
                    nextChainNote.player1Status = player1Action;
                }

                if (nextChainNote.contains(nextPlayer2Note)) {
                    nextChainNote.player1Status = player1Action;
                }

                // both players hit the note
                if (nextChainNote.getStatus() == "hit")
                {
                    teamCombo += 1;
                    activeChainNotes.Remove(nextChainNote);
                }
                
                // at least one player missed the note
                else if (nextChainNote.getStatus() == "miss")
                {
                    teamCombo = 0;
                    activeChainNotes.Remove(nextChainNote);
                }

                // last note has been played
                if (activeChainNotes.Count == 0)
                {
                    // all notes hit
                    if (teamCombo == 10)
                    {
                        teamAttackController.isActive = true;
                    }

                    // boss does damage
                    else
                    {
                        team.attackedByBoss();

                        if (team.health == 0)
                        {
                            // GAME OVER
                            // TODO: death animation, ranking screen showing "F" or similar
                        }
                    }

                    teamCombo = 0;
                }
            }

        }
    }
}
