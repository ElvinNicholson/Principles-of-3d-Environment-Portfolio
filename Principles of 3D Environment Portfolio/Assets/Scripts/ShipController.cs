using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private GameController game_controller;

    [SerializeField] private CharacterController ship_controller;
    [SerializeField] private Camera ship_camera;
    [SerializeField] private Transform ship_transform;

    [SerializeField] private float move_speed = 6f;
    [SerializeField] private float turn_speed = 150f;

    private Vector3 ship_direction;
    private Vector3 velocity;
    private float vertical_input;
    private float horizontal_input;

    private void Update()
    {
        Movement();
        Interact();
    }

    private void Movement()
    {
        if (ship_camera.enabled)
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

        ship_transform.Rotate(Vector3.up, x_direction);

        // ship_animator.SetFloat("Horizontal", horizontal_input, 0.1f, Time.deltaTime);

        ship_direction = new Vector3(0, 0, vertical_input);
        ship_direction = transform.TransformDirection(ship_direction);

        if (ship_direction.magnitude >= 0.1f)
        {
            if (vertical_input > 0)
            {
                // ship_animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else if (vertical_input < 0)
            {
                // ship_animator.SetFloat("Speed", -1f, 0.1f, Time.deltaTime);
            }

            ship_controller.Move(ship_direction * move_speed * Time.deltaTime);
        }
        else
        {
            // ship_animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }
    }

    private void Interact()
    {
        if (ship_camera.enabled)
        {
            game_controller.can_switch_camera = true;
        }
    }
}
