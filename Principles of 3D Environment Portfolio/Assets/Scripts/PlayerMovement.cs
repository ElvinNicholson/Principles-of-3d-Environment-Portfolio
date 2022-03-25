using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController player_controller;
    [SerializeField] private Transform player_transform;
    [SerializeField] private Transform model_transform;
    [SerializeField] private Animator player_animator;

    [SerializeField] private float move_speed = 6f;
    [SerializeField] private float turn_speed = 150f;

    private Vector3 player_direction;
    private Vector3 velocity;

    private bool is_grounded;
    [SerializeField] private LayerMask ground_mask;
    [SerializeField] private float gravity;

    void Update()
    {
        Gravity();
        Movement();
    }

    private void Movement()
    {
        float z_direction = Input.GetAxis("Vertical");
        float x_direction = Input.GetAxis("Horizontal") * Time.deltaTime * turn_speed;

        player_transform.Rotate(Vector3.up, x_direction);

        player_animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"), 0.1f, Time.deltaTime);

        player_direction = new Vector3(0, 0, z_direction);
        player_direction = transform.TransformDirection(player_direction);

        if (player_direction.magnitude >= 0.1f)
        {
            if (z_direction > 0)
            {
                player_animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else if (z_direction < 0)
            {
                player_animator.SetFloat("Speed", -1f, 0.1f, Time.deltaTime);
            }

            player_controller.Move(player_direction * move_speed * Time.deltaTime);
        }
        else
        {
            player_animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }
    }

    private void Gravity()
    {
        is_grounded = Physics.CheckSphere(transform.position, 0.2f, ground_mask);

        if (is_grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime;
        player_controller.Move(velocity * Time.deltaTime * 2);
    }
}
