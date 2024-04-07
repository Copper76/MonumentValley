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
        return ida.transform.rotation * ida.transform.up;
    }

    public static void SetIdaMovable(bool canMove)
    {
        ida.canMove = canMove;
    }
}
