using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform object_transform;
    public bool picked_up = false;

    [SerializeField] private Rigidbody object_rigidbody;
    [SerializeField] private ParticleSystem landing_particle;
    [SerializeField] private LayerMask ground_mask;
    [SerializeField] private LayerMask safe_zone;

    [SerializeField] private ObjectiveController obj_controller;

    private bool is_grounded = true;
    private bool played_particle = true;
    private bool is_safe = false;

    private void Update()
    {
        pickUpCheck();
        groundCheck();
        playParticle();
    }

    private void pickUpCheck()
    {
        if (picked_up)
        {
            object_rigidbody.isKinematic = true;
        }
        else
        {
            object_rigidbody.isKinematic = false;
        }
    }

    private void groundCheck()
    {
        is_grounded = Physics.CheckSphere(landing_particle.transform.position, 2f, ground_mask);

        if (!is_grounded)
        {
            played_particle = false;
        }

        is_safe = Physics.CheckSphere(landing_particle.transform.position, 2f, safe_zone);

        if (is_safe)
        {
            obj_controller.complete();
        }
    }

    private void playParticle()
    {
        if (is_grounded && !played_particle)
        {
            landing_particle.Play();
            played_particle = true;
        }
    }
}
