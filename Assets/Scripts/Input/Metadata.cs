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
}
