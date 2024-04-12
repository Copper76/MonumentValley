using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public string nextLevelName;
    public Scene nextScene;
    public AsyncOperation asyncLoad;

    public Walkable targetCube;
    public Vector3 targetPos;

    public bool isOnGround;
    public bool canMove = true;

    public Vector3 rotateCompensation = new Vector3();

    //Pathfinding
    public List<Walkable> finalPath = new List<Walkable>();
    public Walkable currentCube;
    public MoveController mover;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        GetCurrentCube();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        GetCurrentCube();

        if(isOnGround && canMove)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit mouseHit;

                int layerMask = ~(1 << 7);

                if (Physics.Raycast(mouseRay, out mouseHit, Mathf.Infinity, layerMask))
                {
                    //Debug.Log(mouseHit.transform.name);
                    if (mouseHit.transform.GetComponent<WalkableContainer>())
                    {
                        WalkableContainer container = mouseHit.transform.GetComponent<WalkableContainer>();
                        if (container.GetValidWalkable())
                        {
                            Walkable potentialTarget = container.GetValidWalkable();
                            CalculatePath(potentialTarget);
                            targetPos = transform.position;
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.tag == "Move Controller")
                {
                    mover = mouseHit.transform.GetComponent<MoveController>();
                }
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
            if (mover != null)
            {
                mover.OnCompleteMove();
                mover = null;
            }
        }

        if (targetPos == transform.position && finalPath.Any())
        {
            targetCube = finalPath.First();
            targetPos = targetCube.GetWalkPoint();
            finalPath.RemoveAt(0);
        }
        else
        {
            if (targetCube != null && canMove)
            {
                targetPos = targetCube.GetWalkPoint();
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);
                if (Vector3.Distance(transform.position, targetPos) < 0.05f)
                {
                    transform.position = targetPos;
                }
            }
        }

        if (finalPath.Any())
        {
            ValidatePath();
        }


        //Victory Checking
        if (currentCube != null)
        {
            if (currentCube.tag == "Altar" && transform.position == currentCube.GetWalkPoint())
            {
                LoadLevel(nextLevelName);
            }
        }
    }

    //Called on tile change and interruption
    private void GetCurrentCube()
    {
        RaycastHit hit;
        Ray playerDownRay = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(playerDownRay, out hit, 1.0f))
        {
            if (hit.transform.GetComponent<WalkableContainer>() != null)//To tackle the issue with multiple walkables in one cube
            {
                Walkable validWalkable = hit.transform.GetComponent<WalkableContainer>().GetValidWalkable();
                if (validWalkable != null)
                {
                    isOnGround = true;
                    //rotateCompensation = validWalkable.rotateCompensation;

                    if (currentCube == validWalkable)
                    {
                        return;
                    }

                    if (currentCube != null && currentCube.GetComponent<WalkableContainer>().mover != null)
                    {
                        currentCube.GetComponent<WalkableContainer>().mover.idaOnTile = false;
                    }

                    currentCube = validWalkable;

                    //transform.localRotation = currentCube.transform.localRotation;

                    if (currentCube.GetComponent<WalkableContainer>().mover != null)
                    {
                        currentCube.GetComponent<WalkableContainer>().mover.idaOnTile = true;
                    }

                    transform.parent = hit.transform;

                    return;
                }
            }
        }

        isOnGround = false;
    }

    bool CalculatePath(Walkable target)
    {
        Stack<Walkable> nextCubes = new Stack<Walkable>();
        List<Walkable> pastCubes = new List<Walkable>();

        nextCubes.Push(currentCube);

        if(ExploreCube(nextCubes, pastCubes, target))
        {
            BuildPath(target);
            return true;
        }
        return false;
    }

    //Check if a valid path is found
    bool ExploreCube(Stack<Walkable> nextCubes, List<Walkable> pastCubes, Walkable target)
    {
        if (nextCubes.Count == 0)
        {
            return false;
        }

        Walkable current = nextCubes.Pop();
        if (current == target)
        {
            return true;
        }

        foreach(Walkable path in current.connectedCubes)
        {
            if (!pastCubes.Contains(path))
            {
                nextCubes.Push(path);
                path.previousCube = current;
            }
        }

        pastCubes.Add(current);

        return ExploreCube(nextCubes, pastCubes, target);
    }

    void BuildPath(Walkable target)
    {
        finalPath.Clear();
        Walkable cube = target;

        while (cube != currentCube)
        {
            finalPath.Insert(0, cube);
            cube = cube.previousCube;
        }
    }

    //Validate path live so Ida would stop in front of broken links
    void ValidatePath()
    {
        Walkable next = targetCube;
        int endIndex = finalPath.Count;
        for(int i = 0; i<finalPath.Count; i++)
        {
            if (!next.IsConnected(finalPath[i]))
            {
                targetCube = next;
                endIndex = i;
                break;
            }
            else
            {
                next = finalPath[i];
            }
        }

        finalPath.RemoveRange(endIndex, finalPath.Count - endIndex);
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(NextLevel(levelName));
    }

    IEnumerator NextLevel(string levelName)
    {
        nextScene = SceneManager.GetSceneByName(levelName);
        asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}
