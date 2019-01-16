using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class Player : Singleton<Player>
{

    Health health;
    MovementController mc;
    InputController ic;
    ItemController itemController;
    Coroutine roll;

    public float dodgeForce = 20f;
    [PropertyRange(0f, 1f)]
    public float controlRegainThreshold = .5f;
    public float cooldownTime = 1f;
    private float nextRollTime;

    private void Awake()
    {
        health = GetComponent<Health>();
        mc = GetComponent<MovementController>();
        ic = GetComponent<InputController>();
        itemController = GetComponent<ItemController>();

        Initialize(this);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        health.onHealthChanged += PlayerHealthChanged;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        health.onHealthChanged -= PlayerHealthChanged;
    }

    private void Update()
    {
        if(ic.dodge.pressed && roll == null)
        {
            roll = StartCoroutine(DodgeRoll());
            mc.events.onDodge.Invoke();
        }
    }

    private IEnumerator DodgeRoll()
    {
        float startTime = Time.time;
        nextRollTime = startTime + cooldownTime;
        float force = dodgeForce;
        health.invincible = true;
        Vector2 stick = ic.joystick;
        if(stick.magnitude != 0)
        {
            stick = stick.normalized;
        }
        else
        {
            stick = -ic.lookDirection.normalized;
            force *= .6f;
        }

        mc.Stun(float.MaxValue);
        mc._dodgeVector = stick * force;
        ic.strafe = true;
        yield return new WaitUntil(() => { return mc._dodgeVector.magnitude <= force * controlRegainThreshold; } );
        mc._dodgeVector = Vector2.zero;
        health.invincible = false;
        yield return new WaitForSeconds(.15f);
        mc.StunCancel();

        if (itemController._usingItem)
            ic.strafe = itemController.currentItem.strafeLockedWhileHeld;
        else
            ic.strafe = false;

        yield return new WaitUntil(() => { return Time.time >= nextRollTime; });
        roll = null;
        
    }

    void PlayerHealthChanged(int change)
    {
        if(change < 0)
        {
            //CameraShake.Instance.SmallShake();
            PauseManager.Instance.StartCoroutine(PauseManager.Instance.FreezeFrame(0.2f,0.3f));
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log($"Loaded Scene {arg0.name}");

        if (string.IsNullOrEmpty(GlobalStorage.Instance.storage.GetString("doorTag")) == false)
        {
            Debug.Log("Scene Loaded, Teleporting to Door");
            Teleport[] teleporters = FindObjectsOfType<Teleport>();
            foreach (Teleport t in teleporters)
            {
                if (GlobalStorage.Instance.storage.strings.ContainsKey("doorTag"))
                {
                    if (GlobalStorage.Instance.storage.strings["doorTag"] == t.Id)
                    {
                        Debug.Log("Found doortag");
                        transform.position = t.enterTransform.position;
                        GlobalStorage.Instance.storage.RemoveValue("doorTag");
                        return;
                    }
                }
            }
            
            Debug.Log("No Door Tag in Persistant Variable Storage");
        }
        else
        {
            Debug.Log("Looking For Checkpoint");
            Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

            if (checkpoints.Length > 0)
            {
                foreach (Checkpoint t in checkpoints)
                {
                    if (GlobalStorage.Instance.storage.strings.ContainsKey("checkpoint_id"))
                    {
                        if (GlobalStorage.Instance.storage.strings["checkpoint_id"] == t.Id)
                        {
                            Debug.Log("Found checkpoint_id in scene, teleporting Player.");
                            transform.position = t.transform.position;
                            break;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("No CheckPoints in scene.");
            }
        }
    }

    public void TeleportPlayer(string Id)
    {
        foreach (Teleport t in FindObjectsOfType<Teleport>())
        {
            if (Id == t.Id)
            {
                transform.position = t.enterTransform.position;
                return;
            }

        }
    }
}
