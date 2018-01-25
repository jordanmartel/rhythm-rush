using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;
using UnityEngine.SceneManagement;

public class ConnectController : MonoBehaviour {

    private Wiimote wiimote;

    // Use this for initialization
    void Start () { 

    }
	
	// Update is called once per frame
	void Update () {

        if (!WiimoteManager.HasWiimote()) { return; }

        wiimote.ReadWiimoteData();

        // press A and B to start
        if (wiimote.Button.a && wiimote.Button.b) {
            SceneManager.LoadScene("BaseScene_Annie");
        }

    }


    private void OnGUI()
    {
        GUILayout.BeginVertical(GUILayout.Width(300));

        if (!WiimoteManager.HasWiimote()) {
            GUILayout.Label("Please connect your WiiMote");
            WiimoteManager.FindWiimotes();
        }

        else
        {
            wiimote = WiimoteManager.Wiimotes[0];
            wiimote.SendPlayerLED(true, false, false, false);
            GUILayout.Label("Press A and B to start");
        }

    }

}
