using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Walkable : MonoBehaviour
{
    public Vector3 pathDir;
    public Vector3 testDir;

    [Header("Offsets")]
    public bool isStairs;
    public float walkPointOffset = 1.0f;

    [Header("Paths")]
    public List<Walkable> connectedCubes;

    public Walkable previousCube;

    // Start is called before the first frame update
    void Awake()
    {
        FillConnectedCubes();
        walkPointOffset = isStairs ? 0.6f : 1.0f;
    }

    void Update()
    {
        testDir = transform.position + transform.rotation * pathDir * walkPointOffset;
    }

    //To reduce the task of linked all cubes with each other, set is implemented so there is no duplications
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
        /**
        foreach (Vector3Int vec in testLineTrace)
        {
            Gizmos.DrawLine(transform.position, transform.position + vec);
        }
        **/
    }
}
