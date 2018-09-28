using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingEnemy : MonoBehaviour {

    void Start()
    {
        FlockingAgentManager.Instance.AddEnemy(transform);
    }

    void OnDisable()
    {
        FlockingAgentManager.Instance.RemoveEnemy(transform);
    }

}
