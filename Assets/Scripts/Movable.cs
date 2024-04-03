using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movable : MonoBehaviour
{
    public bool idaOnTile = false;

    public List<Walkable> connectedWalkables;

    public Vector2 mousePos;

    public Vector3[] moveAnchors;
    public int[] anchorWalkableStarts;
    public int[] anchorWalkableEnds;
    public Walkable[] anchorWalkables;
    public int anchorPoint = 0;

    public float timer;//Only allow player movement if it is a click
    public float minHoldTime = 0.1f;

    private void Awake()
    {
        GameManager.AddMovable(this);
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Init()
    {
        foreach (Walkable walkable in transform.GetComponentsInChildren<Walkable>())
        {
            walkable.isMovable = true;
        }
        anchorWalkableEnds = new int[anchorWalkableStarts.Length];
        for (int i = 0; i < anchorWalkableEnds.Length-1; i++)
        {
            anchorWalkableEnds[i] = anchorWalkableStarts[i + 1];
        }
        anchorWalkableEnds[anchorWalkableEnds.Length - 1] = anchorWalkables.Length;
    }

    public virtual void OnStartMove(Vector2 mousePosition)
    {
        mousePos = mousePosition;

        for (int i = anchorWalkableStarts[anchorPoint]; i < anchorWalkableEnds[anchorPoint]; i += 2)
        {
            anchorWalkables[i].RemoveConnection(anchorWalkables[i + 1]);
            anchorWalkables[i + 1].RemoveConnection(anchorWalkables[i]);
        }

        RecalculateAffected();

        timer = minHoldTime;
    }

    public abstract void OnMove(Vector2 mousePosition);

    public virtual bool OnCompleteMove()
    {
        for (int i = anchorWalkableStarts[anchorPoint]; i < anchorWalkableEnds[anchorPoint]; i += 2)
        {
            anchorWalkables[i].AddConnection(anchorWalkables[i + 1]);
            anchorWalkables[i + 1].AddConnection(anchorWalkables[i]);
        }

        RecalculateAffected();
        return timer <= 0.0f;
    }

    protected void RecalculateAffected()
    {
        foreach (Walkable walkable in transform.GetComponentsInChildren<Walkable>())
        {
            walkable.CalculateWalkableSurfaces();
        }

        foreach (Walkable walkable in connectedWalkables)
        {
            walkable.CalculateWalkableSurfaces();
        }

        foreach (Walkable walkable in transform.GetComponentsInChildren<Walkable>())
        {
            walkable.CalculatePossiblePath();
        }

        foreach (Walkable walkable in connectedWalkables)
        {
            walkable.CalculatePossiblePath();
        }
    }
}
