using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player> {

    public PersistentVariableStorage pvs;
    Health health;
    private void Awake()
    {
        health = GetComponent<Health>();
        pvs = GetComponent<PersistentVariableStorage>();
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
        if (string.IsNullOrEmpty(pvs.storage.GetString("doorTag")) == false)
        {
            Debug.Log("Scene Loaded, Teleporting to Door");
            Teleport[] teleporters = FindObjectsOfType<Teleport>();
            foreach (Teleport t in teleporters)
            {
                if (pvs.storage.strings.ContainsKey("doorTag"))
                {
                    if (pvs.storage.strings["doorTag"] == t.doorTag)
                    {
                        Debug.Log("Found doortag");
                        transform.position = t.enterTransform.position;
                        pvs.storage.RemoveValue("doorTag");
                        return;
                    }
                }
            }
            
            Debug.Log("No Door Tag in Persistant Variable Storage");
        }
        else
        {
            Debug.Log("Scene Loaded, Teleporting to Checkpoint");

            Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

            foreach (Checkpoint t in checkpoints)
            {
                if (pvs.storage.strings.ContainsKey("checkpoint_id"))
                {
                    if (pvs.storage.strings["checkpoint_id"] == t.Id)
                    {
                        Debug.Log("Found checkpoint_id");
                        transform.position = t.transform.position;
                        break;
                    }
                }
            }

            StartCoroutine(ResetHealth());

            //Debug.Log("No CheckPoint in Persistant Variable Storage");
        }
    }

    public IEnumerator ResetHealth()
    {
        yield return new WaitForEndOfFrame();
        health.currentHealth.Value = health.maxHealth.Value;
        Debug.Log(health.currentHealth.Value);
    } 

    public void TeleportPlayer(string DoorTag)
    {
        foreach (Teleport t in FindObjectsOfType<Teleport>())
        {
            if (DoorTag == t.doorTag)
            {
                transform.position = t.enterTransform.position;
                return;
            }

        }
    }

    public void EquipItem(Item item)
    {
        var inventory = GetComponent<Inventory>();

        if(inventory.itemSet.Items.Contains(item) == false)
            inventory.AddItem(item);

        inventory.ItemController.EquipItem(inventory.itemSet.Items[inventory.itemSet.Items.Count - 1]);
    }

    public void PlayerColor(Color c)
    {
        this.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = c;
    }

    public void SetString(string key, string val)
    {
        pvs.storage.SetValue(key, val);
    }
    public void SetNumber(string key, float val)
    {
        pvs.storage.SetValue(key, val);
    }
    public void SetBool(string key, bool val)
    {
        pvs.storage.SetValue(key, val);
    }
}
