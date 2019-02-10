using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TriggerDialogueMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextBoxRef trackBinding = playerData as TextBoxRef;

        if (!trackBinding)
            return;
        /*
        int inputCount = playable.GetInputCount ();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<TriggerDialogueBehaviour> inputPlayable = (ScriptPlayable<TriggerDialogueBehaviour>)playable.GetInput(i);
            TriggerDialogueBehaviour input = inputPlayable.GetBehaviour ();
        }
        */
    }
}
