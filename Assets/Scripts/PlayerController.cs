using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public bool isWalking = false;

    public Walkable targetCube;
    public Vector3 targetPos;
    public Movable mover;

    //Pathfinding
    public List<Walkable> finalPath = new List<Walkable>();
    public Walkable currentCube;

    //private
    private float counter = 0.0f;
    private float holdTimer = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        GetCurrentCube();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentCube();

        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.GetComponent<Walkable>())
                {
                    targetCube = mouseHit.transform.GetComponent<Walkable>();
                }
                mover = mouseHit.transform.parent.GetComponent<Movable>();
                if (mover != null)
                {
                    mover.OnStartMove(Input.mousePosition);
                }
            }
        }

        if (Input.GetMouseButton(0) && mover != null)
        {
            mover.OnMove(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            bool wantsToMove = false;
            if (mover != null)
            {
                wantsToMove = mover.OnCompleteMove();
                mover = null;
            }

            if (targetCube != null && !wantsToMove)
            {
                CalculatePath();
            }
        }

        if (targetPos != transform.position && isWalking)
        {
            //transform.position = Vector3.Lerp(transform.position, targetPos, 0.01f * Time.deltaTime);
        }
        else
        {
            isWalking = false;
        }
        if (targetPos == transform.position && finalPath.Any())
        {
            Walkable next = finalPath.Last();
            targetPos = next.GetWalkPoint() + (Vector3)GameManager.GetIdaOrientation() * 0.5f;
            finalPath.Remove(next);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
        }

    }

    //Called on tile change and interruption
    private void GetCurrentCube()
    {
        Ray playerDownRay = new Ray(transform.position, -GameManager.GetIdaOrientation());
        RaycastHit hit;

        if (Physics.Raycast(playerDownRay, out hit))
        {
            if (hit.transform.GetComponent<Walkable>() != null)
            {
                if (currentCube == hit.transform.GetComponent<Walkable>())
                {
                    return;
                }

                if (currentCube != null && currentCube.transform.parent.GetComponent<Movable>() != null)
                {
                    currentCube.transform.parent.GetComponent<Movable>().idaOnTile = false;
                }

                currentCube = hit.transform.GetComponent<Walkable>();

                if (hit.transform.parent.GetComponent<Movable>() != null)
                {
                    hit.transform.parent.GetComponent<Movable>().idaOnTile = true;
                }

                transform.parent = hit.transform;

                //more stuff for animation
            }
        }
    }

    void CalculatePath()
    {
        finalPath.Clear();
        Stack<Walkable> nextCubes = new Stack<Walkable>();
        List<Walkable> pastCubes = new List<Walkable>();

        foreach (Walkable path in currentCube.possiblePaths)
        {
            if (path.CanWalk())
            {
                nextCubes.Push(path);
                path.previousCube = currentCube;
            }
        }

        nextCubes.Push(currentCube);

        if(ExploreCube(nextCubes, pastCubes))
        {
            BuildPath();
        }
    }

    //Check if a valid path is found
    bool ExploreCube(Stack<Walkable> nextCubes, List<Walkable> pastCubes)
    {
        if (nextCubes.Count == 0)
        {
            return false;
        }

        Walkable current = nextCubes.Pop();
        if (current == targetCube)
        {
            return true;
        }

        foreach(Walkable path in current.possiblePaths)
        {
            if (path.CanWalk() && !pastCubes.Contains(path))
            {
                nextCubes.Push(path);
                path.previousCube = current;
            }
        }

        pastCubes.Add(current);

        return ExploreCube(nextCubes, pastCubes);
    }

    void BuildPath()
    {
        Walkable cube = targetCube;

        while (cube != currentCube)
        {
            finalPath.Add(cube);
            cube = cube.previousCube;
        }

        //TP for now
        transform.position = targetCube.GetWalkPoint() + (Vector3)GameManager.GetIdaOrientation() * 0.5f;
    }
}
