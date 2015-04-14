using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectionInfo : MonoBehaviour {
    public Text UI;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        UI.text = "Connected Users: " + Network.connections.Length;
	
	}
}
