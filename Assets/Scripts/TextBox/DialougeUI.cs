using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;
using System.Linq;
using UnityEngine.Events;


public class DialougeUI : DialogueUIBehaviour
{
    public GameObject choicePrefab;
    public UnityEvent OnStart, onEnd;
    public List<CommandHook> commands;

    private TextBoxRef m_lastTextBox;
    private List<TextBoxRef> m_activeTextBoxes;
    private AudioSource m_audioSource;
    private OptionChooser m_SetSelectedOption;

    public override IEnumerator RunCommand(Command command)
    {
        string[] strings = command.text.Split(' ');
        commands.Find((CommandHook x) => { return x.name == strings[0]; })?.unityEvent.Invoke(strings);
        yield break;
    }

    public override IEnumerator DialogueStarted()
    {
        if (m_activeTextBoxes == null)
            m_activeTextBoxes = new List<TextBoxRef>();
        else
            m_activeTextBoxes.Clear();

        OnStart?.Invoke();
        yield return null;
    }

    public override IEnumerator DialogueComplete()
    {
        foreach (var tb in m_activeTextBoxes)
        {
            tb.textBoxSettings = tb.defaultSettings;
            if (tb.caller != null)
                tb.caller.executing = false;
        }
        onEnd?.Invoke();
        yield return null;
    }

    public override IEnumerator RunLine(Line line)
    {
        TextBoxRef textBoxRef;
        TextBox textBox;
        string[] lineText;
        string msg;
        string textBoxKey;

        //split TextBoxKey from message.
        if (line.text.Contains(":") == false)
            yield break;

        lineText = line.text.Split(':');
        textBoxKey = lineText[0];
        msg = lineText[1];

        // Find TextBox with textBoxKey
        textBoxRef = FindObjectsOfType<TextBoxRef>()
            .Where((TextBoxRef s) => { return s.textBoxKey == textBoxKey; })
            .First();

        textBox = textBoxRef.textBox;
        m_lastTextBox = textBoxRef;

        textBox.text.rectTransform.sizeDelta = textBoxRef.textBoxSettings.maxSize;
        textBox.bg.sizeDelta = textBoxRef.textBoxSettings.maxSize;

        textBox.EnabledTextBox(textBoxRef.textBoxSettings.useTween);

        // if the current textbox isnt in the conversation yet we add it to the activeTextboxes.
        if (m_activeTextBoxes.Contains(textBoxRef) == false)
        {
            m_activeTextBoxes.Add(textBoxRef);
        }        

        textBox.text.text = msg;
        textBox.text.maxVisibleCharacters = 0;
        m_audioSource = textBox.GetComponent<AudioSource>();

        //Set Font From SettingsAsset.
        if(textBoxRef.textBoxSettings.TMP_Font != null)
        {
            textBox.text.font = textBoxRef.textBoxSettings.TMP_Font;    
        }

        textBox.text.fontSize = textBoxRef.textBoxSettings.fontSize;

        //Set the size of the text box
        if (textBoxRef.textBoxSettings.autoSize)
        {
            
            textBox.text.rectTransform.sizeDelta.Set(textBoxRef.textBoxSettings.maxSize.x, textBox.text.rectTransform.sizeDelta.y);
            textBox.text.ForceMeshUpdate();

            Vector2 pSize = textBox.text.GetPreferredValues(textBox.text.text, textBoxRef.textBoxSettings.maxSize.x, textBoxRef.textBoxSettings.maxSize.y);
            if(pSize.x >= textBoxRef.textBoxSettings.maxSize.x)
            {
                pSize.y = 0;

                int index = 0;
                float lineWidth = 0f;
                for (; index < textBox.text.textInfo.lineCount; index++)
                {
                    
                    if(textBox.text.textInfo.lineInfo[index].width > lineWidth)
                    {
                        lineWidth = textBox.text.textInfo.lineInfo[index].width;
                    }

                    pSize.y += textBox.text.textInfo.lineInfo[index].lineHeight;
                    if(textBox.text.textInfo.lineInfo[index].lastCharacterIndex == textBox.text.text.Length - 1)
                    {
                        break;
                    }
                }

                pSize.y += textBox.text.margin.y + textBox.text.margin.w;

                pSize.x = lineWidth + textBox.text.margin.y + textBox.text.margin.w;
            }

            textBox.text.rectTransform.sizeDelta = pSize;
            textBox.bg.sizeDelta = pSize;            
        }

        textBox.text.ForceMeshUpdate();

        // display per character.
        if (textBoxRef.textBoxSettings.useDelay)
        {
            while (textBox.text.maxVisibleCharacters < textBox.text.textInfo.characterCount)
            {
                // Show Next Character.
                textBox.text.maxVisibleCharacters++;
                
                // Play Audio
                textBoxRef.textBoxSettings.sfx.Play(m_audioSource);

                // not 100% sure this is right.. set the delay between characters.
                if (textBox.useTimeScale)
                    yield return new WaitForSeconds(textBoxRef.textBoxSettings.delay * (Input.GetButton("Interact") ? .5f : 1f));
                else
                    yield return new WaitForSecondsRealtime(textBoxRef.textBoxSettings.delay * (Input.GetButton("Interact") ? .5f : 1f));
            }
        }
        // display per page.
        else
        {
            // show all characters.
            textBox.text.maxVisibleCharacters = textBox.text.textInfo.characterCount;

            // Play Audio
            textBoxRef.textBoxSettings.sfx.Play(m_audioSource);
        }

        //Handle per-line Delay.
        if (textBoxRef.textBoxSettings.inputType == TextBoxSettings.InputType.Player)
            yield return new WaitWhile(() => { return SimpleInput.GetButtonDown("Interact") == false; });
        else if (textBoxRef.textBoxSettings.inputType == TextBoxSettings.InputType.Passive)
        {
            float nextTime = Time.time + textBoxRef.textBoxSettings.passiveLineDelay;
            yield return new WaitWhile(() => { return Time.time < nextTime; });
        }

        //tween textbox closed.
        textBox.DisableTextbox(textBoxRef.textBoxSettings.useTween);
        yield return new WaitWhile(() => { return textBox.gameObject.activeSelf; });
    }

    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class CommandEvent : UnityEngine.Events.UnityEvent<string[]> { }

[System.Serializable]
public class CommandHook
{
    public string name;
    public CommandEvent unityEvent;
}

