using UnityEngine;
using System.Collections;

public class LevelTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Network.connections.Length > 0)
        {
            Invoke("LoadLevel", 3);

        }
	
	}

    void LoadLevel()
    {
        Application.LoadLevel(1);
    }
}
