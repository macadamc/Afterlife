using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : InteractOnTrigger2D
{
    public string Id;
    public GlobalStorageObject gso;

    protected override void ExecuteOnEnter(Collider2D other)
    {
        base.ExecuteOnEnter(other);
        gso.SetValue("checkpoint_id", Id);
        gso.SetValue("checkpoint_scene", SceneManager.GetActiveScene().name);
    }
}
