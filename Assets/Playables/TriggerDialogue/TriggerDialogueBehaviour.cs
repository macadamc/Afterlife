using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Sirenix.OdinInspector;

[Serializable]
public class TriggerDialogueBehaviour : PlayableBehaviour
{
    public bool useInlineDialogue;
    [HideIf("useInlineDialogue")]
    public TextAsset dialogueAsset;
    [ShowIf("useInlineDialogue"), TextArea]
    public string inlineDialogue;

    public string startNode;
    public bool pauseTimeline;

    private Playable m_playable;
    private PlayableGraph m_graph;
    private TextBoxRef m_textBoxRef;
    private DialougeUI m_dialougeUI;

    private UnityEngine.Events.UnityAction m_onEnd;

    [NonSerialized]
    public bool began;
    public bool jumpToEndOfClip;

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        began = false;
    }

    public override void OnPlayableCreate(Playable playable)
    {
        m_playable = playable;
        m_graph = playable.GetGraph();
        m_onEnd = OnDialogueEnd;
        m_dialougeUI = GameObject.FindObjectOfType<DialougeUI>();
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if(m_textBoxRef == null)
            m_textBoxRef = playerData as TextBoxRef;

        if (!m_textBoxRef)
        {
            return;
        }

        if (Application.isPlaying && began == false)
        {
            if (m_textBoxRef.dialogueRunner.isDialogueRunning == false)
            {
                began = true;
                Debug.Log("<color=green>TEXTBOX STARTED</color>");

                m_textBoxRef.dialogueRunner.Clear();// UNLOADS ALL LOADED YARN FILES.

                if (useInlineDialogue)
                    m_textBoxRef.dialogueRunner.AddScript(inlineDialogue);
                else if (dialogueAsset != null)
                    m_textBoxRef.dialogueRunner.AddScript(dialogueAsset);

                m_dialougeUI.onEnd.AddListener(m_onEnd);
                m_textBoxRef.CallTextBox(startNode);

                //Pause without breaking the current animation states. Work around for 2018.2
                if (pauseTimeline)
                    m_graph.GetRootPlayable(0).SetSpeed(0);
                if (jumpToEndOfClip)
                    JumpToEndofPlayable();
            }

        }
    }

    public void OnDialogueEnd()
    {
        m_dialougeUI.onEnd.RemoveListener(m_onEnd);
        
        if(jumpToEndOfClip)
            JumpToEndofPlayable();
        //Unpause
        if (pauseTimeline)
            m_graph.GetRootPlayable(0).SetSpeed(1);
    }

    private void JumpToEndofPlayable()
    {
        if(m_graph.IsValid())
            m_graph.GetRootPlayable(0).SetTime(m_graph.GetRootPlayable(0).GetTime() + m_playable.GetDuration());
    }
}
