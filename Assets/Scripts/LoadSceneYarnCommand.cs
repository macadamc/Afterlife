using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class LoadSceneYarnCommand : MonoBehaviour {

	[YarnCommand("LoadScene")]
    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
}
