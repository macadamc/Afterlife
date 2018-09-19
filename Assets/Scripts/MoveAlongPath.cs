using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class MoveAlongPath : State
{
    // put the points from unity interface
    public Transform[] wayPointList;

    public int currentWayPoint = 0;
    Transform m_targetWayPoint;

    public MovementController mc;
    public InputController ic;

    public bool loop;

    // Update is called once per frame
    void Update()
    {
        // check if we have somewere to walk
        if (currentWayPoint < this.wayPointList.Length)
        {
            if (m_targetWayPoint == null)
                m_targetWayPoint = wayPointList[currentWayPoint];
            walk();
        }
        else
        {
            if (loop)
            {
                currentWayPoint = 0;
                m_targetWayPoint = null;
            }
        }
    }

    void walk()
    {
        if (Vector2.Distance(ic.transform.position, m_targetWayPoint.position) > .15f)
        {
            ic.joystick =  m_targetWayPoint.position - ic.transform.position;
            ic.joystick.Normalize();
        }
        else
        {
            currentWayPoint++;
            m_targetWayPoint = null;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ic.joystick = Vector2.zero;
    }
}