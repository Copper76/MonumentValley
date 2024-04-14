using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Walkable : MonoBehaviour
{
    [SerializeField] private Vector3 pathDir;
    [SerializeField] private Vector3 rotateCompensation;

    [Header("Offsets")]
    [SerializeField] private bool isStairs;
    [SerializeField] private float walkPointOffset = 1.0f;

    [Header("Paths")]
    [SerializeField] private List<Walkable> connectedCubes;

    private Walkable previousCube;

    // Start is called before the first frame update
    void Awake()
    {
        FillConnectedCubes();
        walkPointOffset = isStairs ? 0.6f : 1.0f;
    }

    void Update()
    {

    }

    private void FillConnectedCubes()
    {
        foreach (Walkable cube in connectedCubes)
        {
            cube.AddConnection(this);
        }
    }

    public void AddConnection(Walkable cube)
    {
        if (!IsConnected(cube))
        {
            connectedCubes.Add(cube);
        }
    }

    public void RemoveConnection(Walkable cube)
    {
        connectedCubes.Remove(cube);
    }

    public bool IsConnected(Walkable cube)
    {
        return connectedCubes.Contains(cube);
    }

    public List<Walkable> GetConnectedCubes() 
    { 
        return connectedCubes; 
    }

    public Vector3 GetPathDir()
    {
        return pathDir;
    }

    public Vector3 GetRotateCompensation()
    {
        return rotateCompensation;
    }

    public Walkable GetPreviousCube()
    {
        return previousCube;
    }

    public void SetPreviousCube(Walkable cube)
    {
        previousCube = cube;
    }

    public Vector3 GetWalkPoint()
    {
        return transform.position + transform.rotation * pathDir * walkPointOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        Gizmos.DrawSphere(GetWalkPoint(), 0.1f);

        if (connectedCubes == null) return;

        foreach (Walkable path in connectedCubes) 
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(GetWalkPoint(), path.GetWalkPoint());
        }
    }
}
