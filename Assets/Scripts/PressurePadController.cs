using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadController : MonoBehaviour
{
    [SerializeField] private Walkable attachedWalkable;
    [SerializeField] private MovableController[] moverControllers;

    [SerializeField] private bool isOneWay = false;
    [SerializeField] private bool isSingleUse;
    [SerializeField] private Material usedMaterial;

    private bool inUse = false;

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
        if (!isOneWay)
        {
            inUse = true;
        }
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
        if (!isOneWay)
        {
            foreach (var controller in moverControllers)
            {
                controller.Deactivate();
            }
            inUse = false;
        }
    }
}
