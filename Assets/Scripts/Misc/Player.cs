using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player> {

    //public PersistentVariableStorage pvs;
    Health health;
    private void Awake()
    {
        health = GetComponent<Health>();
        //pvs = GetComponent<PersistentVariableStorage>();
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

    public void PlayerColor(Color c)
    {
        this.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = c;
    }

    public void SetString(string key, string val)
    {
        GlobalStorage.Instance.storage.SetValue(key, val);
    }
    public void SetNumber(string key, float val)
    {
        GlobalStorage.Instance.storage.SetValue(key, val);
    }
    public void SetBool(string key, bool val)
    {
        GlobalStorage.Instance.storage.SetValue(key, val);
    }
}
