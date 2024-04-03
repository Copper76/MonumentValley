using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    //Global variables
    static List<Movable> movables = new List<Movable>();
    static List<Walkable> walkables = new List<Walkable>();

    public static Vector3Int[] WorldDirs = {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1),
    };

    static int idaOrientation = 2;

    public static void Initialise()
    {
        foreach (Movable movable in movables)
        {
            movable.Init();
        }

        foreach (Walkable walkable in walkables)
        {
            walkable.CalculateWalkableSurfaces();
        }

        foreach (Walkable walkable in walkables)
        {
            walkable.CalculatePossiblePath();
        }
    }

    public static List<Movable> GetAllMovables()
    {
        return movables;
    }

    public static void AddMovable(Movable movable)
    {
        movables.Add(movable);
    }

    public static List<Walkable> GetAllWalkables()
    {
        return walkables;
    }

    public static void AddWalkable(Walkable walkable)
    {
        walkables.Add(walkable);
    }

    public static void SetIdaOrientation(int orientation)
    {
        idaOrientation = orientation;
    }

    public static Vector3Int GetIdaOrientation()
    {
        return WorldDirs[idaOrientation];
    }
}
