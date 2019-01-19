using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAgent : MovementController
{
    public Vector2 velocity;
    public Vector2 acceleration;
    public FlockingAgentConfig config;
    private Vector2 wanderTarget;
   

    GameObject player;

    private float RandomBinomial
    {
        get
        {
            return Random.Range(0f, 1f) - Random.Range(0f, 1f);
        }

    }

    public virtual void Start()
    {
        FlockingAgentManager.Instance.AddAgent(this);
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public virtual void OnDisable()
    {
        FlockingAgentManager.Instance.RemoveAgent(this);
    }

    public override void Update()
    {
        if (Ic == null)
            return;

        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        if (Knockedback || _stunned)
        {
            if (anim != null)
                anim.SetBool(setAnimationBoolToIsMoving, false);

            // sets move vector to zero
            _moveVector = Vector2.Lerp(_moveVector, Vector2.zero, smoothing);

            // check to see if stun time is over and input controller will regain control
            if (Time.time > _nextMoveTime)
                _stunned = false;
        }
        else
        {
            acceleration = Combine();
            acceleration = Vector2.ClampMagnitude(acceleration, config.maxAcceleration);
            velocity = acceleration;
            velocity = Vector2.ClampMagnitude(velocity, config.maxVelocity);

            // use normal input from InputController.joystick
            _moveVector = Vector2.Lerp(_moveVector, velocity, smoothing);

            if(anim != null)
                anim.SetBool(setAnimationBoolToIsMoving, IsMoving);
        }
        /*
        acceleration = Combine();
        acceleration = Vector2.ClampMagnitude(acceleration, config.maxAcceleration);
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, config.maxVelocity);
        position = position + velocity * Time.deltaTime;

        transform.position = position;
        */
    }

    protected Vector2 Combine()
    {
        return  config.cohesionPriority * Cohesion() +
                config.wanderPriority * Wander() + 
                config.allignmentPriority * Allignment() + 
                config.seperationPriority * Seperation() + 
                config.obstacleAvoidancePriority * Avoidance() +
                config.playerMagnitismPriority * Magnitism() +
                config.inputPriority * Input();
    }

    protected Vector2 Wander()
    {
        float jitter = config.wanderJitter * Time.deltaTime;
        wanderTarget += new Vector2(RandomBinomial * jitter, RandomBinomial * jitter);
        wanderTarget.Normalize();
        wanderTarget *= config.wanderRadius;
        Vector2 targetInLocalSpace = wanderTarget + Ic.joystick;
        Vector2 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
        targetInWorldSpace -= (Vector2)transform.position;
        return targetInWorldSpace.normalized;
    }

    protected Vector2 Cohesion()
    {
        Vector2 cohesionVector = new Vector2();
        int countAgents = 0;

        var neighbors = FlockingAgentManager.Instance.GetNeighbors(this, config.cohesionRadius);
        if (neighbors.Count == 0)
            return cohesionVector;

        foreach (var agent in neighbors)
        {
            if (IsInFOV(agent.transform.position))
            {
                cohesionVector += (Vector2)agent.transform.position;
                countAgents++;
            }
        }
        if (countAgents == 0)
            return cohesionVector;

        cohesionVector /= countAgents;
        cohesionVector = cohesionVector - (Vector2)transform.position;
        cohesionVector.Normalize();
        return cohesionVector;

    }

    protected Vector2 Allignment()
    {
        Vector2 allignVector = new Vector2();
        var agents = FlockingAgentManager.Instance.GetNeighbors(this, config.allignmentRadius);
        if (agents.Count == 0)
            return allignVector;

        foreach (var agent in agents)
        {
            if(IsInFOV(agent.transform.position))
            {
                allignVector += agent.velocity;
            }
        }

        return allignVector.normalized;
    }

    protected Vector2 Seperation()
    {
        Vector2 seperationVector = new Vector2();
        var agents = FlockingAgentManager.Instance.GetNeighbors(this,config.seperationRadius);
        if (agents.Count == 0)
            return seperationVector;

        foreach (var agent in agents)
        {
            if(IsInFOV(agent.transform.position))
            {
                Vector2 movingTowards = transform.position - agent.transform.position;
                if (movingTowards.magnitude > 0)
                    seperationVector += movingTowards.normalized / movingTowards.magnitude;
            }
        }

        return seperationVector.normalized;
    }

    protected Vector2 Avoidance()
    {
        Vector2 avoidVector = new Vector2();
        var enemyList = FlockingAgentManager.Instance.GetEnemies(this, config.obstacleAvoidanceRadius);
        if (enemyList.Count == 0)
            return avoidVector;

        foreach (var enemy in enemyList)
        {
            avoidVector += RunAway(enemy.transform.position);
        }
        return avoidVector.normalized;
    }

    protected Vector2 Magnitism()
    {
        if (player == null)
            player = Player.Instance.gameObject;

        Vector2 magnitismVector = new Vector2();
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > config.playerMagnitismRadius)
            return magnitismVector;

        magnitismVector = (player.transform.position - transform.position);

        return magnitismVector.normalized;
    }

    Vector2 RunAway(Vector2 target)
    {
        Vector2 neededVelocity = ((Vector2)transform.position - target).normalized * config.maxVelocity;
        return neededVelocity - velocity;
    }

    bool IsInFOV(Vector2 vec)
    {
        return Vector2.Angle(velocity, vec - (Vector2)transform.position) <= config.maxFov;
    }

    protected Vector2 Input()
    {
        return Ic.joystick;
    }
}
