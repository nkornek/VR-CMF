using UnityEngine;
using System.Collections;

public class GameStartNetwork : MonoBehaviour {
    public GameObject toSpawn;

	// Use this for initialization
	void Start () {
        if (UDPSender.networkManager.isHost)
        {
            Network.Instantiate(toSpawn, Vector3.zero, Quaternion.identity, 0);
        }
	
	}
	
}
