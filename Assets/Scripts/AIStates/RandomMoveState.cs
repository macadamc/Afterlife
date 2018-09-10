using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;
using Sirenix.OdinInspector;

public class RandomMoveState : State {

    [MinMaxSlider(0f, 10f, true)]
    public Vector2 moveTime;

    [MinMaxSlider(0f, 10f, true)]
    public Vector2 delayTime;

    public InputController inputController;
    public MovementController movementControler;
    public GameObject nextState;

    public bool useRaycasting;
    [ShowIf("useRaycasting")]
    public LayerMask physicsLayers;

    private float _nextMoveTime;
    private bool _delayed;
    private bool _joystickSet;
    private bool _delayedSet;

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    protected override void OnDisable()
    {
        Init();
        base.OnDisable();
    }

    private void Init()
    {
        if (inputController == null)
            inputController = GetComponentInChildren<InputController>();

        movementControler = inputController.GetComponent<MovementController>();

        inputController.joystick = Vector2.zero;
        inputController.input.SetValue(false);
        _delayed = false;
        _delayedSet = false;
    }


    private void FixedUpdate()
    {
        if(useRaycasting && PathBlocked())
        {
            inputController.joystick = Vector2.zero;
        }

        if (_delayed)
        {
            // if delay input has not been set
            if (!_delayedSet)
            {
                _delayedSet = true;
                _joystickSet = false;
                inputController.joystick = Vector2.zero;
                UpdateMoveTime(delayTime.x, delayTime.y);
            }
        }
        else
        {
            // if movement input has not been set yet
            if (inputController.joystick == Vector2.zero)
            {
                inputController.joystick = Random.insideUnitCircle;

                if (useRaycasting && PathBlocked())
                {
                    int trys = 0;
                    while (PathBlocked() && trys < 10)
                    {
                        trys++;
                        inputController.joystick = Random.insideUnitCircle;
                    }
                }

                if (!_joystickSet)
                {
                    _joystickSet = true;
                    _delayedSet = false;
                    UpdateMoveTime(moveTime.x, moveTime.y);
                }
                
            }
        }

        // if it is time for the delayed bool to flip or if we cant ove forward;
        if (Time.time >= _nextMoveTime)
        {
            _delayed = !_delayed;
        }

    }

    private void UpdateMoveTime(float min, float max)
    {
        _nextMoveTime = Time.time + Random.Range(min, max);
    }

    private void UpdateMoveTime(float time)
    {
        _nextMoveTime = Time.time + time;
    }

    protected List<RaycastHit2D> GetAjacentWalkableTiles()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        Vector3[] positions = new Vector3[4]
        {
            transform.position + (Vector3.up /2f),
            transform.position + (Vector3.right/2f),
            transform.position - (Vector3.up/2f),
            transform.position - (Vector3.right/2f)
        };

        foreach(Vector3 pos in positions)
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, -Vector2.up, 1f, physicsLayers);

            if (hit.collider != null)
                hits.Add(hit);
        }

        return hits;
    }

    bool PathBlocked()
    {
        Vector3 pos = ((Vector3)Vector2.Lerp(movementControler.GetMoveVector, inputController.joystick * movementControler.moveSpeed, movementControler.smoothing)) + movementControler.transform.position;
        Physics2D.queriesStartInColliders = true;
        //return Physics2D.Raycast(pos, -Vector2.up, .1f, physicsLayers);
        return Physics2D.CircleCastAll(pos, .48f, -Vector2.up, .1f, physicsLayers).Length > 0;
    }
}
