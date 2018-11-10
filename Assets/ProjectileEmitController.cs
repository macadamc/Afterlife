using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using ShadyPixel.StateMachine;
using UnityEditor;

public class ProjectileEmitController : StateMachine
{
    [Button(ButtonSizes.Large)]
    void CreateProjectileEmitter()
    {
        GameObject newEmitter = new GameObject("New Projectile Emitter");
        newEmitter.transform.parent = transform;
        var emitter = newEmitter.AddComponent<RadialProjectileEmitter>();
        newEmitter.transform.localPosition = Vector3.zero;

        Selection.activeGameObject = newEmitter;
    }
}
