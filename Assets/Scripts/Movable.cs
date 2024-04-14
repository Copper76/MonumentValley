using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movable : MonoBehaviour
{
    [SerializeField] protected bool idaOnTile = false;
    [SerializeField] protected bool canMoveWithIda;

    protected Vector2 mousePos;
    protected bool inUse;

    [SerializeField] protected int[] anchorWalkableStarts;
    [SerializeField] protected int[] anchorWalkableEnds;
    [SerializeField] protected Walkable[] anchorWalkables;
    protected int anchorPoint = 0;

    void Start()
    {
        foreach (WalkableContainer walkableContainer in transform.GetComponentsInChildren<WalkableContainer>())
        {
            walkableContainer.SetMover(this);
        }
        CalculateArrayEnds();
    }

    void Update()
    {

    }

    public bool isInUse()
    {
        return inUse;
    }

    public void SetIdaOnTile(bool onTile)
    {
        idaOnTile = onTile;
    }

    public void SetAnchorWalkables(Walkable[] newWalkables)
    {
        anchorWalkables = newWalkables;
    }

    public void SetAnchorStarts(int[] newStarts)
    {
        anchorWalkableStarts = newStarts;
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
        inUse = true;
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
        inUse = false;

        MakeConnections();
    }

    public bool CanNotMove()
    {
        return idaOnTile && !canMoveWithIda;
    }
}
