using UnityEngine;
using System.Collections;

public class GameStartNetwork : MonoBehaviour {
    public GameObject toSpawnClient;

	// Use this for initialization
	void Start () {
        if (Network.isClient)
        {
            Network.Instantiate(toSpawnClient, Vector3.zero, Quaternion.identity, 0);
        }
	
	}
	
}
