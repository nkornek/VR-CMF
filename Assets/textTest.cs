using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class textTest : MonoBehaviour {
    public int thisInt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = "TestVar: " + thisInt;
	
	}

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            // Sending
            stream.Serialize(ref thisInt);
        }
        else
        {
            // Receiving
            stream.Serialize(ref thisInt);
        }
    }

}
