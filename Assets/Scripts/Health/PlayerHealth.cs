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
        yield return new WaitForSeconds(deathWaitTime);
        var ic = GetComponent<InputController>();
        ic.joystick = Vector2.zero;
        ic.dodge.SetValue(false);
        ic.input.SetValue(false);
        playerDeath.Invoke();

        var hp = Player.Instance.gameObject.GetComponent<PlayerHealth>();
        hp.currentHealth = hp.maxHealth;

        PersistentDataManager.SaveExternal();
        SceneManager.LoadScene("TitleTitle");
    }
}
