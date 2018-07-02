using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;
using System.Linq;

public class DialougeUI : DialogueUIBehaviour
{
    public GameObject choicePrefab;
    public TextBox globalTextBox;
    public TextBoxSettings globalTextBoxSettings;
    private OptionChooser SetSelectedOption;
    private DialogueRunner runner;
    private TextGenerator generator;
    private List<TextBoxRef> activeTextBoxes;
    private AudioSource audioSource;

    public override IEnumerator RunCommand(Command command)
    {
        // "Perform" the command
        Debug.Log("Command: " + command.text);
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
            throw new System.Exception("TextBox Has No Target.");
        }

        lineText = line.text.Split(':');

        tb = FindObjectsOfType<TextBoxRef>()
            .Where((TextBoxRef s) => { return s.name == lineText[0]; })
            .First();

        text = lineText[1];
        textBox = tb.textBox;
        textBox.textComp.text = string.Empty;
        int i = 0;

        audioSource = textBox.GetComponent<AudioSource>();

        textBox.textComp.font = tb.textBoxSettings.font;
        textBox.textComp.fontSize = tb.textBoxSettings.fontSize;
        textBox.delay = tb.textBoxSettings.delay;
        textBox.passiveLineDelay = tb.textBoxSettings.lineDelay;

        if (textBox.autoSize)
        {
            TextGenerationSettings settings = textBox.textComp.GetGenerationSettings(textBox.autoSize ? textBox.maxSize : textBox.GetComponent<RectTransform>().sizeDelta);
            generator.Populate(text, settings);
            Vector2 textBoxSize = GetPageSize(settings);
            textBox.GetComponent<RectTransform>().sizeDelta = textBoxSize + textBox.padding;
        }

        if(textBox.gameObject.activeSelf == false)
        {
            textBox.gameObject.SetActive(true);
        }

        while (i < text.Length)
        {
            textBox.textComp.text += text[i];
            tb.textBoxSettings.sfx.Play(audioSource);
            yield return new WaitForSeconds(textBox.delay);
            i++;
        }

        if(textBox.inputType == TextBox.InputType.Player)
            yield return new WaitWhile(() => { return Input.GetButtonDown("Fire1") == false; });

        else if (textBox.inputType == TextBox.InputType.Passive)
        {
            float nextTime = Time.time + textBox.passiveLineDelay;
            yield return new WaitWhile(() => { return Time.time < nextTime; });
            textBox.gameObject.SetActive(false);
        }
    }

    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        Debug.Log(optionsCollection.options);
        optionChooser(0); // choose the first option.

        yield return null;
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
        }
        yield return null;
    }


    // instead of '/100f' should get the canvas scaler from the current canvas it is trying to reference.
    // auto size might only use the world space canvas?
    public Vector2 GetPageSize(TextGenerationSettings settings)
    {
        Vector2 ret = Vector2.zero;
        var lArray = generator.GetLinesArray();
        var cArray = generator.GetCharactersArray();

        float lineWidth;

        for (int lIndex = 0; lIndex < lArray.Length; lIndex++)
        {
            lineWidth = 0;
            ret.y += (settings.fontSize + settings.lineSpacing) /100f;
            int i = lIndex + 1 < lArray.Length ? lArray[lIndex + 1].startCharIdx : cArray.Length - 1;
            for (int cIndex = lArray[lIndex].startCharIdx; cIndex < i; cIndex++)
            {
                lineWidth += cArray[cIndex].charWidth /100f;
            }

            if (ret.x < lineWidth)
                ret.x = lineWidth;
        }
        return ret;
    }
}