using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleScreen : MonoBehaviour
{
    public Transition transition;
    private bool _waiting = true;
    public GlobalStorageObject DefaultGlobalStorageValues;
    public GameObject LoadButton;
    public GameObject HUD;

    public void StartNewGame()
    {
        if (_waiting)
            return;

        _waiting = true;
        TransitionManager.Instance.onTransitionEnd += NewGame;
        TransitionManager.Instance.FadeOut(transition);
    }

    public void StartLoad()
    {
        if (_waiting)
            return;

        _waiting = true;
        TransitionManager.Instance.onTransitionEnd += LoadGame;
        TransitionManager.Instance.FadeOut(transition);
    }

    public void StartQuit()
    {
        if (_waiting)
            return;

        _waiting = true;
        TransitionManager.Instance.onTransitionEnd += QuitGame;
        TransitionManager.Instance.FadeOut(transition);
    }

    private void Start()
    {
        TransitionManager.Instance.onTransitionEnd += OnFadeIn;
        LoadButton.SetActive(File.Exists(Application.persistentDataPath + "/" + "SaveData.bin"));
        HUD = GameObject.FindGameObjectWithTag("Canvas");
        HUD.SetActive(false);
    }

    private void OnFadeIn()
    {
        TransitionManager.Instance.onTransitionEnd -= OnFadeIn;
        _waiting = false;
    }

    private void NewGame()
    {
        TransitionManager.Instance.onTransitionEnd -= NewGame;
        
        //copy default values.
        GlobalStorage.Instance.storage.CopyValues(DefaultGlobalStorageValues);

        PersistentDataManager.SaveExternal();
        HUD.SetActive(true);
        StartCoroutine(delayedSceneLoad());

    }

    private void LoadGame()
    {
        TransitionManager.Instance.onTransitionEnd -= LoadGame;
        PersistentDataManager.LoadExternal();
        HUD.SetActive(true);
        StartCoroutine(delayedSceneLoad());
    }

    IEnumerator delayedSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        PersistentDataManager.Instance.DoSheduled();
        SceneManager.LoadScene(GlobalStorage.Instance.storage.GetString("checkpoint_scene"), LoadSceneMode.Single);

    }
    private void QuitGame()
    {
        TransitionManager.Instance.onTransitionEnd -= QuitGame;
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
