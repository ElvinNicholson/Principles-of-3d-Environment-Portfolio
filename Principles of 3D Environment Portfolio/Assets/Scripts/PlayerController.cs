using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController player_controller;
    [SerializeField] private Transform player_transform;
    [SerializeField] private Animator player_animator;
    [SerializeField] private Camera player_camera;
    [SerializeField] private Transform player_raycast;

    [SerializeField] private float move_speed = 6f;
    [SerializeField] private float turn_speed = 150f;

    [SerializeField] private GameController game_controller;

    private Vector3 player_direction;
    private Vector3 velocity;
    private float vertical_input;
    private float horizontal_input;

    private bool is_grounded;
    [SerializeField] private LayerMask ground_mask;
    [SerializeField] private float gravity;

    private void Update()
    {
        Gravity();
        Movement();
        if (player_camera.enabled)
        {
            Interact();
        }
    }

    private void Movement()
    {
        if (player_camera.enabled)
        {
            vertical_input = Input.GetAxis("Vertical");
            horizontal_input = Input.GetAxis("Horizontal");
        }
        else
        {
            vertical_input = 0f;
            horizontal_input = 0f;
        }

        float x_direction = horizontal_input * Time.deltaTime * turn_speed;

        player_transform.Rotate(Vector3.up, x_direction);

        player_animator.SetFloat("Horizontal", horizontal_input, 0.1f, Time.deltaTime);

        player_direction = new Vector3(0, 0, vertical_input);
        player_direction = transform.TransformDirection(player_direction);

        if (player_direction.magnitude >= 0.1f)
        {
            if (vertical_input > 0)
            {
                player_animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else if (vertical_input < 0)
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

        if (!is_grounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;
        }
        
        player_controller.Move(velocity * Time.deltaTime * 2);
    }

    private void Interact()
    {
        Ray ray = new Ray(player_raycast.position, player_raycast.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5))
        {
            ShipController ship = hit.collider.GetComponent<ShipController>();

            if (ship != null)
            {
                game_controller.can_switch_camera = true;
            }
        }
        else
        {
            game_controller.can_switch_camera = false;
        }
    }
}
