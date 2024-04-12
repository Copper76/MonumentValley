using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SlideMover : Movable
{
    public Vector3 moveAxis;//Z-Axis needs to be inverse
    public Vector3 minPos;
    public Vector3 maxPos;
    public Vector3[] moveAnchors;
    public override void OnStartMove(Vector2 mousePosition)
    {
        base.OnStartMove(mousePosition);
    }
    public override void OnMove(Vector2 mousePosition)
    {
        if (CanNotMove())
        {
            return;
        }
        Vector3 changeAmount = Vector3.zero;
        changeAmount.x += (mousePosition.x - mousePos.x) * moveAxis.x / Screen.width;
        changeAmount.y += (mousePosition.y - mousePos.y) * moveAxis.y / Screen.height;
        changeAmount.z += (mousePosition.x - mousePos.x) * moveAxis.z / Screen.width;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + changeAmount.x, minPos.x, maxPos.x),
            Mathf.Clamp(transform.position.y + changeAmount.y, minPos.y, maxPos.y),
            Mathf.Clamp(transform.position.z + changeAmount.z, minPos.z, maxPos.z));
        mousePos = mousePosition;
    }

    public override void OnCompleteMove()
    {
        if (CanNotMove())
        {
            return;
        }
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        for (int i=0;i<moveAnchors.Length;i++)
        {
            if (moveAnchors[i] == transform.position)
            {
                anchorPoint = i;
                MakeConnections();
                break;
            }
        }
    }
}
