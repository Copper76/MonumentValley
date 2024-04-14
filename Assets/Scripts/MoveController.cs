using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public Movable[] movers;

    public bool OnStartMove(Vector2 mousePosition)
    {
        foreach (Movable movable in movers)
        {
            if (movable.CanNotMove())
            {
                return false;
            }
        }

        foreach (Movable movable in movers)
        {
            movable.OnStartMove(mousePosition);
        }
        return true;
    }

    public void OnMove(Vector2 mousePosition)
    {
        foreach (Movable movable in movers)
        {
            movable.OnMove(mousePosition);
        }
    }

    public void OnCompleteMove()
    {
        foreach (Movable movable in movers)
        {
            movable.OnCompleteMove();
        }
    }
}
