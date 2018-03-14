using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider manabar;
    public Image manabarFill;
    public Player player;

    public Color manaColor;
    public Color fullColor;

    // Use this for initialization
    void Start()
    {
        manabar.value = 0;
        if (Metadata.playerManaLevels.ContainsKey(player.joystick)) {
            player.pUpCombo = Metadata.playerManaLevels[player.joystick];
        }

        // hit area does not need mana bar
        if (player.powerUp == 0) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // it will be 1 if the pUpCombo is equal to the required combo
        manabar.value = ((float) player.pUpCombo / (int) player.powerUp);
        if (!Metadata.playerManaLevels.ContainsKey(player.joystick))
        {
            Metadata.playerManaLevels.Add(player.joystick, player.pUpCombo);
        }

        else
        {
            Metadata.playerManaLevels[player.joystick] = player.pUpCombo;
        }

        if (manabar.value != 1)
        {
            manabarFill.color = manaColor;
        }

        else
        {
            manabarFill.color = fullColor;
        }
    }
}
