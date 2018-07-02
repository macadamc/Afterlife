using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    public Item currentItem;
    public SpriteRenderer heldItemSpriteRend;
    public Transform itemSpawnTransform;
    public bool logToConsole;
    public Animator animator;
    public bool lockToCardinalDirections;
    public bool lockDirectionWhenUsingItem;
    public Vector2 lookDirection = Vector2.right;
    [HideInInspector]
    public float lastX;

    public Transform SpawnTransform
    {
        get
        {
            if (itemSpawnTransform != null)
                return itemSpawnTransform;
            else
                return transform;
        }
    }
    public InputController InputController
    {
        get
        {
            if (_inputController == null)
                _inputController = GetComponentInChildren<InputController>();

            return _inputController;
        }
        set
        {
            _inputController = value;
        }
    }
    public MovementController MovementController
    {
        get
        {
            if (_movementController == null)
                _movementController = GetComponent<MovementController>();

            return _movementController;
        }
    }

    MovementController _movementController;
    InputController _inputController;
    float _nextPossibleUseTime;
    bool _usingItem;

    public void EquipItem(Item item)
    {
        currentItem = item;
        heldItemSpriteRend.sprite = item.onBackSprite;
    }
    public void Unequip()
    {
        if (_usingItem)
            EndItem();

        heldItemSpriteRend.sprite = null;
        currentItem = null;
    }
    public void ApplyItemDelay(float time)
    {
        _nextPossibleUseTime = Time.time + time;
    }
    public void ApplyMoveStun(float time)
    {
        if (MovementController != null)
            MovementController.Stun(time);
    }

    private void Start()
    {
        if (currentItem != null)
            EquipItem(currentItem);
    }
    private void Update()
    {
        if (PauseManager.Instance.Paused)
            return;

        if (InputController.joystick.magnitude > 0.0f)
        {
            if ((lockDirectionWhenUsingItem && !_usingItem) || !lockDirectionWhenUsingItem)
                SetLookDirection();
        }

        if (_nextPossibleUseTime > Time.time)
            return;

        if (heldItemSpriteRend.enabled == false && !_usingItem)
            heldItemSpriteRend.enabled = true;

        if (currentItem == null)
            return;

        if (!_usingItem)
        {
            if (InputController.input.pressed && !MovementController.IsStunned)
            {
                StartItem();
                _usingItem = true;
            }
        }
        else
        {
            if (InputController.input.released || !InputController.input.held)
            {
                EndItem();
                _usingItem = false;
            }
            else
                HoldItem();
        }
    }
    private void StartItem()
    {
        Log("Start Item [" + currentItem.name + "]");
        heldItemSpriteRend.enabled = false;
        currentItem.Begin(this);
    }
    private void HoldItem()
    {
        Log("Hold Item [" + currentItem.name + "]");

        currentItem.Hold(this);
    }
    private void EndItem()
    {
        Log("End Item [" + currentItem.name + "]");
        currentItem.End(this);
        SetAttackTrigger();
    }
    private void Log(string message)
    {
        if (logToConsole)
            Debug.Log(message);
    }
    private void SetAttackTrigger()
    {
        animator.SetTrigger("attack");
    }
    private void SetLookDirection()
    {
        lastX = InputController.joystick.x;

        if (lockToCardinalDirections)
        {
            Vector2 cardinalDirection = InputController.joystick;
            if (Mathf.Abs(cardinalDirection.x) >= Mathf.Abs(cardinalDirection.y))
                cardinalDirection.y = 0;
            else
            if (Mathf.Abs(cardinalDirection.y) >= Mathf.Abs(cardinalDirection.x))
                cardinalDirection.x = 0;

            lookDirection = cardinalDirection.normalized;
        }
        else
        {
            lookDirection = InputController.joystick.normalized;
        }
    }
}
