using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Image heart1, heart2, heart3, heart4, heart5;
    public Player player;

	// Use this for initialization
	void Start () {
        //team = FindObjectOfType<Team>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (player.health)
        {
            case 0:
                heart1.GetComponent<Image>().color = new Color32(255,255,255,0);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart4.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart5.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                break;
            case 1:
                heart1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart4.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart5.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                break;
            case 2:
                heart1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart4.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart5.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                break;
            case 3:
                heart1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart4.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart5.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                break;
            case 4:
                heart1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart4.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart5.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                break;
            case 5:
                heart1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart4.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart5.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                break;
        }
    }
}
