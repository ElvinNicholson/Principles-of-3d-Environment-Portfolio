using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController player_controller;
    [SerializeField] private Transform player_transform;

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
        float z_movement = Input.GetAxis("Vertical");
        float x_rotation = Input.GetAxis("Horizontal") * Time.deltaTime * turn_speed;

        player_transform.Rotate(Vector3.up, x_rotation);

        player_direction = new Vector3(0, 0, z_movement);
        player_direction = transform.TransformDirection(player_direction);

        if (player_direction.magnitude >= 0.1f)
        {
            player_controller.Move(player_direction * move_speed * Time.deltaTime);
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
