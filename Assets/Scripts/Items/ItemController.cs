using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item currentItem;
    public SpriteRenderer heldItemSpriteRend;
    public Transform itemSpawnTransform;
    [Range(0,360)]
    public int angleSnap = 0;
    public bool logToConsole;
    public Animator animator;
    public bool lockDirectionWhenUsingItem;
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
    bool _init;

    private void OnEnable()
    {
        heldItemSpriteRend.enabled = true;
    }

    private void OnDisable()
    {
        heldItemSpriteRend.enabled = false;
    }

    public void EquipItem(Item item)
    {
        currentItem = Instantiate(item);
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
        if (PauseManager.Instance != null && PauseManager.Instance.Paused)
            return;

        if (currentItem == null)
            return;

        if (_nextPossibleUseTime > Time.time)
            return;
        else if( _init == false)
            Init();

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
                _init = false;
            }
            else
                HoldItem();
        }
    }
    private void StartItem()
    {
        if (lockDirectionWhenUsingItem)
            InputController.strafe = true;
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
        if (lockDirectionWhenUsingItem)
            InputController.strafe = false;
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
    private void Init()
    {
        heldItemSpriteRend.enabled = true;
        _init = true;
    }
}
