using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShadyPixel/New Flocking Agent Config File")]
public class FlockingAgentConfig : ScriptableObject
{
    public float maxFov;
    public float maxAcceleration;
    public float maxVelocity;

    // wander
    public float wanderJitter;
    public float wanderRadius;
    public float wanderDistance;
    public float wanderPriority;

    // cohesion
    public float cohesionRadius;
    public float cohesionPriority;

    // allignment
    public float allignmentRadius;
    public float allignmentPriority;

    // seperation
    public float seperationRadius;
    public float seperationPriority;

    // avoidance
    public float obstacleAvoidanceRadius;
    public float obstacleAvoidancePriority;

    // player magnitism
    public float playerMagnitismRadius;
    public float playerMagnitismPriority;

    public float inputPriority;
}
