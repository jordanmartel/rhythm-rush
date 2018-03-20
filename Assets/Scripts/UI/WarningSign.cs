using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningSign : MonoBehaviour {
    private double timer;
    private const double appearTime = 0.5;
    private const double disappearTime = 1;
    private const int maxFlashTime = 3;
    private int flashTime = 0;
    public Image sign;
	// Use this for initialization
	void Start () {
        timer = 0;
        flashTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= disappearTime)
        {
            sign.enabled = true;
            timer = 0;
            flashTime++;
            
        }
        else if (timer >= appearTime)
        {
            sign.enabled = false;
        }
        if (flashTime >= maxFlashTime)
        {
            Destroy(gameObject);
        }
	}
}
