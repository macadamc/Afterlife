using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : InteractOnInteractButton2D
{
    public string Id;
    private string m_currentSceneName;

    private void Start()
    {
        m_currentSceneName = SceneManager.GetActiveScene().name;

        if (CheckPointManager.Instance.checkPoints.ContainsKey(Id))
        {
            Debug.Log($"Key : {Id} found in checkpoints");
        }
    }

    protected override void OnInteractButtonPress()
    {
        base.OnInteractButtonPress();

        GlobalStorage.Instance.storage.SetValue("checkpoint_id", Id);
        GlobalStorage.Instance.storage.SetValue("checkpoint_scene", m_currentSceneName);

        if (CheckPointManager.Instance.checkPoints.ContainsKey(Id) == false)
        {
            CheckPointManager.Instance.checkPoints.Add(Id, m_currentSceneName);
        }

        var hp = Player.Instance.gameObject.GetComponent<PlayerHealth>();
        hp.currentHealth.Value = hp.maxHealth.Value;

        PersistentDataManager.SaveExternal();

        Debug.Log($"checkpoints Visited : {CheckPointManager.Instance.checkPoints.Count}");
        Debug.Log("Bringing up Teleport Menu....");
    }
}