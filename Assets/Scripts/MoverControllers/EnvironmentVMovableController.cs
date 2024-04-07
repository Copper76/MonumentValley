using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentVMovableController : MovableController
{
    public GameObject movedObject;

    public bool blockIda;

    public Vector3 destination;
    public bool willRotate;
    public Vector3 destinationRotation;

    public Walkable[] addedWalkables;
    public Walkable[] removedWalkables;

    private Vector3 initialPosition;
    private Vector3 targetPos;

    private Vector3 initialRotation;
    private Vector3 targetRotation;
    private bool isUsing;

    public Movable mover;
    public Walkable[] newWalkables;
    public int[] newWalkableStarts;

    void Awake()
    {
        targetPos = movedObject.transform.position;

        targetRotation = movedObject.transform.eulerAngles;
    }
    public override void Activate()
    {
        for (int i = 0; i < removedWalkables.Length; i += 2)
        {
            removedWalkables[i].RemoveConnection(removedWalkables[i + 1]);
            removedWalkables[i + 1].RemoveConnection(removedWalkables[i]);
        }

        initialPosition = movedObject.transform.position;
        initialRotation = movedObject.transform.eulerAngles;

        if (blockIda)
        {
            GameManager.SetIdaMovable(false);
        }

        targetPos = destination;
        if (willRotate)
        {
            targetRotation = destinationRotation;
        }
        else
        {
            targetRotation = initialRotation;
        }

        if (!movedObject.activeInHierarchy)
        {
            movedObject.SetActive(true);
        }
        if (mover != null)
        {
            mover.BreakConnections();
            mover.anchorWalkableStarts = newWalkableStarts;
            mover.anchorWalkables = newWalkables;
            mover.CalculateArrayEnds();
        }

        isUsing = true;
    }

    public override void Deactivate()
    {
        targetPos = initialPosition;
        targetRotation = initialRotation;
        isUsing = true;
    }

    void Update()
    {
        if ((targetPos != movedObject.transform.position || targetRotation != movedObject.transform.eulerAngles) && isUsing)
        {
            movedObject.transform.position = Vector3.Lerp(movedObject.transform.position, targetPos, 0.1f);

            movedObject.transform.rotation = Quaternion.Lerp(movedObject.transform.rotation, Quaternion.Euler(targetRotation), 0.1f);

            if (Vector3.Distance(targetPos, movedObject.transform.position) < 0.01f)
            {
                movedObject.transform.position = targetPos;
                movedObject.transform.eulerAngles = targetRotation;

                for (int i = 0; i < addedWalkables.Length; i += 2)
                {
                    addedWalkables[i].AddConnection(addedWalkables[i + 1]);
                    addedWalkables[i + 1].AddConnection(addedWalkables[i]);
                }

                if (mover)
                {
                    mover.MakeConnections();
                }

                if (blockIda)
                {
                    GameManager.SetIdaMovable(true);
                }

                isUsing = false;
            }
        }
    }
}
