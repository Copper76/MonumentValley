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
            Debug.Log(other.transform.position + " " + attachedWalkable.GetWalkPoint());
            if (other.transform.position == attachedWalkable.GetWalkPoint())
            {
                Debug.Log("TELEPORTING");
                other.transform.position = target.GetWalkPoint();
                other.GetComponent<PlayerController>().targetCube = target;
                
            }
        }
    }
}
