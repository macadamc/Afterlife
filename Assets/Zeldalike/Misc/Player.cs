using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player> {

    public string doorTag;



    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        Initialize(this);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        PrintVar();
        Teleport[] teleporters = FindObjectsOfType<Teleport>();
        foreach(Teleport t in teleporters)
        {
            if(doorTag == t.doorTag)
            {
                transform.position = t.enterTransform.position;
                doorTag = null;
                CameraFollow.Instance.SetPosition(t.TeleportTransform.position, t.cameraZone.bounds);
                return;
            }
        }
    }

    void PrintVar()
    {
        foreach(var item in SaveLoadManager.Instance.savedVariables.variables)
        {
            Debug.Log($"{item.Key} {item.Value}");
        }
        Debug.Log(SaveLoadManager.Instance.savedVariables.variables.Count);
    }
}
