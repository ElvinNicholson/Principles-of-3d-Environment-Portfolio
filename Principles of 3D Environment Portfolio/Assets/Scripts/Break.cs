using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    [SerializeField] private GameObject broken_object;

    private void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            breakObject();
        }
    }

    private void breakObject()
    {
        Instantiate(broken_object, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
