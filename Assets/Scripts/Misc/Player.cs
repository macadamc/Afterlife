using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player> {

    PersistentVariableStorage pvs;
    //public string doorTag;
    Health health;

    private void OnEnable()
    {
        health = GetComponent<Health>();
        pvs = GetComponent<PersistentVariableStorage>();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        health.onHealthChanged += PlayerHealthChanged;
        Initialize(this);
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
            CameraShake.Instance.SmallShake();
            PauseManager.Instance.StartCoroutine(PauseManager.Instance.FreezeFrame(0.2f,0.3f));
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("Scene Loaded, trying to teleport");
        Teleport[] teleporters = FindObjectsOfType<Teleport>();
        foreach(Teleport t in teleporters)
        {
            if (pvs.storage.strings.ContainsKey("doorTag"))
            {
                if(pvs.storage.strings["doorTag"] == t.doorTag)
                {
                    Debug.Log("Found doortag");
                    transform.position = t.enterTransform.position;
                    //CameraFollow.Instance.SetPosition(t.TeleportTransform.position, t.cameraZone);
                    return;
                }
                else
                {
                    Debug.Log("Could not find corrent door tag in scene.");
                }
            }
            else
            {
                Debug.Log("No Door Tag in Persistant Variable Storage");
                //CameraFollow.Instance.SetPosition(transform.position);
            }
        }
    }
}
