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

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        canvas = FindObjectsOfType<Canvas>().Where((Canvas c) => { return c.renderMode == (useWorldSpaceCanvas ? RenderMode.WorldSpace : RenderMode.ScreenSpaceCamera); }).FirstOrDefault();
        Debug.Assert(canvas != null, "No canavas found with correct render mode set");

        defaultSettings = textBoxSettings;
    }
    void OnEnable()
    {
        canvas = FindObjectsOfType<Canvas>().Where((Canvas c) => { return c.renderMode == (useWorldSpaceCanvas ? RenderMode.WorldSpace : RenderMode.ScreenSpaceCamera); }).FirstOrDefault();
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
        if (dialogueRunner.isDialogueRunning)
            return;

        CallTextBoxStringInterrupt(text);
    }

    public void CallTextBoxStringInterrupt(string text)
    {
        string prefix = 
            @"title:Inline
---
";
        string node = prefix + text + System.Environment.NewLine + "===";

        dialogueRunner.Stop();
        dialogueRunner.Clear();

        dialogueRunner.AddScript(node);
        // Load all scripts
        if (dialogueRunner.sourceText != null)
        {
            foreach (var source in dialogueRunner.sourceText)
            {
                // load and compile the text
                dialogueRunner.AddScript(source.text);
            }
        }

        Clean();
        dialogueRunner.StartDialogue("Inline");
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
    public void SetTextBoxSettings(string name="default")
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
        dialogueRunner.Stop();
        _textBox?.DisableTextbox();
    }
}
