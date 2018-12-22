using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeColorBasedOnDistance : MonoBehaviour {

    public Transform T1, T2;
    public SpriteRenderer[] SpriteRends;
    public float MaxDistance;
    public AnimationCurve Curve;
    public Gradient Gradient;

    public UnityEvent onMaxValue;
    public bool disableOnMaxValue = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        foreach(SpriteRenderer r in SpriteRends)
        {
            r.color = Gradient.Evaluate(Curve.Evaluate(Percentage));
        }

        if (Percentage >= 0.99f)
        {
            //Debug.Log("You walked into the light");
            onMaxValue.Invoke();

            if(disableOnMaxValue)
                enabled = false;
        }
		
	}

    public float Distance
    {
        get
        {
            return Vector2.Distance( T1.position, T2.position );
        }
    }

    public float Percentage
    {
        get
        {
            return Mathf.Clamp( Distance / MaxDistance , 0.0f, 1.0f );
        }
    }

}
