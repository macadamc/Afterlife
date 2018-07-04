using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class Teleport : TriggerZone
{
    public Transition transition;
    public string doorTag;
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

    protected override void OnEnter(Collider2D collision)
    {
        mc = collision.gameObject.GetComponent<MovementController>();
        if (mc != null)
        {
            mc.Stun( (transition.startDelay*2.0f) + (transition.transitionTime*2.0f) + 0.25f);
        }

        TransitionManager.Instance.onTransitionEnd += OnFadeOut;
        TransitionManager.Instance.FadeOut(transition);

        _teleportObj = collision.gameObject;
        //PauseManager.Instance.Paused = true;
    }

    public void OnFadeOut()
    {
        TeleportToTag(_teleportObj, this, doorTag);
        TransitionManager.Instance.onTransitionEnd -= OnFadeOut;
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
