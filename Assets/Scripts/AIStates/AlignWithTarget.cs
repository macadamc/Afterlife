using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class AlignWithTarget : State
{
    public Vision vis;
    public InputController ic;
    public AlignType alignment;
    public enum AlignType { Horizontal, Vertical }

    private void Update()
    {
        if(vis.targets.transforms.Count > 0)
        {
            Transform target = vis.targets.transforms[0];

            if(alignment == AlignType.Horizontal)
            {

                vis.transform.position = new Vector3(target.position.x, vis.transform.position.y, vis.transform.position.z);

            }
        }
    }
}
