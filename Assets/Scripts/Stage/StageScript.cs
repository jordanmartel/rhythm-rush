using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    public double countDownOffset;
    public double playerOffset;
    public double beatInterval;
    public double noteTravelDistance;
    public int noteHitIndex;
    public double phaseOffset;
    public int currentSection;
    public int currentPhase;
    private BeatmapPhase beatmapPhase;
    private bool isRevival = false;
    private int currentRevivalSection = -1;
    public double noteTravelSpeed;
    public double noteSpeedQuotient = 20;
    public bool autoPlay = false;

    [Header("Input Materials")]
    public Material triangle;
    public Material circle;
    public Material square;
    public Material cross;
    public Material dUp;
    public Material dLeft;
    public Material dRight;
    public Material dDown;

    private double nextPhaseStartTime;

    private Player revivingPlayer;

    public float timer;
    public float phaseTimer;
    public float positionInSongTimer;
    public Canvas countDownCanvas;
    public Sprite countDown3;
    public Sprite countDown2;
    public Sprite countDown1;
    public Sprite countDownGo;

    [Header("General Player Attributes")]
    public Team team;
    private BossScript boss;
    private TeamAttack teamAttackController;
    public bool repeatFlag = false;
    public float repeatTime;

    // this is used to know that a team attack just ended, even though teamAttackController.isActive is false
    private bool teamAttackEnding = false;

    // ==========================
    // Use this for initialization

    void Awake()
    {

        parseJson(stageName);
        noteTravelSpeed = beatmap.bpm / noteSpeedQuotient;
        noteTravelDistance = 11.1;
        countDownOffset = 4;
        playerOffset = -0.2;
        phaseOffset = beatmap.getPhase(currentSection, currentPhase).offset;
        nextBeatTime = phaseOffset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(beatmap.bpm, beatmap.beat_split);


        team.player1.joystick = Joysticks.player1Joystick;
        team.player2.joystick = Joysticks.player2Joystick;

        boss = FindObjectOfType<BossScript>();
        teamAttackController = FindObjectOfType<TeamAttack>();

        // for testing, we may start the audio at a different phase
        if (currentSection > 0 || currentPhase > 0)
        {
            FindObjectOfType<AudioControl>().GetComponent<AudioSource>().time = (float)beatmap.getPhase(currentSection, currentPhase).getStartTime();
            FindObjectOfType<AudioControl>().GetComponent<AudioSource>().enabled = true;
            timer = (float)beatmap.getPhase(currentSection, currentPhase).getStartTime();
            positionInSongTimer = (float)beatmap.getPhase(currentSection, currentPhase).getStartTime();
            countDownOffset = 0;
        }

        int nextSection = currentSection;
        int nextPhase = currentPhase + 1;

        if (beatmap.sections[currentSection].Count == currentPhase + 1)
        {
            nextSection = nextSection + 1;
            nextPhase = 0;
        }

        if (nextSection >= beatmap.sections.Count)
        {
            return;
        }

        nextPhaseStartTime = beatmap.getPhase(nextSection, nextPhase).startTime;
    }

    void parseJson(string filePath)
    {
        string beatMapJson = Resources.Load<TextAsset>(filePath).text;
        beatmap = JsonConvert.DeserializeObject<Beatmap>(beatMapJson);

        beatmapPhase = beatmap.getPhase(currentSection, currentPhase);

        // Create new copies of the dictionaries. This way we can delete values from the player dictionary when 
        // spawning notes and still have access to the original notes in the beatmap dictionary when phases are repeated

        if (beatmapPhase.bothPlayerNotes.Count > 0)
        {
            team.player1.notes = new Dictionary<string, string>(beatmapPhase.bothPlayerNotes);
            team.player2.notes = new Dictionary<string, string>(beatmapPhase.bothPlayerNotes);
        }

        else
        {
            team.player1.notes = new Dictionary<string, string>(beatmapPhase.player1Notes);
            team.player2.notes = new Dictionary<string, string>(beatmapPhase.player2Notes);
        }
    }

    double BeatInterval(int bpm, int beat_split)
    {
        return 60.0 / bpm / beat_split;
    }

    void setRepeat()
    {
        repeatFlag = true;
        repeatTime = (float)beatmap.getPhase(currentSection, currentPhase).getEndTime() - (float)beatmap.getPhase(currentSection, currentPhase).getStartTime();
    }

    void moveToNextPhase()
    {
        if (team.player1.failedPhase)
        {
            team.player1.stats.incrementFail();
        }
        else
        {
            team.player1.stats.incrementPerfect();
        }

        if (team.player2.failedPhase)
        {
            team.player2.stats.incrementFail();
        }
        else
        {
            team.player2.stats.incrementPerfect();
        }

        if (teamAttackEnding)
        {
            if (!team.hasFailedPhase())
            {
                currentSection++;
                currentPhase = 0;
            }
            else
            {
                setRepeat();
            }

            teamAttackEnding = false;
            //return;
        }

        else
        {
            // only move to the next phase if the phase has been failed
            if (!team.hasFailedPhase() && !isRevival)
            {
                // boss attack phase that has been successful
                if (currentPhase + 1 == beatmap.sections[currentSection].Count)
                {
                    boss.resetAttackState();
                    teamAttackController.startTeamAttack();

                    team.player1.activeNotes.Clear();
                    team.player1.notes.Clear();
                    team.player2.activeNotes.Clear();
                    team.player2.notes.Clear();

                    return;
                }

                // regular phase
                else
                {

                    if (!isRevival)
                    {
                        boss.giveDamage(1);
                        currentPhase++;
                    }
                }
            }

            // this is a boss attack phase that has been failed
            else if (currentPhase + 1 == beatmap.sections[currentSection].Count)
            {
                if (!isRevival)
                {
                    team.attackedByBoss();
                    boss.resetAttackState();
                    setRepeat();
                }

                if (team.health == 0)
                {
                    // GAME OVER
                    // Press [F] to pay respects

                }

                if (team.player1.failedPhase && team.player2.failedPhase)
                {
                    // Go back a phase when both players "stunned" by boss attack
                    setRepeat();
                    currentPhase--;
                    boss.recoverHealth(1);
                    team.player1.stats.incrementFail();
                    team.player2.stats.incrementFail();
                }

                else
                {
                    if (isRevival)
                    {
                        isRevival = false;
                    }
                    else
                    {
                        isRevival = true;
                        //REVIVE Phase
                        //Debug.Log("player Fails: " + team.player1.failedPhase + "<" + team.player2.failedPhase);
                        //I have NO idea why the player are backwards but this is way it works ¯\_(ツ)_/¯
                        if (team.player1.failedPhase)
                        {
                            team.KnockDownPlayer(team.player2);

                        }
                        else
                        {
                            team.KnockDownPlayer(team.player1);
                        }

                        FindObjectOfType<AudioControl>().GetComponent<AudioSource>().Pause();

                        currentRevivalSection = currentSection;
                        currentSection = beatmap.sections.Count - 1;
                        currentPhase = 0;
                    }
                }
            }
            // A regular phase that is failed
            else if (!isRevival && team.hasFailedPhase())
            {
                setRepeat();
            }
        }

        // retrieve the next phase, and corresponding notes
        beatmapPhase = beatmap.getPhase(currentSection, currentPhase);
        phaseOffset = beatmapPhase.offset;

        // entering a boss attack phase should show the boss as preparing for an attack
        if (!isRevival && currentPhase + 1 == beatmap.sections[currentSection].Count)
        {
            //Debug.Log("Entering Boss Attack Phase");
            boss.setAttackState();
        }

        if (isRevival)
        {
            if (team.player1.IsDown)
            {
                team.player1.notes = new Dictionary<string, string>(beatmapPhase.revivalNotes);
                team.player2.notes = new Dictionary<string, string>();
            }
            else
            {
                team.player2.notes = new Dictionary<string, string>(beatmapPhase.revivalNotes);
                team.player1.notes = new Dictionary<string, string>();
            }

        }
        else
        {
            // get the values from the beatmap
            if (beatmapPhase.bothPlayerNotes.Count > 0)
            {
                team.player1.notes = new Dictionary<string, string>(beatmapPhase.bothPlayerNotes);
                team.player2.notes = new Dictionary<string, string>(beatmapPhase.bothPlayerNotes);
            }

            else
            {
                team.player1.notes = new Dictionary<string, string>(beatmapPhase.player1Notes);
                team.player2.notes = new Dictionary<string, string>(beatmapPhase.player2Notes);
            }
        }

        if (!repeatFlag)
        {
            // the beat time is phase dependent, so reset these
            nextBeatTime = phaseOffset + playerOffset - noteTravelDistance / noteTravelSpeed;
            noteCreateIndex = 0;
            phaseTimer = 0;
            // resets the failed phase status for both players
            team.nextPhaseBegin();


            int nextSection = currentSection;
            int nextPhase = currentPhase + 1;

            if (beatmap.sections[currentSection].Count == currentPhase + 1)
            {
                nextSection = nextSection + 1;
                nextPhase = 0;
            }

            if (nextSection >= beatmap.sections.Count)
            {
                return;
            }

            nextPhaseStartTime = beatmap.getPhase(nextSection, nextPhase).startTime;
            Debug.Log(nextPhaseStartTime);

        }        
    }

    GameObject createNote(String note, string placement, Player player)
    {
        Vector3 position = player.getNoteStart(note);

        GameObject newNote = Instantiate(noteObject, new Vector3(position.x, position.y, position.z), new Quaternion(0, 180, 0, 0));
        newNote.GetComponent<NoteScript>().key = note;
        newNote.GetComponent<NoteScript>().index = noteCreateIndex;
        newNote.GetComponent<NoteScript>().stage = gameObject;
        newNote.GetComponent<NoteScript>().destination = player.getNoteDestination(note);
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(note);
        newNote.GetComponent<NoteScript>().feedback = player.feedback;
        newNote.GetComponent<NoteScript>().player = player;
        newNote.SetActive(true);

        player.activeNotes.Add(newNote);

        return newNote;

    }

    bool isFaceNote(String note)
    {
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

    void checkPlayerAction(Player player)
    {
        GameObject noteObj = player.activeNotes[0];
        NoteScript headNote = noteObj.GetComponent<NoteScript>();
        bool buttonPressed = false;

        // get the dpad axis orientation
        float dpadHorizontal = Input.GetAxis("Controller Axis-Joystick" + player.joystick + "-Axis7");
        float dpadVertical = Input.GetAxis("Controller Axis-Joystick" + player.joystick + "-Axis8");

        // player has pressed or released dpad horizontally
        if (dpadHorizontal != player.previousDpadHorizontal)
        {
            player.previousDpadHorizontal = dpadHorizontal;
            if (dpadHorizontal != 0)
            {
                buttonPressed = true;
            }

        }

        // player has pressed or released dpad vertically
        if (dpadVertical != player.previousDpadVertical)
        {
            player.previousDpadVertical = dpadVertical;
            if (dpadVertical != 0)
            {
                buttonPressed = true;
            }
        }


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

        string keyToHit = stringToKey(headNote.key, player.joystick);
        if (headNote.canHit || headNote.canMiss)
        {
            if (Input.GetKeyDown(keyToHit) || (((headNote.key.Equals("square") && dpadHorizontal == -1) ||
                   (headNote.key.Equals("circle") && dpadHorizontal == 1) ||
                   (headNote.key.Equals("triangle") && dpadVertical == 1) ||
                   (headNote.key.Equals("cross") && dpadVertical == -1)) && (buttonPressed)) || autoPlay)
            {


                if (autoPlay && !headNote.canHit)
                {
                    return;
                }

                //print("hit successfully");
                noteHitIndex++;
                int dealtDamage = headNote.destroyWithFeedback(player.getHitArea(headNote.key), true);
                player.updateComboCount(true);

                if (dealtDamage == 0)
                {
                    player.combo = 0;

                    // fail phase on miss
                    player.failedPhase = true;
                    player.updateComboCount(false);
                }
                else
                {
                    player.combo += 1;
                }

                player.activeNotes.Remove(noteObj);

            }
            else if (buttonPressed)
            {
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
        // press space to toggle autoplay
        if (Input.GetKeyDown(KeyCode.Space))
        {
            autoPlay = !autoPlay;
        }

        timer += Time.deltaTime;

        if (timer >= 3)
        {
            countDownCanvas.GetComponentInChildren<Image>().sprite = countDownGo;
        }
        else if (timer >= 2)
        {
            countDownCanvas.GetComponentInChildren<Image>().sprite = countDown1;
        }
        else if (timer >= 1)
        {
            countDownCanvas.GetComponentInChildren<Image>().sprite = countDown2;
        }

        if (timer >= countDownOffset)
        {
            phaseTimer += Time.deltaTime;
            positionInSongTimer += Time.deltaTime;
            countDownCanvas.enabled = false;
        }
        if (repeatFlag)
        {
            nextBeatTime = phaseOffset + playerOffset - noteTravelDistance / noteTravelSpeed;
            noteCreateIndex = 0;

            positionInSongTimer = positionInSongTimer - phaseTimer;
            phaseTimer = 0;

            // resets the failed phase status for both players
            team.nextPhaseBegin();
            FindObjectOfType<AudioControl>().GetComponent<AudioSource>().time = (float)beatmap.getPhase(currentSection, currentPhase).getStartTime();
            repeatFlag = false;
        }
        if (teamAttackController.isActive)
        {
            if (teamAttackController.timerExpired())
            {
                int damage = teamAttackController.unleashTeamAttack();
                if (damage == 0)
                {
                    team.player1.failedPhase = true;
                    team.player2.failedPhase = true;
                    team.player1.updateComboCount(false);
                    team.player2.updateComboCount(false);
                }
                else
                {
                    boss.giveDamage(damage);
                }

                // team attack is over, but this variable lets us know later that a team attack just finished. This is used
                // to correctly spawn notes at the right time after a team attack (prevents the next phase from starting early)
                teamAttackEnding = true;

            }
            else
            {
                if (Input.anyKeyDown || autoPlay)
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
                if (positionInSongTimer >= nextPhaseStartTime) {

                    //Revival Complete. IsRevival is set to false inside MovetoNextPhase
                    if (isRevival && !team.hasFailedPhase())
                    {
                        currentSection = currentRevivalSection;
                        currentPhase = beatmap.sections[currentSection].Count - 1;
                        team.revivePlayer();

                        FindObjectOfType<AudioControl>().GetComponent<AudioSource>().time = (float) beatmap.getPhase(currentSection, currentPhase).startTime;
                        FindObjectOfType<AudioControl>().GetComponent<AudioSource>().Play();

                    }

                    moveToNextPhase();
                }

                // after a boss phase, start the next phase immediately (team attack)
                else if (currentPhase + 1 == beatmap.sections[currentSection].Count && !teamAttackEnding)
                {
                    moveToNextPhase();
                }
            }


            if (phaseTimer > nextBeatTime && phaseTimer - nextBeatTime < 0.1f)
            {
                createPlayerNote("left", team.player1);
                createPlayerNote("right", team.player2);

                noteCreateIndex++;
                nextBeatTime = phaseOffset + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
            }

            if (team.player1.activeNotes.Count > 0)
            {
                checkPlayerAction(team.player1);
            }

            if (team.player2.activeNotes.Count > 0)
            {
                checkPlayerAction(team.player2);
            }
        }
    }
}
