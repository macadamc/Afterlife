using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System.Linq;

public class TextBoxRef : MonoBehaviour {

    #region Public Vars
    [System.NonSerialized]
    public Canvas canvas;
    public TextBox textBox
    {
        get
        {
            if (_textBox == null)
            {
                _textBox = Instantiate(TextBoxPrefab, canvas.transform, false).GetComponent<TextBox>();
                _textBox.EnabledTextBox();
            }
            return _textBox;
        }
    }
    public string textBoxKey;
    public GameObject TextBoxPrefab;
    public Vector2 textBoxOffset;
    [System.NonSerialized]
    public DialogueRunner dialogueRunner;
    public TextBoxSettings textBoxSettings;
    [System.NonSerialized]
    public Interactable caller;
    public bool useWorldSpaceCanvas = true;
    #endregion

    #region Private Vars
    internal TextBox _textBox;
    internal TextBoxSettings defaultSettings;
    #endregion

    #region UnityMethods
    private void Reset()
    {
        if (string.IsNullOrEmpty(textBoxKey))
        {
            textBoxKey = gameObject.transform.parent.name;
        }
    }

    private void Awake()
    {
        dialogueRunner = GetComponentInChildren<DialogueRunner>();
    }

    private void Start()
    {
        canvas = FindObjectsOfType<Canvas>().Where((Canvas c) => { return c.renderMode == (useWorldSpaceCanvas ? RenderMode.WorldSpace : RenderMode.ScreenSpaceOverlay); }).FirstOrDefault();
        Debug.Assert(canvas != null, "No canavas found with correct render mode set");

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

    private void OnDisable()
    {
        _textBox?.DisableTextbox();
    }

    private void OnApplicationQuit()
    {
        _textBox = null;
    }
    #endregion

    [YarnCommand("CallTextBox")]
    public void CallTextBox(string startNode)
    {
        if (dialogueRunner.isDialogueRunning)
            return;

        CallTextBoxInterrupt(startNode);
    }

    [YarnCommand("CallTextBoxInterrupt")]
    public void CallTextBoxInterrupt(string startNode)
    {
        Clean();
        dialogueRunner.StartDialogue(startNode);
    }

    public void CallTextBoxString(string text)
    {
        CallTextBoxString(dialogueRunner, text);
    }

    public static void CallTextBoxString(DialogueRunner runner, string text)
    {
        if (runner.isDialogueRunning)
            return;

        string prefix = 
            @"title:Inline
---
";
        string node = prefix + text + System.Environment.NewLine + "===";
        runner.Clear();

        runner.AddScript(node);
        // Load all scripts
        if (runner.sourceText != null)
        {
            foreach (var source in runner.sourceText)
            {
                // load and compile the text
                runner.AddScript(source.text);
            }
        }

        runner.StartDialogue("Inline");
    }

    private void Clean()
    {
        textBox.gameObject.SetActive(false);
        textBox?.cTween.Cancel();
        SetTextBoxSettings("default");
        if (caller == null)
            caller = GetComponentInChildren<Interactable>();
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

    public void ForceCloseTextbox()
    {
        if(textBoxSettings.inputType == TextBoxSettings.InputType.Player)
        {
            dialogueRunner.Stop();
            _textBox?.DisableTextbox();
        }
    }
}
