using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 0.5f, 1f)]
[TrackClipType(typeof(TriggerDialogueClip))]
[TrackBindingType(typeof(TextBoxRef))]
public class TriggerDialogueTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<TriggerDialogueMixerBehaviour>.Create (graph, inputCount);
    }
}
