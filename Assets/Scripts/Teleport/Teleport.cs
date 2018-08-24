using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.SceneManagement;

public class Teleport : InteractOnTrigger2D
{
    public enum Type
    {
        Local, ChangeScene
    }

    public Type type;
    public Transition transition;
    public string doorTag;
    public string sceneName;
    public Transform enterTransform;
    MovementController mc;


    public Transform TeleportTransform
    {
        get
        {
            if (enterTransform != null)
                return enterTransform;
            else
                return transform;
        }
    }

    public Collider2D cameraZone;

    GameObject _teleportObj;

    protected override void ExecuteOnEnter(Collider2D other)
    {
        base.ExecuteOnEnter(other);
        mc = other.gameObject.GetComponent<MovementController>();
        if (mc != null)
        {
            mc.Stun( (transition.startDelay*2.0f) + (transition.transitionTime*2.0f) + 0.25f);
        }

        TransitionManager.Instance.onTransitionEnd += OnFadeOut;
        TransitionManager.Instance.FadeOut(transition);

        _teleportObj = other.gameObject;
        //PauseManager.Instance.Paused = true;
    }

    public void OnFadeOut()
    {
        TransitionManager.Instance.onTransitionEnd -= OnFadeOut;
        if (type == Type.ChangeScene)
        {
            // changing scenes
            Player.Instance.doorTag = doorTag;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        else if (type == Type.Local)
        {
            // in scene teleport
            TeleportToTag(_teleportObj, this, doorTag);
        }
    }

    public void OnFadeIn()
    {
        //PauseManager.Instance.Paused = false;
        TransitionManager.Instance.onTransitionEnd -= OnFadeIn;
    }

    public void TeleportToTag(GameObject objectToTeleport, Teleport startingTeleporter, string tagToLookFor)
    {
        Teleport[] teleporters = FindObjectsOfType<Teleport>();

        foreach(Teleport t in teleporters)
        {
            if(t.doorTag == tagToLookFor && t != startingTeleporter)
            {
                TransitionManager.Instance.onTransitionEnd += OnFadeIn;
                objectToTeleport.transform.position = t.TeleportTransform.position;
                CameraFollow.Instance.SetPosition( t.TeleportTransform.position , t.cameraZone.bounds );
                TransitionManager.Instance.FadeIn(transition);
                if (mc!=null)
                {
                    mc.enabled = true;
                    mc = null;
                }
            }
        }
    }



}
