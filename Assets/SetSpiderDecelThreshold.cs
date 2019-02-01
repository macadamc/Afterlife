using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;


public class SetSpiderDecelThreshold : State
{

    public SpiderMC mc;
    public float threshold;
    protected float m_Threshold;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (mc == null)
            mc = GetComponentInParent<SpiderMC>();
        m_Threshold = mc.decelDeadZone;
        mc.decelDeadZone = threshold;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        mc.decelDeadZone = m_Threshold;
    }
}
