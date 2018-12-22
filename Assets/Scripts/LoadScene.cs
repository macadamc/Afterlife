using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName;
    public string startId;
    public Transition transition;

    public void Load()
    {
        TransitionManager.Instance.onTransitionEnd += OnFadeOut;
        TransitionManager.Instance.FadeOut(transition);
    }


    void OnFadeOut()
    {
        TransitionManager.Instance.onTransitionEnd -= OnFadeOut;
        GlobalStorage.Instance.storage.SetValue("checkpoint_id", startId);
        GlobalStorage.Instance.storage.SetValue("checkpoint_scene", sceneName);
        PersistentDataManager.SaveExternal("SaveData");

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
