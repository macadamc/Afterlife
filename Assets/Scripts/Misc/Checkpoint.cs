using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pixelplacement;
using Yarn.Unity;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : InteractOnInteractButton2D
{
    public string Id;
    private string m_currentSceneName;

    public SpriteRenderer waypointLight;
    public GameObject particles;
    public GameObject dialouge;
    private DialogueRunner runner;
    public string Node;
    public Transition transition;

    private Color lightColor;
    bool activated = false;

    private void Start()
    {
        lightColor = waypointLight.color;
        m_currentSceneName = SceneManager.GetActiveScene().name;
        Init();
    }

    public void Init()
    {
        activated = CheckPointManager.Instance.checkPoints.Contains(Id);

        dialouge.SetActive(activated);
        particles.SetActive(activated);

        if (activated)
        {
            Debug.Log($"Key : {Id} found in checkpoints");

        }
        else
        {
            waypointLight.color = Color.clear;

        }
    }
    protected override void OnInteractButtonPress()
    {
        if (runner == null)
            runner = dialouge.GetComponent<DialogueRunner>();

        if (runner == null || runner.isDialogueRunning)
            return;

        base.OnInteractButtonPress();
        GlobalStorage.Instance.storage.SetValue("checkpoint_id", Id);
        GlobalStorage.Instance.storage.SetValue("checkpoint_scene", m_currentSceneName);

        var hp = Player.Instance.gameObject.GetComponent<PlayerHealth>();
        hp.currentHealth.Value = hp.maxHealth.Value;

        if (activated == false)
        {
            CheckPointManager.Instance.checkPoints.Add(Id);
            Tween.Color(waypointLight, waypointLight.color, lightColor, .2f, 0.2f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
            particles.SetActive(true);
            dialouge.SetActive(true);
            activated = true;
        }
        else
        {
            StartCoroutine(TeleportToPrompt());
        }

        PersistentDataManager.SaveExternal();
    }

    IEnumerator TeleportToPrompt()
    {
        bool dialougeComplete;
        var runner = dialouge.GetComponent<DialogueRunner>();
        var textboxref = dialouge.GetComponent<TextBoxRef>();
        textboxref.CallTextBox(Node);
        yield return new WaitUntil(() => { return runner.isDialogueRunning == false; });
    }
}