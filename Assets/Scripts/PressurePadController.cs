using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadController : MonoBehaviour
{
    public Walkable attachedWalkable;
    public MovableController[] moverControllers;

    public bool inUse = false;
    public bool isSingleUse;
    public Material usedMaterial;

    private void OnTriggerStay(Collider other)
    {
        //Ida is on pressure pad
        if (other.tag == "Ida")
        {
            if (other.transform.position == attachedWalkable.GetWalkPoint() && !inUse)
            {
                Use();
            }

            if (other.transform.position != attachedWalkable.GetWalkPoint() && inUse)
            {
                Release();
            }
        }
    }

    private void Use()
    {
        foreach (var controller in moverControllers)
        {
            controller.Activate();
        }
        inUse = true;
        if (isSingleUse)
        {
            if (transform.childCount>0)
            {
                transform.GetChild(0).GetComponent<Renderer>().material = usedMaterial;
            }
            Destroy(this);
            
        }
    }

    private void Release()
    {
        foreach(var controller in moverControllers)
        {
            controller.Deactivate();
        }
        inUse = false;
    }
}
