using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRotation : MonoBehaviour
{
    [SerializeField] private Walkable parent;
    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Ida")
        {
            GameManager.SetIdaParent(parent);
        }
    }
}
