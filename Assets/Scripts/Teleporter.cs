using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Teleporter : MonoBehaviour
{
    public Walkable attachedWalkable;
    public Walkable target;

    private void OnTriggerStay(Collider other)
    {
        //Ida is on pressure pad
        if (other.tag == "Ida")
        {
            if (other.transform.position == attachedWalkable.GetWalkPoint())
            {
                other.transform.position = target.GetWalkPoint();
                other.transform.parent = target.transform;
                other.transform.localRotation = Quaternion.identity;
                other.GetComponent<PlayerController>().targetCube = target;
                
            }
        }
    }
}
