using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMover : Movable
{
    public Vector3 rotateAxis;//Z-Axis needs to be inverse
    public Vector2 pivot;

    public int[] orientations;

    public override void OnStartMove(Vector2 mousePosition)
    {
        base.OnStartMove(mousePosition);

        pivot = Camera.main.WorldToScreenPoint(transform.position);
    }
    public override void OnMove(Vector2 mousePosition)
    {
        float changeAmount = 0.0f;
        changeAmount += (mousePosition.x - mousePos.x);
        changeAmount += (mousePosition.y - mousePos.y);
        transform.Rotate(changeAmount * 0.5f * rotateAxis);
        mousePos = mousePosition;
    }

    public override bool OnCompleteMove()
    {
        float minDist = Mathf.Infinity;
        int closestAnchor = 0;
        Vector3 eulerAngle = transform.eulerAngles;
        for(int i = 0; i< moveAnchors.Length;i++)
        {
            float dist = Vector3.Distance(eulerAngle, moveAnchors[i]);
            if (dist < minDist)
            {
                minDist = dist;
                closestAnchor = i;
            }
        }
        closestAnchor %= moveAnchors.Length-1;//360 is the same as 0
        GameManager.SetIdaOrientation(orientations[closestAnchor]);
        transform.rotation = Quaternion.Euler(moveAnchors[closestAnchor]);
        anchorPoint = closestAnchor;
        return base.OnCompleteMove();
    }

}
