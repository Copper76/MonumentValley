using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    //Global variables
    static PlayerController ida;

    public static void Initialise()
    {
        ida = GameObject.Find("Ida").GetComponent<PlayerController>();
    }

    public static Vector3 GetIdaOrientation()
    {
        return Quaternion.Euler(ida.rotateCompensation) * ida.transform.up;
    }

    public static void SetIdaMovable(bool canMove)
    {
        ida.SetCanMove(canMove);
    }

    public static void SetIdaTarget(Walkable target)
    {
        ida.SetTarget(target);
    }

    public static void SetIdaParent(Walkable newCube)
    {
        ida.transform.parent = newCube.transform.parent;
        ida.transform.localRotation = newCube.transform.localRotation;
        ida.rotateCompensation = newCube.GetRotateCompensation();
    }
}
