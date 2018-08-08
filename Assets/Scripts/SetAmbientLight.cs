using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using Sirenix.OdinInspector;

public class SetAmbientLight : MonoBehaviour {

    public AmbientLightSettings settings;

	// Use this for initialization
	void Start () {
        UpdateLight();
	}

    [Button]
    public void UpdateLight()
    {
        Camera lightCamera = GameObject.FindGameObjectWithTag("LightCamera").GetComponent<Camera>();
        lightCamera.backgroundColor = settings.lightColor;
        SpriteLightKitImageEffect slkie = FindObjectOfType<SpriteLightKitImageEffect>();
        slkie.intensity = settings.lightIntensity;
    }
}
