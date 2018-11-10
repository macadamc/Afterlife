using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ShadyPixel.StateMachine;

public class ChangeStateAfterX : State {

    [MinMaxSlider(0f, 30f, true)]
    public Vector2 range;
    float nextTime;

     protected override void OnEnable()
    {
        base.OnEnable();
        nextTime = Random.Range(range.x, range.y) + Time.time;   
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Time.time >= nextTime)
        {
            Next();
        }
	}
}
