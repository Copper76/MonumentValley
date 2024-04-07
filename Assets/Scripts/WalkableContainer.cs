using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableContainer : MonoBehaviour
{
    public Movable mover;
    public List<Walkable> walkables;

    private void Start()
    {
        foreach (Walkable walkable in transform.GetComponents<Walkable>())
        {
            walkables.Add(walkable);
        }
    }

    public Walkable GetValidWalkable()
    {
        foreach (Walkable walkable in walkables)
        {
            if (walkable.transform.rotation * walkable.pathDir == GameManager.GetIdaOrientation())
            {
                return walkable;
            }
        }

        return null;
    }
}
