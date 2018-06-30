using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPC : MonoBehaviour
{
    TextBoxRef textBoxController;

    public string characterName = "";

    public string talkToNode = "";

    [Header("Optional")]
    public TextAsset scriptToLoad;

    // Use this for initialization
    void Start()
    {
        textBoxController = GetComponent<TextBoxRef>();
        if (scriptToLoad != null)
        {
            textBoxController.dialogueRunner.AddScript(scriptToLoad);
        }

    }
}