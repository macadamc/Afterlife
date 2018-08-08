using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using Pixelplacement;
using Sirenix.OdinInspector;

public class CameraShake : Singleton<CameraShake>
{
    new Camera camera;

    private void OnEnable()
    {
        Initialize(this);
        camera = Camera.main;
    }

    public void Update()
    {

    }

    public void Shake(Vector3 intensity, float duration)
    {
        Tween.Shake(camera.transform, camera.transform.position, intensity, duration, 0.0f);
    }

    [ButtonGroup]
    public void SmallShake()
    {
        Shake(Vector3.one * 0.25f, 0.25f);
    }

    [ButtonGroup]
    public void BigShake()
    {
        Shake(Vector3.one, 0.5f);
    }

    [ButtonGroup]
    public void Rumble()
    {
        Shake(Vector3.one * 0.2f, 1f);
    }

    [ButtonGroup]
    public void Earthquake()
    {
        Shake(Vector3.one, 2.5f);
    }
}
