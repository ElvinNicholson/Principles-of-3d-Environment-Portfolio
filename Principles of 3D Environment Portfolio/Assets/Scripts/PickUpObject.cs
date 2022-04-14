using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform object_transform;
    public bool picked_up = false;

    [SerializeField] private Rigidbody object_rigidbody;

    private void Update()
    {
        if (picked_up)
        {
            object_rigidbody.useGravity = false;
        }
        else
        {
            object_rigidbody.useGravity = true;
        }
    }
}
