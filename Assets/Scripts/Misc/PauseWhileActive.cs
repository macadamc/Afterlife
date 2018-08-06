using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseWhileActive : MonoBehaviour
{
    private void OnEnable()
    {
        PauseManager.Instance.Paused = true;
    }

    private void OnDisable()
    {
        PauseManager.Instance.Paused = false;
    }

}
