using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Image heart1, heart2, heart3;
    public Team team;

	// Use this for initialization
	void Start () {
        team = FindObjectOfType<Team>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (team.health)
        {
            case 0:
                heart1.GetComponent<Image>().color = new Color32(255,255,255,0);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                break;
            case 1:
                heart1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                break;
            case 2:
                heart1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                break;
            case 3:
                heart1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                heart3.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                break;
        }
    }
}
