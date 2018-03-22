using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider manabar;
    public Image manabarFill;
    public GameObject launchIndicator;
    public Player player;

    //public Color manaColor;
    //public Color fullColor;

    public Sprite manabarFillSprite;
    public Sprite manabarFilledSprite;

    private bool filledColour = true;
    private float fullTimer = 0;

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
            //Debug.Log(player.pUpCombo);
            Metadata.playerManaLevels[player.joystick] = player.pUpCombo;
        }

        if (manabar.value != 1)
        {
            manabarFill.sprite = manabarFillSprite;
            launchIndicator.SetActive(false);

        }

        else
        {
            launchIndicator.SetActive(true);

            fullTimer += Time.deltaTime;
            if (fullTimer > 0.5)
            {
                filledColour = !filledColour;
                fullTimer = 0;
            }

            if (filledColour)
            {
                manabarFill.sprite = manabarFilledSprite;
            }

            else
            {
                manabarFill.sprite = manabarFillSprite;

            }
        }
    }
}
