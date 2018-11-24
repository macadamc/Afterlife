using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public float deathWaitTime;

    public UnityEvent playerDeath;

    protected override void CheckHealth()
    {
        if (currentHealth.Value <= 0)
        {
            events.onDeath.Invoke();

            if (deactivateRootObjectOnDeath)
                gameObject.SetActive(false);
            else
            {
                StartCoroutine(PlayerDead());
            }
        }

    }

    protected IEnumerator PlayerDead()
    {
        PersistentDataManager.SaveExternal("SaveData");
        yield return new WaitForSeconds(deathWaitTime);
        playerDeath.Invoke();
        SceneManager.LoadScene("TitleTitle");
    }
}
