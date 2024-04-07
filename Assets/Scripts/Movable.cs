using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movable : MonoBehaviour
{
    public bool idaOnTile = false;
    public bool canMoveWithIda;

    public Vector2 mousePos;

    public int[] anchorWalkableStarts;
    public int[] anchorWalkableEnds;
    public Walkable[] anchorWalkables;
    public int anchorPoint = 0;

    void Start()
    {
        foreach (WalkableContainer walkableContainer in transform.GetComponentsInChildren<WalkableContainer>())
        {
            walkableContainer.mover = this;
        }
        CalculateArrayEnds();
    }

    void Update()
    {

    }

    public void CalculateArrayEnds()
    {
        anchorWalkableEnds = new int[anchorWalkableStarts.Length];
        for (int i = 0; i < anchorWalkableEnds.Length - 1; i++)
        {
            anchorWalkableEnds[i] = anchorWalkableStarts[i + 1];
        }
        anchorWalkableEnds[anchorWalkableEnds.Length - 1] = anchorWalkables.Length;
    }

    public void BreakConnections()
    {
        for (int i = anchorWalkableStarts[anchorPoint]; i < anchorWalkableEnds[anchorPoint]; i += 2)
        {
            anchorWalkables[i].RemoveConnection(anchorWalkables[i + 1]);
            anchorWalkables[i + 1].RemoveConnection(anchorWalkables[i]);
        }
    }
    public void MakeConnections()
    {
        for (int i = anchorWalkableStarts[anchorPoint]; i < anchorWalkableEnds[anchorPoint]; i += 2)
        {
            anchorWalkables[i].AddConnection(anchorWalkables[i + 1]);
            anchorWalkables[i + 1].AddConnection(anchorWalkables[i]);
        }
    }

    public virtual void OnStartMove(Vector2 mousePosition)
    {
        if (CanNotMove())
        {
            return;
        }
        mousePos = mousePosition;

        BreakConnections();
    }

    public abstract void OnMove(Vector2 mousePosition);

    public virtual void OnCompleteMove()
    {
        if (CanNotMove())
        {
            return;
        }

        MakeConnections();
    }

    protected bool CanNotMove()
    {
        return idaOnTile && !canMoveWithIda;
    }
}
