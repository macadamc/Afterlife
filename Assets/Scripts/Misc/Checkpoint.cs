using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : InteractOnTrigger2D
{
    public string Id;

    protected override void ExecuteOnEnter(Collider2D other)
    {
        base.ExecuteOnEnter(other);
        GlobalStorage.Instance.storage.SetValue("checkpoint_id", Id);
        GlobalStorage.Instance.storage.SetValue("checkpoint_scene", SceneManager.GetActiveScene().name);
    }
}
