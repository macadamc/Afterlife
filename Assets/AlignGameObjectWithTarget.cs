using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class AlignGameObjectWithTarget : State
{
    public InputController inputController;
    public  InputController target_InputController;
    public ItemController itemController;
    private Vision _vision;
    private ItemSpawnPrefabWithCharge itemWithSnap;


    public bool exitStateOnAlign;
    public Vector2 targetDistance;
    public float errorThreshold = .5f;
    public Transform debugObj;

    protected override void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();
        
        if (itemController == null)
            itemController = GetComponentInParent<ItemController>();

        itemWithSnap = itemController.currentItem as ItemSpawnPrefabWithCharge;
        Debug.Log(itemWithSnap.angleSnap);

        if (_vision == null)
            _vision = GetComponentInParent<Vision>();

        if (_vision.targets.transforms != null && _vision.targets.transforms.Count > 0)
            target_InputController = _vision.targets.transforms[0].GetComponent<InputController>();

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inputController.joystick = Vector2.zero;
    }

    private void LateUpdate()
    {
        int snap = itemWithSnap != null ? itemWithSnap.angleSnap : 1;
        //int snap = 45;
        //Debug.Log(snap);
        Vector3 lookdir = AngleSnap(target_InputController.transform.position - inputController.transform.position, snap);

        if (target_InputController != null)
        {
            //Debug.Log(target_InputController.gameObject.name + "," + inputController.gameObject.name);
            //var dist = Vector2.Distance(inputController.transform.position, target_InputController.transform.position);
            //var desiredPos = target_InputController.transform.position - (lookdir * Random.Range(targetDistance.x,targetDistance.y));
            //var desiredPos = target_InputController.transform.position;
            var desiredPos = GetPointOnLine(target_InputController.transform.position, -lookdir, inputController.transform.position);
            if (debugObj != null)
                debugObj.position = desiredPos;

            float dst = Vector2.Distance(inputController.transform.position, desiredPos);
            
            if (dst > errorThreshold)
            {
                inputController.joystick = desiredPos - inputController.transform.position;
                inputController.joystick.Normalize();
            }
            
            else if (exitStateOnAlign)
                Next();
                
                

            /*
            if (IsWithin(dist, targetDistance.x, targetDistance.y) == false)
            {
                if(Vector2.Distance(inputController.transform.position, desiredPos) > errorThreshold)
                {
                    inputController.joystick = inputController.transform.position - desiredPos;
                    inputController.joystick.Normalize();
                }
                else if (exitStateOnAlign)
                    Next();
            }
            */
        }

        
    }

    Vector3 AngleSnap(Vector2 direction, int _angleSnap)
    {
        //  gets the angle from the look direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //  rotates object to face the new angle
        Vector3 vec = Quaternion.AngleAxis(angle, Vector3.forward).eulerAngles;
        
        vec.x = Mathf.Round(vec.x / _angleSnap) * _angleSnap;
        vec.y = Mathf.Round(vec.y / _angleSnap) * _angleSnap;
        vec.z = Mathf.Round(vec.z / _angleSnap) * _angleSnap;

        return Quaternion.Euler(vec) * Vector3.right;
    }

    bool IsParallel(Vector2 v1, Vector2 v2)
    {
        //2 vectors are parallel if the angle between the vectors are 0 or 180 degrees
        if (IsWithin(Vector2.Angle(v1, v2), -.2f, .2f) || IsWithin(Vector2.Angle(v1, v2), 179.8f, 180.2f))
        {
            return true;
        }

        return false;
    }

    bool IsWithin(float value, float min, float max)
    {
        return value >= min && value <= max;
    }

    /*
    //linePnt - point the line passes through
    //lineDir - unit vector in direction of line, either direction works
    //pnt - the point to find nearest on line for
    public Vector3 NearestPointOnLine(Vector2 linePnt, Vector2 lineDir, Vector2 pnt)
    {
        lineDir.Normalize();//this needs to be a unit vector
        var v = pnt - linePnt;
        var d = Vector2.Dot(v, lineDir);
        return linePnt + lineDir * d;
    }
    */

    public Vector3 GetPointOnLine(Vector2 targetPoint, Vector2 dir, Vector2 positionToTest)
    {
        var v = positionToTest - targetPoint;
        var d = Random.Range(targetDistance.x ,targetDistance.y);
        return (targetPoint + dir * d);
    }
}

