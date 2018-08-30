﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player> {

    public string doorTag;
    Health health;

    private void OnEnable()
    {
        health = GetComponent<Health>();
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
        Teleport[] teleporters = FindObjectsOfType<Teleport>();
        foreach(Teleport t in teleporters)
        {
            if(doorTag == t.doorTag)
            {
                transform.position = t.enterTransform.position;
                doorTag = null;
                CameraFollow.Instance.SetPosition(t.TeleportTransform.position, t.cameraZone);
                return;
            }
        }
    }
}
