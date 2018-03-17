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
    public bool isRevival = false;
    public bool revivalInProgress = false;
    //private int currentRevivalSection = -1;
    public double noteTravelSpeed;
    public double noteSpeedQuotient = 20;
    public bool autoPlay = false;
    private int GSpeed = 1;
    public bool bossAttackInProgress = false;

    [Header("Audio")]
    public AudioSource musicPlayer;
    public AudioSource revivalSoundPlayer;

    [Header("Input Materials")]
    public Material triangle;
    public Material circle;
    public Material square;
    public Material cross;
    public Material dUp;
    public Material dLeft;
    public Material dRight;
    public Material dDown;

    [Header("Input Glow Materials")]
    public Material SSJtriangle;
    public Material SSJcircle;
    public Material SSJsquare;
    public Material SSJcross;

    private double nextPhaseStartTime;

    private Player revivingPlayer;

    [Header("Intro Objects")]
    public GameObject mainCamera;
    [Header("Other")]
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

    public Animator bossAnimator;    
    
    // this is used to know that a team attack just ended, even though teamAttackController.isActive is false
    private bool teamAttackEnding = false;

    // ==========================
    // Use this for initialization

    void Awake()
    {

        parseJson(stageName);
        Metadata.resetStats();
        noteTravelSpeed = beatmap.bpm / noteSpeedQuotient;
        noteTravelDistance = 11.1;
        countDownOffset = 0;
        phaseOffset = beatmap.getPhase(currentSection, currentPhase).offset;
        nextBeatTime = phaseOffset + playerOffset - noteTravelDistance / noteTravelSpeed;
        beatInterval = BeatInterval(beatmap.bpm, beatmap.beat_split);


        team.player1.joystick = Metadata.player1Joystick;
        team.player2.joystick = Metadata.player2Joystick;

        boss = FindObjectOfType<BossScript>();
        teamAttackController = FindObjectOfType<TeamAttack>();


        // for testing, we may start the audio at a different phase
        if (currentSection > 0 || currentPhase > 0)
        {
            musicPlayer.time = (float)beatmap.getPhase(currentSection, currentPhase).getStartTime();
            musicPlayer.enabled = true;
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


        // moving to the next phase after boss attack is in progress implies team attack.
        if (bossAttackInProgress)
        {
            if (bossAnimator != null)
            {
                bossAnimator.SetBool("PreparingAttack", false);
            }
            teamAttackController.startTeamAttack();

            team.player1.activeNotes.Clear();
            team.player1.notes.Clear();
            team.player2.activeNotes.Clear();
            team.player2.notes.Clear();

            bossAttackInProgress = false;

            currentSection++;
            currentPhase = 0;

        }
        
        // revival just finished
        else if (revivalInProgress)
        {
            currentSection++;
            currentPhase = 0;
        }

        // regular phase just finished
        else
        {
            //boss.giveDamage(1);
            currentPhase++;
            if (bossAnimator != null)
            {
                bossAnimator.SetBool("Damaged", true);
            }

            // transitioning from a regular phase to a boss phase
            if (currentPhase + 1 == beatmap.sections[currentSection].Count)
            {
                bossAttackInProgress = true;
                if (bossAnimator != null)
                {
                    bossAnimator.SetBool("PreparingAttack", true);
                }
            }
        }
        

        // no more phases
        if (beatmap.sections.Count <= currentSection)
        {
            team.player1.health = 0;
            team.player2.health = 0;
            return;
        }


        // retrieve the next phase, and corresponding notes
        beatmapPhase = beatmap.getPhase(currentSection, currentPhase);
        phaseOffset = beatmapPhase.offset;

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
        

        // the beat time is phase dependent, so reset these
        nextBeatTime = phaseOffset + playerOffset - noteTravelDistance / noteTravelSpeed;
        noteCreateIndex = 0;
        phaseTimer = 0;
        // resets the failed phase status for both players
        team.nextPhaseBegin();

        Debug.Log(currentSection + " " +  currentPhase);

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

    GameObject createNote(String note, string placement, Player player)
    {
        Vector3 position = player.getNoteStart(note);

        GameObject newNote = Instantiate(noteObject, new Vector3(position.x, position.y, position.z), new Quaternion(0, 180, 0, 0));
        newNote.GetComponent<NoteScript>().key = note;
        newNote.GetComponent<NoteScript>().index = noteCreateIndex;
        newNote.GetComponent<NoteScript>().stage = gameObject.GetComponent<StageScript>();
        newNote.GetComponent<NoteScript>().destination = player.getNoteDestination(note);
        newNote.GetComponent<MeshRenderer>().material = stringToMesh(note);
        newNote.GetComponent<NoteScript>().feedback = player.feedback;
        newNote.GetComponent<NoteScript>().player = player;
        newNote.SetActive(true);
        if (player.skillController.anyKeyActive)
        {
            newNote.GetComponent<NoteScript>().anyKey = true;
        }

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
        if (bossAttackInProgress) {
            switch (key) {
                case "triangle":
                    return SSJtriangle;
                case "circle":
                    return SSJcircle;
                case "square":
                    return SSJsquare;
                case "cross":
                    return SSJcross;
            }
        }

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

    public void copyPlayerStats() {
        Debug.Log("Copy Start");
        team.copyStats();
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

        //powerup used // Note: R1 button
        if (player.pUpAvailable && Input.GetKeyDown("joystick " + player.joystick + " button 5"))
        {
            Debug.Log("skill triggered");
            player.triggerSkill();
        }

        string keyToHit = stringToKey(headNote.key, player.joystick);
        if (headNote.canHit || headNote.canMiss)
        {
            if ((headNote.anyKey && buttonPressed) || Input.GetKeyDown(keyToHit) || 
                (((headNote.key.Equals("square") && dpadHorizontal == -1) ||
                   (headNote.key.Equals("circle") && dpadHorizontal == 1) ||
                   (headNote.key.Equals("triangle") && dpadVertical == 1) ||
                   (headNote.key.Equals("cross") && dpadVertical == -1)) && buttonPressed) || autoPlay)
            {


                if (autoPlay && !headNote.canHit)
                {
                    return;
                }

                //print("hit successfully");
                noteHitIndex++;
                int score = headNote.destroyWithFeedback(player.getHitArea(headNote.key), true);
                player.updateComboCount(true, score);

                if (score == 0)
                {
                    player.resetCombo();

                    // fail phase on miss
                    if (!player.skillController.petActive)
                    {
                        if (bossAttackInProgress)
                        {
                            isRevival = true;
                            player.KnockDownPlayer();
                            if (bossAnimator != null)
                            {
                                bossAnimator.SetBool("Attacking", true);
                                bossAnimator.SetBool("PreparingAttack", false);
                            }
                            bossAttackInProgress = false;
                        }

                        // failed a revive
                        else if (revivalInProgress)
                        {
                            isRevival = true;

                            // attack the opposite player that is stunned (bleeding out)
                            if (team.player1 == player)
                            {
                                team.player2.attackedByBoss();
                            }

                            else
                            {
                                team.player1.attackedByBoss();
                            }
                        }

                        else
                        {
                            player.attackedByBoss();
                        }

                    }
                    else
                    {
                        player.skillController.petHelp();
                    }
                    player.updateComboCount(false, 0);
                }
                else
                {
                    player.updateCombo();
                }

                player.activeNotes.Remove(noteObj);

            }

            else if (buttonPressed)
            {
                noteHitIndex++;
                headNote.destroyWithFeedback(player.getHitArea(headNote.key), false);
                player.updateComboCount(false, 0);
                player.activeNotes.Remove(noteObj);
                player.resetCombo();

                // fail phase on miss
                if (!player.skillController.petActive)
                {
                    if (bossAttackInProgress)
                    {
                        isRevival = true;
                        player.KnockDownPlayer();
                        if (bossAnimator != null)
                        {
                            bossAnimator.SetBool("Attacking", true);
                            bossAnimator.SetBool("PreparingAttack", false);
                        }
                        bossAttackInProgress = false;
                    }

                    // failed a revive
                    else if (revivalInProgress)
                    {
                        isRevival = true;

                        // attack the opposite player that is stunned (bleeding out)
                        if (team.player1 == player)
                        {
                            team.player2.attackedByBoss();
                        }

                        else
                        {
                            team.player1.attackedByBoss();
                        }
                    }

                    else
                    {
                        player.attackedByBoss();
                    }
                }
                else
                {
                    player.skillController.petHelp();
                }

            }
        }
    }

    // Update is called once per frame
    void Update() {

        Time.timeScale = GSpeed;

        if (!mainCamera.GetComponent<Animation>().isPlaying) {

            if (!musicPlayer.isPlaying) {
                musicPlayer.Play();
                countDownCanvas.gameObject.SetActive(true);

            }

            if (isRevival) {
                if (team.player1.IsDown && team.player2.IsDown) {
                    team.player1.health = 0;
                    team.player2.health = 0;
                    return;
                }

                // clear notes from the list, and destroy any that are in play
                team.player1.activeNotes.Clear();
                team.player1.notes.Clear();
                team.player2.activeNotes.Clear();
                team.player2.notes.Clear();

                GameObject[] notes = GameObject.FindGameObjectsWithTag("note");
                foreach (GameObject note in notes) {
                    // nani the fuck
                    Destroy(note);
                }

                // get the revival phase
                beatmapPhase = beatmap.getPhase(beatmap.sections.Count - 1, 0);
                nextBeatTime = phaseOffset + playerOffset - noteTravelDistance / noteTravelSpeed;
                noteCreateIndex = 0;
                phaseTimer = 0;

                // set up notes for the player still alive
                if (team.player2.IsDown) {
                    team.player1.notes = new Dictionary<string, string>(beatmapPhase.revivalNotes);
                    team.player2.notes = new Dictionary<string, string>();
                }
                else {
                    team.player2.notes = new Dictionary<string, string>(beatmapPhase.revivalNotes);
                    team.player1.notes = new Dictionary<string, string>();
                }


                musicPlayer.Stop();
                revivalSoundPlayer.Play();

                isRevival = false;
                revivalInProgress = true;
            }

            // press space to toggle autoplay
            if (Input.GetKeyDown(KeyCode.Space)) {
                autoPlay = !autoPlay;
            }
            // press Right Arrow for 2x Speed
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                GSpeed *= 2;
            }
            // pressLeft Arrow for x0.5 Speed
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                GSpeed /= 2;
            }

            timer += Time.deltaTime;

            if (timer >= 3) {
                countDownCanvas.GetComponentInChildren<Image>().sprite = countDownGo;
            }
            else if (timer >= 2) {
                countDownCanvas.GetComponentInChildren<Image>().sprite = countDown1;
            }
            else if (timer >= 1) {
                countDownCanvas.GetComponentInChildren<Image>().sprite = countDown2;
            }

            if (timer >= countDownOffset) {
                phaseTimer += Time.deltaTime;
                positionInSongTimer += Time.deltaTime;
                countDownCanvas.enabled = false;
            }

            if (teamAttackController.isActive) {

                // even though it is in the team attack stage, update the next beat time when the phase time passes the next beat in the song
                if (phaseTimer > nextBeatTime) {
                    noteCreateIndex++;
                    nextBeatTime = phaseOffset + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
                }

                if (teamAttackController.timerExpired()) {
                    teamAttackEnding = true;
                    int damage = teamAttackController.unleashTeamAttack();
                    if (damage == 0) {
                        team.player1.updateComboCount(false, 0);
                        team.player2.updateComboCount(false, 0);
                    }
                    else {
                        boss.giveDamage(damage);
                        if (bossAnimator != null) {
                            bossAnimator.SetBool("Damaged", true);
                        }
                    }
                }
                else {
                    if (Input.anyKeyDown || autoPlay) {
                        teamAttackController.buildTeamAttack();
                    }
                }
            }

            else {
                // start of the next phase
                if (!team.hasNotesLeft()) {
                    // after successful revive, restart the music and move to the next phase
                    if (revivalInProgress) {
                        team.revivePlayer();
                        moveToNextPhase();
                        revivalInProgress = false;

                        musicPlayer.time = (float)beatmap.getPhase(currentSection, currentPhase).startTime;
                        musicPlayer.Play();

                        revivalSoundPlayer.Stop();

                        positionInSongTimer = (float)beatmap.getPhase(currentSection, currentPhase).startTime;

                    }


                    else if (positionInSongTimer >= nextPhaseStartTime) {
                        moveToNextPhase();
                    }

                    // after a boss phase, start the next phase immediately (team attack)
                    else if (currentPhase + 1 == beatmap.sections[currentSection].Count) {
                        moveToNextPhase();
                    }
                }

                // wait for the correct phase start time after a team attack ends
                if (teamAttackEnding) {
                    if (positionInSongTimer >= beatmapPhase.startTime) {
                        nextBeatTime = phaseOffset + playerOffset - noteTravelDistance / noteTravelSpeed;
                        noteCreateIndex = 0;
                        phaseTimer = 0;
                        // resets the failed phase status for both players
                        team.nextPhaseBegin();
                        teamAttackEnding = false;
                    }
                }


                else if (phaseTimer > nextBeatTime) {
                    createPlayerNote("left", team.player1);
                    createPlayerNote("right", team.player2);

                    noteCreateIndex++;
                    nextBeatTime = phaseOffset + playerOffset + noteCreateIndex * beatInterval - noteTravelDistance / noteTravelSpeed;
                }

                if (team.player1.activeNotes.Count > 0) {
                    checkPlayerAction(team.player1);
                }

                if (team.player2.activeNotes.Count > 0) {
                    checkPlayerAction(team.player2);
                }
            }
        }
    }
}
