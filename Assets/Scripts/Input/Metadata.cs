using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Metadata {

    public static int player1Joystick = 1;
    public static int player2Joystick = 2;

    public static PowerUpHandler.powerUp P1PowerUp = PowerUpHandler.powerUp.None;
    public static PowerUpHandler.powerUp P2PowerUp = PowerUpHandler.powerUp.None;

    // preserve mana level across stages so they do not have to start again from 0
    public static Dictionary<int, int> playerManaLevels = new Dictionary<int, int>();

    public static string nextStage = "";
    public static string currentStage = "";



    //P1 Stats
    public static int P1maxCombo;
    public static int P1failCount;
    public static int P1perfectCount;
    public static int P1stunCount;
    public static int P1reviveCount;
    public static int P1score;
    //P2 Stats
    public static int P2maxCombo;
    public static int P2failCount;
    public static int P2perfectCount;
    public static int P2stunCount;
    public static int P2reviveCount;
    public static int P2score;


    public static void resetStats() {
        P1maxCombo= 0;
        P1failCount= 0;
        P1perfectCount= 0;
        P1stunCount= 0;
        P1reviveCount= 0;
        P1score= 0;
    
        P2maxCombo= 0;
        P2failCount= 0;
        P2perfectCount= 0;
        P2stunCount= 0;
        P2reviveCount= 0;
        P2score= 0;
        nextStage = "";
        currentStage = "";
    }
}
