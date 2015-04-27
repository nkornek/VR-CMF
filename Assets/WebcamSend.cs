using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class WebcamSend : MonoBehaviour
{

    WebCamDevice usedCamera;
    bool camOn;
    public RawImage frame;
    WebCamTexture webcamTexture;
    public NetworkView nView;
    NetworkPlayer otherPlayer;
    public GameObject pixelTracker;
    GameObject[] trackers;
    public Transform pixelTrackerParent;

	// Use this for initialization
	void Start () {
        if (Network.isClient)
        {
            Invoke("StartCamera", 0.5f);
        }
        pixelTrackerParent = GameObject.Find("Pixel Trackers").transform;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (camOn)
        {
            PixToTransform();
        }
        trackers = GameObject.FindGameObjectsWithTag("PixelTrackers");
        foreach (GameObject t in trackers)
        {
            if (t.transform.parent == null)
            {
                t.transform.parent = pixelTrackerParent;
            }
        }
        if (Network.isServer && trackers.Length == 320*240)
        {            
            ReadTransforms();
            print("trying to receive");
        }

	
	}

    void StartCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (var i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
            Debug.Log(devices[i].isFrontFacing);
        }
        for (int i = 0; i < devices.Length; i++)
        {
           
            if (devices[i].isFrontFacing)
            {
                usedCamera = devices[i];
                break;
            }
            else
            {
                usedCamera = devices[0];
            }
        }
        webcamTexture = new WebCamTexture(usedCamera.name, 320, 240, 60);
        frame.texture = webcamTexture;
        webcamTexture.Play();
        Color[] camPixels = webcamTexture.GetPixels(0 , 0 , 320, 240);
        for (int i = 0; i < camPixels.Length; i++)
        {
            Network.Instantiate(pixelTracker, transform.position, Quaternion.identity, 0);
        }
        camOn = true;
    }

    void PixToTransform()
    {
        Color[] camPixels = webcamTexture.GetPixels();
        for (int i = 0; i < camPixels.Length; i++)
        {
            trackers[i].transform.position = new Vector3(camPixels[i].r, camPixels[i].g, camPixels[i].b);
        }
    }

    void ReadTransforms()
    {
        Color[] camPixels = new Color[320*240];
        for (int i = 0; i < camPixels.Length; i++)
        {
            camPixels[i] = new Color(trackers[i].transform.position.x, trackers[i].transform.position.y, trackers[i].transform.position.z, 1);
        }
        Texture2D newTex = new Texture2D(320, 240);
        newTex.SetPixels(camPixels);
        newTex.Apply();
        frame.texture = newTex;
    }
  
}
