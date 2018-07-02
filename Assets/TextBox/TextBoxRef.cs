using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TextBoxRef : MonoBehaviour {

    public Canvas canvas;

    internal TextBox _textBox;
    public TextBox textBox
    {
        get
        {
            if (_textBox == null)
            {
                _textBox = Instantiate(TextBoxPrefab, canvas.transform, false).GetComponent<TextBox>();
            }
            return _textBox;
        }
    }

    public GameObject TextBoxPrefab;
    public Vector2 textBoxOffset;
    public DialogueRunner dialogueRunner;
    public TextBoxSettings textBoxSettings;
    internal TextBoxSettings defaultSettings;

    private void Awake()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
        defaultSettings = textBoxSettings;
    }

    private void LateUpdate()
    {
        if (_textBox == null)
            return;

        _textBox.transform.position = new Vector3(
                    transform.position.x + textBoxOffset.x,
                    transform.position.y + textBoxOffset.y,
                    transform.position.z);
    }

    [YarnCommand("CallTextBox")]
    public void CallTextBox(string startNode)
    {
        dialogueRunner.StartDialogue(startNode);
    }

    [YarnCommand("SetTextBoxSettings")]
    public void SetTextBoxSettings(string name)
    {
        if(name.ToLower() == "default")
        {
            textBoxSettings = defaultSettings;
        }
        else
        {
            textBoxSettings = Instantiate(Resources.Load<TextBoxSettings>($"TextBoxSettings/{name}"));
        }
        
    }
}
