using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pixelplacement;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : InteractOnInteractButton2D
{
    public string Id;
    private string m_currentSceneName;

    public SpriteRenderer waypointLight;
    private Color lightColor;
    //bool activated = true;

    private void Start()
    {
        lightColor = waypointLight.color;

        m_currentSceneName = SceneManager.GetActiveScene().name;

        if (CheckPointManager.Instance.checkPoints.ContainsKey(Id))
        {
            Debug.Log($"Key : {Id} found in checkpoints");
        }

        Tween.Color(waypointLight, waypointLight.color, Color.clear, 4.0f, 0.2f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
    }

    protected override void ExecuteOnEnter(Collider2D other)
    {
        base.ExecuteOnEnter(other);
        /*
        if(!activated)
        {
            Tween.Color(waypointLight, waypointLight.color, lightColor, 4.0f, 0.2f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
            activated = true;
        }
        */
    }

    protected override void ExecuteOnExit(Collider2D other)
    {
        base.ExecuteOnExit(other);
        /*
        if(activated)
        {
            Tween.Color(waypointLight, waypointLight.color, Color.clear, 4.0f, 0.2f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
            activated = false;
        }
        */
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