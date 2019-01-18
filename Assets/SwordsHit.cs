using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsHit : MonoBehaviour {

    HeldItem gameObjectItem;
    ItemController ic;
    public MovementController mc;
    public float force = 5f;

    public GameObject hitParticlePrefab;

    private void OnEnable()
    {
        if(gameObjectItem == null)
            gameObjectItem = GetComponent<HeldItem>();

        if(ic == null)
            gameObjectItem.onStart.AddListener(Init);
    }
    private void OnDisable()
    {
        gameObjectItem.onStart.RemoveListener(Init);
    }

    public void Init(ItemController ic)
    {
        this.ic = ic;
        mc = ic.GetComponent<MovementController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var sword = other.GetComponent<SwordsHit>(); ;
        if (sword != null && this != sword)
        {
            mc.AddKnockback((mc.transform.position - sword.mc.transform.position).normalized * force);
            GameObject go = Instantiate(hitParticlePrefab);
            go.transform.position = (mc.transform.position + sword.mc.transform.position) / 2f;

            /*
             * parallel lines dont intersect?;
            Vector2 intersectionPoint;
            var s1 = (Vector2)mc.transform.position;
            var s2 = (Vector2)sword.mc.transform.position;
            var e1 = s1 + (mc.Ic.lookDirection * 2f);// force is not the righgt variable here.. is should be the tip off the swordGameobject.
            var e2 = s2 + (sword.mc.Ic.lookDirection * 2f);// force is not the righgt variable here.. is should be the tip off the swordGameobject.
            Debug.Log(LineSegmentsIntersection(s1, e1, s2, e2, out intersectionPoint));

            go.transform.position = new Vector3(intersectionPoint.x, intersectionPoint.y, go.transform.position.z);
            Debug.Log(go.transform.position);
            */
        }
    }

    public bool LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector3 p4, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

        if (d == 0.0f)
        {
            return false;
        }

        var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        intersection.x = p1.x + u * (p2.x - p1.x);
        intersection.y = p1.y + u * (p2.y - p1.y);

        return true;
    }
}
