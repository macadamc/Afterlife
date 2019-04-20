using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckHealth : MonoBehaviour {

    public bool checkHealthOnEnable = true;
    public int amount = 3;
    public UnityEvent OnCurrentHealthLessThanTargetAmount;

    Health healthComponent;

    private void OnEnable()
    {
        healthComponent = GetComponentInParent<Health>();

        if (checkHealthOnEnable)
            Check();
    }

    public void Check()
    {
        if(healthComponent.currentHealth.Value <= amount)
        {
            OnCurrentHealthLessThanTargetAmount.Invoke();
        }
    }
}
