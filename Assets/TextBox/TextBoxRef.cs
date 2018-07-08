using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Pixelplacement;
using System.Linq;

public class TextBoxRef : MonoBehaviour {

    [System.NonSerialized]
    public Canvas canvas;

    internal TextBox _textBox;
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

    public GameObject TextBoxPrefab;
    public Vector2 textBoxOffset;
    [System.NonSerialized]
    public DialogueRunner dialogueRunner;
    public TextBoxSettings textBoxSettings;
    internal TextBoxSettings defaultSettings;
    [System.NonSerialized]
    public Interactable caller;
    public bool useWorldSpaceCanvas = true;

    private void Start()
    {
        canvas = FindObjectsOfType<Canvas>().Where((Canvas c) => { return c.renderMode == (useWorldSpaceCanvas ? RenderMode.WorldSpace : RenderMode.ScreenSpaceCamera); }).First();

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

    private void OnDisable()
    {
        _textBox?.DisableTextbox();
    }
}
