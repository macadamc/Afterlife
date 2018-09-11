using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using TMPro;
/*
public class DialougeUIOLD : DialogueUIBehaviour
{
    public GameObject choicePrefab;
    private OptionChooser SetSelectedOption;
    private TextGenerator generator;
    private List<TextBoxRef> activeTextBoxes;
    private AudioSource audioSource;
    private TextBoxRef lastTextBox;

    public UnityEvent OnStart, onEnd;

    [DrawWithUnity]
    public List<CommandHook> commands;

    public override IEnumerator RunCommand(Command command)
    {
        string[] strings = command.text.Split(' ');
        commands.Find((CommandHook x) => { return x.name == strings[0]; })?.unityEvent.Invoke(strings);
        yield break;
    }

    public override IEnumerator RunLine(Line line)
    {
        TextBoxRef tb = null;
        TextBox textBox = null;
        string text = string.Empty;
        string[] lineText = null;

        if (line.text.Contains(":") == false)
        {
            yield break;
            throw new System.Exception("TextBox Has No Target.");
        }

        lineText = line.text.Split(':');

        tb = FindObjectsOfType<TextBoxRef>()
            .Where((TextBoxRef s) => { return s.textBoxKey == lineText[0];})
            .First();

        text = lineText[1];
        textBox = tb.textBox;
        textBox.textComp.text = string.Empty;
        lastTextBox = tb;
        if(activeTextBoxes.Contains(tb) == false)
        {
            activeTextBoxes.Add(tb);
        }
        int i = 0;

        audioSource = textBox.GetComponent<AudioSource>();

        textBox.textComp.font = tb.textBoxSettings.font;
        textBox.textComp.fontSize = tb.textBoxSettings.fontSize;

        if (tb.textBoxSettings.autoSize)
        {
            TextGenerationSettings settings = textBox.textComp.GetGenerationSettings(tb.textBoxSettings.autoSize ? tb.textBoxSettings.maxSize : textBox.GetComponent<RectTransform>().sizeDelta);
            generator.Populate(text, settings);
            Vector2 textBoxSize = GetPageSize(settings);
            textBox.GetComponent<RectTransform>().sizeDelta = textBoxSize + tb.textBoxSettings.padding;
        }

        if(textBox.gameObject.activeSelf == false)
        {
            textBox.EnabledTextBox(tb.textBoxSettings.useTween);
        }

        if(tb.textBoxSettings.useDelay)
        {
            while (i < text.Length)
            {
                textBox.textComp.text += text[i];
                tb.textBoxSettings.sfx.Play(audioSource);

                if(textBox.useTimeScale)
                    yield return new WaitForSeconds(tb.textBoxSettings.delay * (Input.GetButton("Interact") ? .5f : 1f));
                else
                    yield return new WaitForSecondsRealtime(tb.textBoxSettings.delay * (Input.GetButton("Interact") ? .5f : 1f));

                i++;
            }
        }
        else
        {
            textBox.textComp.text = text;
            tb.textBoxSettings.sfx.Play(audioSource);
        }

        if(tb.textBoxSettings.inputType == TextBoxSettings.InputType.Player)
            yield return new WaitWhile(() => { return SimpleInput.GetButtonDown("Interact") == false; });
        else if (tb.textBoxSettings.inputType == TextBoxSettings.InputType.Passive)
        {
            float nextTime = Time.time + tb.textBoxSettings.passiveLineDelay;
            yield return new WaitWhile(() => { return Time.time < nextTime; });
        }

        textBox.DisableTextbox(tb.textBoxSettings.useTween);
        yield return new WaitWhile(() => { return textBox.gameObject.activeSelf; });
    }

    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        bool waitingForChoice = true;
        lastTextBox.textBox.EnabledTextBox();
        lastTextBox.textBox.choiceContainer.SetActive(true);

        for (int i = 0; i < lastTextBox.textBox.choiceContainer.transform.childCount; i++)
        {
            Destroy(lastTextBox.textBox.choiceContainer.transform.GetChild(i).gameObject);
        }

        foreach(string option in optionsCollection.options)
        {
            Button choice = Instantiate(choicePrefab, lastTextBox.textBox.choiceContainer.transform, false).GetComponent<Button>();
            choice.GetComponentInChildren<Text>().text = option;
            choice.onClick.AddListener(() => {
                waitingForChoice = false;
                optionChooser(choice.transform.GetSiblingIndex());
            });
        }

        yield return new WaitUntil(() => { return waitingForChoice == false;});
        lastTextBox.textBox.choiceContainer.SetActive(false);
        lastTextBox.textBox.DisableTextbox();
        yield return new WaitWhile(() => { return lastTextBox.textBox.enabled; });
    }

    public override IEnumerator DialogueStarted()
    {
        activeTextBoxes = new List<TextBoxRef>();            

        if (generator == null)
            generator = new TextGenerator();
        OnStart?.Invoke();
        yield return null;
    }

    public override IEnumerator DialogueComplete()
    {
        foreach(var tb in activeTextBoxes)
        {
            tb.textBoxSettings = tb.defaultSettings;
            if(tb.caller != null)
                tb.caller.executing = false;
        }
        onEnd?.Invoke();
        yield return null;
    }

    public Vector2 GetPageSize(TextGenerationSettings settings)
    {
        Vector2 ret = Vector2.zero;
        var lArray = generator.GetLinesArray();
        var cArray = generator.GetCharactersArray();

        float lineWidth;

        for (int lIndex = 0; lIndex < lArray.Length; lIndex++)
        {
            lineWidth = 0;
            ret.y += settings.fontSize + settings.lineSpacing / 100f;
            int i = lIndex + 1 < lArray.Length ? lArray[lIndex + 1].startCharIdx : cArray.Length - 1;
            for (int cIndex = lArray[lIndex].startCharIdx; cIndex < i; cIndex++)
            {
                lineWidth += cArray[cIndex].charWidth / 100f;
            }

            if (ret.x < lineWidth)
                ret.x = lineWidth;
        }
        return ret;
    }
}
*/
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
            textBox.text.ForceMeshUpdate();
            Vector2 pSize = textBox.text.GetPreferredValues(textBoxRef.textBoxSettings.maxSize.x, textBoxRef.textBoxSettings.maxSize.y);
            if(pSize.x > textBoxRef.textBoxSettings.maxSize.x)
            {
                pSize.y = 0;
                //pSize.y = (pSize.x % textBoxRef.textBoxSettings.maxSize.x);
                foreach (var lineInfo in textBox.text.textInfo.lineInfo)
                {
                    pSize.y += lineInfo.lineHeight;
                }

                pSize.y += (textBox.text.textInfo.lineCount - 1 * textBox.text.lineSpacing) + textBox.text.margin.y + textBox.text.margin.w;

                pSize.x = textBoxRef.textBoxSettings.maxSize.x;
            }            

            textBox.text.rectTransform.sizeDelta = pSize;
            textBox.bg.sizeDelta = pSize;
            print(pSize);
            
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

