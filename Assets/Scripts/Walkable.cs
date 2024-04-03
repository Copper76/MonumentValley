using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Walkable : MonoBehaviour
{
    public bool isStairs;
    public bool isMovable = false;
    public bool canWalk;

    public Vector3Int[] stairsDir  = new Vector3Int[2];

    public Vector3Int[] unWalkableSurfaces;
    public Vector3Int[] extraWalkableSurfaces;
    public List<Vector3Int> walkableSurfaces;


    [Header("Offsets")]
    public float flatOffset = 0.5f;
    public float stairOffset = 0.1f;

    [Header("Paths")]
    public List<Walkable> connectedCubes;
    public List<Walkable> possiblePaths = new List<Walkable>();

    public Walkable previousCube;

    //Debug
    //public List<Vector3Int> testLineTrace;


    // Start is called before the first frame update
    void Awake()
    {
        GameManager.AddWalkable(this);
        FillConnectedCubes();
    }

    void Start()
    {
        //CalculatePossiblePath();
    }

    void Update()
    {

    }

    public void CalculateWalkableSurfaces()
    {
        walkableSurfaces.Clear();

        RaycastHit rayHit;
        foreach (Vector3Int vec in GameManager.WorldDirs)
        {
            Vector3Int adjustedVec = vec;

            if (isMovable)
            {
                adjustedVec = Vector3Int.RoundToInt(transform.InverseTransformDirection(vec));
                //Debug.Log(transform.gameObject.name + Vector3Int.RoundToInt(transform.InverseTransformDirection(vec)) + vec);
            }

            if (unWalkableSurfaces.Contains(adjustedVec))
            {
                continue;
            }

            RaycastHit[] hits = new RaycastHit[3];

            int numHits = Physics.RaycastNonAlloc(transform.position, vec, hits, 1.25f);
            if (numHits > 0)
            {
                rayHit = hits[numHits - 1];

                if (rayHit.transform.tag == "Ida" || rayHit.transform == transform)
                {
                    walkableSurfaces.Add(vec);
                }
                /**
                else
                {
                    if (isMovable)
                    {
                        Debug.Log(transform.gameObject.name + adjustedVec + vec);
                        testLineTrace.Add(vec);
                        Debug.Log(rayHit.transform.gameObject.name);
                    }
                }
                **/
            }
            /**
            if (Physics.Raycast(transform.position, vec, out rayHit, 1.25f))
            {
                if (rayHit.distance > 1.25f || rayHit.transform.tag == "Ida" || rayHit.transform == transform)
                {
                    walkableSurfaces.Add(vec);
                }
                else
                {
                    if (isMovable)
                    {
                        Debug.Log(transform.gameObject.name + adjustedVec + vec);
                        testLineTrace.Add(vec);
                        Debug.Log(rayHit.transform.gameObject.name);
                    }
                }
            }
            **/
            else
            {
                walkableSurfaces.Add(vec);
            }
        }
        canWalk = walkableSurfaces.Contains(GameManager.GetIdaOrientation());
    }

    //To reduce the task of linked all cubes with each other, set is implemented so there is no duplications
    private void FillConnectedCubes()
    {
        foreach (Walkable cube in connectedCubes)
        {
            cube.AddConnection(this);
        }
    }

    public void CalculatePossiblePath()
    {
        possiblePaths.Clear();

        if (!CanWalk()) return;

        foreach (Walkable cube in connectedCubes)
        {
            if (cube.CanWalk())
            {
                possiblePaths.Add(cube);
            }
        }
    }

    public bool CanWalk()
    {
        return canWalk;
    }

    public void AddConnection(Walkable cube)
    {
        if (!connectedCubes.Contains(cube))
        {
            connectedCubes.Add(cube);
        }
    }

    public void RemoveConnection(Walkable cube)
    {
        connectedCubes.Remove(cube);
    }

    public Vector3 GetWalkPoint()
    {
        Vector3 idaOrientation = GameManager.GetIdaOrientation();
        float walkPointOffset = isStairs && stairsDir.Contains<Vector3Int>(Vector3Int.RoundToInt(isMovable ? Quaternion.Inverse(transform.parent.rotation) * idaOrientation : idaOrientation)) ? stairOffset : flatOffset;
        return transform.position + idaOrientation * walkPointOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        if(CanWalk())
        {
            Gizmos.DrawSphere(GetWalkPoint(), 0.1f);
        }

        if (possiblePaths == null) return;

        foreach (Walkable path in possiblePaths) 
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
