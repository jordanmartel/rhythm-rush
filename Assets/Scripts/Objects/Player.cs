using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject directionalTrack;
    public GameObject faceTrack;
    public GameObject hitArea;
    public GameObject directionalMissBox;
    public GameObject faceMissBox;
    public GameObject feedback;

    public int accumulatedDamage;
    public int combo;
    public int joystick;

    public float previousDpadHorizontal;
    public float previousDpadVertical;
    public string previousButton;

    public Dictionary<string, string> notes;
    public List<GameObject> activeNotes;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int calculateComboDamage(int comboThreshold)
    {
        return accumulatedDamage + (((combo / comboThreshold) - 1) * accumulatedDamage);
    }

    
}
