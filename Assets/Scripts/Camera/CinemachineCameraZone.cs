using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraZone : InteractOnTrigger2D
{
    public bool slowdownEffectOnPlayerEnter = true;

    // this object gets enabled / disabled as the player enters and exits the trigger.
    CinemachineVirtualCamera vCam;
    private void Start()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        vCam.gameObject.SetActive(false);
    }

    protected override void ExecuteOnEnter(Collider2D other)
    {
        // only the player can trigger
        if (!other.gameObject.CompareTag(Tags.Player))
            return;

        // calls some unity events on base class
        base.ExecuteOnEnter(other);

        //turns ON virtual camera
        vCam.gameObject.SetActive(true);

        if(slowdownEffectOnPlayerEnter)
            StartCoroutine(PauseManager.Instance.LerpTimeScaleOverTime(0.0f, 1.0f, 1.0f));
    }

    protected override void ExecuteOnExit(Collider2D other)
    {
        //this is all same as above but exit events / turning OFF virtual camera
        if (!other.gameObject.CompareTag(Tags.Player))
            return;
        base.ExecuteOnExit(other);
        vCam.gameObject.SetActive(false);
    }
}
