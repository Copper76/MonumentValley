using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableContainer : MonoBehaviour
{
    [SerializeField] private Movable mover;
    [SerializeField] private List<Walkable> walkables;

    private void Start()
    {
        foreach (Walkable walkable in transform.GetComponents<Walkable>())
        {
            walkables.Add(walkable);
        }
    }

    public void SetMover(Movable mover)
    {
        this.mover = mover;
    }

    public Movable GetMover()
    {
        return mover; 
    }

    public Walkable GetValidWalkable()
    {
        foreach (Walkable walkable in walkables)
        {
            //Debug.Log(Quaternion.Euler(walkable.rotateCompensation) * walkable.transform.rotation * walkable.pathDir);
            if (Quaternion.Euler(walkable.GetRotateCompensation()) * walkable.transform.rotation * walkable.GetPathDir() == GameManager.GetIdaOrientation())
            {
                return walkable;
            }
        }

        return null;
    }
}
