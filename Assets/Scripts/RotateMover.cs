using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMover : Movable
{
    public Vector3 rotateAxis;//Z-Axis needs to be inverse
    public Vector3[] moveAnchors;
    public Vector3 pivot;

    public override void OnStartMove(Vector2 mousePosition)
    {
        base.OnStartMove(mousePosition);

        pivot = Camera.main.WorldToScreenPoint(transform.position);
    }
    public override void OnMove(Vector2 mousePosition)
    {
        if(CanNotMove())
        {
            return;
        }
        float changeAmount = 0.0f;
        changeAmount += (mousePosition.x - mousePos.x);
        changeAmount += (mousePosition.y - mousePos.y);
        transform.Rotate(rotateAxis, changeAmount);
        mousePos = mousePosition;
    }

    public override void OnCompleteMove()
    {
        float minAngle = Mathf.Infinity;
        int closestAnchor = 0;
        for(int i = 0; i< moveAnchors.Length;i++)
        {
            float angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(moveAnchors[i]));
            if (angle < minAngle)
            {
                minAngle = angle;
                closestAnchor = i;
            }
        }
        closestAnchor %= moveAnchors.Length-1;//360 is the same as 0
        //GameManager.SetIdaOrientation(orientations[closestAnchor]);
        transform.eulerAngles = moveAnchors[closestAnchor];
        anchorPoint = closestAnchor;
        base.OnCompleteMove();
    }

}
