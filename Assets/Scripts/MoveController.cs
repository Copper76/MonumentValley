using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public Movable[] movers;

    public void OnStartMove(Vector2 mousePosition)
    {
        foreach (Movable movable in movers)
        {
            movable.OnStartMove(mousePosition);
        }
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
