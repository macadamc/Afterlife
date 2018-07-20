using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;

public class DialougeUI : DialogueUIBehaviour
{
    [System.NonSerialized]
    public VariableStorage variables;
    public GameObject choicePrefab;
    private OptionChooser SetSelectedOption;
    private TextGenerator generator;
    private List<TextBoxRef> activeTextBoxes;
    private AudioSource audioSource;
    private TextBoxRef lastTextBox;

    [DrawWithUnity]
    public List<CommandHook> commands;

    private void Start()
    {
        variables = SaveLoadManager.Instance.savedVariables;
    }

    public override IEnumerator RunCommand(Command command)
    {
        string[] strings = command.text.Split(' ');
        commands.Find((CommandHook x) => { return x.name == strings[0]; })?.unityEvent.Invoke(strings);
        yield break;
    }

    public override IEnumerator RunLine(Line line)
    {
        line.text = ParseVariables(line.text);
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
            choice.GetComponentInChildren<Text>().text = ParseVariables(option);
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
        yield return null;
    }

    public override IEnumerator DialogueComplete()
    {
        foreach(var tb in activeTextBoxes)
        {
            tb.textBoxSettings = tb.defaultSettings;
            if(tb.caller != null)
                tb.caller.triggered = false;
        }
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

    public string ParseVariables(string text)
    {
        string ret = text;
        Regex varcheck = new Regex("\\[.*\\]");
        foreach(var match in varcheck.Matches(text))
        {
            string str = match.ToString();
            string key = $"${str.Substring(1, str.Length - 2)}";

            if(variables.HasKey(key))
            {
                string value = variables.GetValue(key).AsString;
                //Debug.Log($"{key}: {value}");
                ret = ret.Replace(str, value);
            }
            
        }
        return ret;
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

