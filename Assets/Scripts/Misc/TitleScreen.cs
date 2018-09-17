using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public Transition transition;
    public string newGameSceneName;
    private bool _waiting = true;

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
    }

    private void OnFadeIn()
    {
        TransitionManager.Instance.onTransitionEnd -= OnFadeIn;
        _waiting = false;
    }

    private void NewGame()
    {
        TransitionManager.Instance.onTransitionEnd -= NewGame;
        //SaveLoadManager.Instance.Save();
        SceneManager.LoadScene(newGameSceneName, LoadSceneMode.Single);
    }

    private void LoadGame()
    {
        TransitionManager.Instance.onTransitionEnd -= LoadGame;
        //SaveLoadManager.Instance.Load();
        SceneManager.LoadScene(newGameSceneName, LoadSceneMode.Single);
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
