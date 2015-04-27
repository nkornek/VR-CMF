using UnityEngine;
using System.Collections;

public class SkyboxControl : MonoBehaviour {
    public Material skybox;
    [Range(0, 1)]
    public float sunSize;
    [Range(0, 5)]
    public float atmosphereThickness;
    public Color skyTint, ground;
    [Range(0, 8)]
    public float exposure;

    void Start()
    {
        sunSize = skybox.GetFloat("_SunSize");
        atmosphereThickness = skybox.GetFloat("_AtmosphereThickness");
        skyTint = skybox.GetColor("_SkyTint");
        ground = skybox.GetColor("_GroundColor");
        exposure = skybox.GetFloat("_Exposure");
    }

	// Update is called once per frame
	void Update () {
        skybox.SetFloat("_SunSize", sunSize);
        skybox.SetFloat("_AtmosphereThickness", atmosphereThickness);
        skybox.SetColor("_SkyTint", skyTint);
        skybox.SetColor("_GroundColor", ground);
        skybox.SetFloat("_Exposure", exposure);
	}
}
