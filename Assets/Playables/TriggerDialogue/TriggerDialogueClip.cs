using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TriggerDialogueClip : PlayableAsset, ITimelineClipAsset
{
    public TriggerDialogueBehaviour template = new TriggerDialogueBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TriggerDialogueBehaviour>.Create (graph, template);
        TriggerDialogueBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
