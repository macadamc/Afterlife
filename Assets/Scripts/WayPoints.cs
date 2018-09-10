using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{

    // put the points from unity interface
    public Transform[] wayPointList;

    public int currentWayPoint = 0;
    Transform m_targetWayPoint;

    public float speed = 4f;
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
        // rotate towards the target
        //transform.forward = Vector3.RotateTowards(transform.forward, m_targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, m_targetWayPoint.position, speed * Time.deltaTime);

        if (transform.position == m_targetWayPoint.position)
        {
            currentWayPoint++;
            m_targetWayPoint = null;
        }
    }
}
