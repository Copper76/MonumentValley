using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Walkable node1;
    public Walkable node2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            BreakLine();
        }
    }

    void BreakLine()
    {
        node1.RemoveConnection(node2);
        node2.RemoveConnection(node1);
    }
}
