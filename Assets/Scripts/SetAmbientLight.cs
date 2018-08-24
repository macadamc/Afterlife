using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using Sirenix.OdinInspector;

public class SetAmbientLight : MonoBehaviour {

    public AmbientLightSettings settings;
    public Camera LightCamera
    {
        get
        {
            if(cam == null)
                cam = GameObject.FindGameObjectWithTag("LightCamera").GetComponent<Camera>();
            return cam;
        }
    }
    public SpriteLightKitImageEffect SpriteLightImageEffect
    {
        get
        {
            if(slkie == null)
                slkie = FindObjectOfType<SpriteLightKitImageEffect>();
            return slkie;
        }
    }

    SpriteLightKitImageEffect slkie;
    Camera cam;

    // Use this for initialization
    void Start () {
        UpdateLight();
	}

    [Button]
    public void UpdateLight()
    {
        if (settings == null)
            return;

        LightCamera.backgroundColor = settings.lightColor;
        SpriteLightImageEffect.intensity = settings.lightIntensity;
    }
}
