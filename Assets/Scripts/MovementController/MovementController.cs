using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
    [System.Serializable]
    public class Events
    {
        public UnityEvent onKnockback;
        public UnityEvent onDodge;
    }
    [DrawWithUnity]
    public Events events;

    public InputController Ic
    {
        get
        {
            if (_inputController == null)
            {
                _inputController = GetComponentInChildren<InputController>();
            }

            return _inputController;
        }
        set
        {
            _inputController = value;
        }
    }
    public Transform Target
    {
        get
        {
            if (moveTarget != null)
                return moveTarget;
            else
                return transform;
        }
        set
        {
            moveTarget = value;
        }
    }
    public Rigidbody2D Rb
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = Target.GetComponent<Rigidbody2D>();

            return _rigidbody;
        }
    }

    public bool Knockedback
    {
        get
        {
            return _knockbackVector.magnitude > knockbackDeadzone;
        }
    }
    public virtual bool IsMoving
    {
        get
        {
            return _moveVector.magnitude > 0.01f;
        }
    }
    public bool IsStunned
    {
        get
        { return _stunned; }
    }
    public Vector2 GetMoveVector
    {
        get { return _moveVector; }
    }

    public Animator anim;
    public string setAnimationBoolToIsMoving = "isMoving";
    public Transform moveTarget;
    public FloatReference moveSpeed = new FloatReference(5f);
    [HideInInspector]
    public FloatReference oldMoveSpeed;
    [Range(0.01f, 1f)]
    public float smoothing = 0.4f;
    public float knockbackDecel = 5f;
    public float knockbackDeadzone = 0.1f;
    public float dodgeDecel = 5f;
    public float dodgeDeadzone = 0.1f;

    protected InputController _inputController;
    protected Vector2 _moveVector;
    public Vector2 _knockbackVector;
    public Vector2 _dodgeVector;

    protected Rigidbody2D _rigidbody;
    protected float _nextMoveTime;
    protected bool _stunned;
    protected bool _initialized;

    public virtual void OnEnable()
    {
        //sets the startposition of this object. if already set will reset to it when re enabled
        if (!_initialized)
        {
            _initialized = true;
        }
    }

    /// <summary>
    /// movement logic should go here.
    /// </summary>
    public virtual void Update()
    {
        if (Ic == null)
            return;

        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        if (Knockedback || _stunned)
        {
            if(anim != null && setAnimationBoolToIsMoving.Length > 0)
                anim.SetBool(setAnimationBoolToIsMoving, false);

            // sets move vector to zero
            _moveVector = Vector2.Lerp(_moveVector, Vector2.zero * moveSpeed, smoothing);

            // check to see if stun time is over and input controller will regain control
            if (Time.time >= _nextMoveTime)
                _stunned = false;
        }
        else
        {
            // use normal input from InputController.joystick
            _moveVector = Vector2.Lerp(_moveVector, Ic.joystick * moveSpeed, smoothing);
            if(anim!=null)
                anim.SetBool(setAnimationBoolToIsMoving, IsMoving);
        }
    }

    /// <summary>
    /// takes movement vector and adds it to knockback vector.
    /// calls updateknockback()
    /// </summary>
    public virtual void FixedUpdate()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        Rb.velocity = _moveVector + _knockbackVector + _dodgeVector;
        UpdateKnockback();
        UpdateDodge();

    }

    /// <summary>
    /// logic that updates the knockback vector.
    /// </summary>
    public virtual void UpdateKnockback()
    {
        if(_knockbackVector.magnitude > 0)
        {
            _knockbackVector *= (1 - Time.fixedDeltaTime * knockbackDecel);

            if (_knockbackVector.magnitude <= float.Epsilon)
                _knockbackVector = Vector2.zero;
        }
    }

    public virtual void UpdateDodge()
    {
        if (_dodgeVector.magnitude > 0)
        {
            _dodgeVector *= (1 - Time.fixedDeltaTime * dodgeDecel);

            if (_dodgeVector.magnitude <= float.Epsilon)
                _dodgeVector = Vector2.zero;
        }
    }

    /// <summary>
    /// adds knockback to the current knockback vector
    /// </summary>
    /// <param name="knockback"></param>
    public virtual void AddKnockback(Vector2 knockback)
    {
        _knockbackVector += knockback;
    }

    /// <summary>
    /// sets knockback vector to a vector
    /// </summary>
    /// <param name="knockback"></param>
    public virtual void SetKnockback(Vector2 knockback)
    {
        _knockbackVector = knockback;
    }

    /// <summary>
    /// sets a stun time for the player.
    /// </summary>
    /// <param name="time"></param>
    public virtual void Stun(float time)
    {
        if(_stunned)
        {
            // if stunned already, just add on the extra time
            _nextMoveTime += time;
        }
        else
        {
            // if not already stunned must add Time.time also to the variable
            _stunned = true;
            _nextMoveTime = Time.time + time;
        }
    }

    public virtual void Stun(float time,bool instant)
    {
        if (instant)
        {
            _moveVector = Vector2.zero;
            _stunned = true;
        }

        if (_stunned)
        {
            // if stunned already, just add on the extra time
            _nextMoveTime += time;
        }
        else
        {
            // if not already stunned must add Time.time also to the variable
            _stunned = true;
            _nextMoveTime = Time.time + time;
        }
    }

    public void StunCancel()
    {
        _stunned = false;
        _nextMoveTime = Time.time;
    }
}
