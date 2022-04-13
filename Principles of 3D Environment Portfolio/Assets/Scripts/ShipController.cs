using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private GameController game_controller;

    [SerializeField] private CharacterController ship_controller;
    [SerializeField] private Camera ship_camera;
    [SerializeField] private Transform ship_transform;
    [SerializeField] private Animator ship_animator;

    [SerializeField] private float move_speed = 6f;
    [SerializeField] private float turn_speed = 150f;

    private Vector3 ship_direction;
    private float vertical_input;
    private float horizontal_input;

    private bool is_grounded;
    [SerializeField] private LayerMask ground_mask;
    [SerializeField] private Transform ground_check;

    private void Update()
    {
        HorizontalMovement();
        if (ship_camera.enabled)
        {
            VerticalMovement();
            Dismount();
        }
    }

    private void HorizontalMovement()
    {
        if (ship_camera.enabled && !is_grounded)
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

        ship_animator.SetFloat("Horizontal", horizontal_input, 0.1f, Time.deltaTime);

        ship_direction = new Vector3(0, 0, vertical_input);
        ship_direction = transform.TransformDirection(ship_direction);

        if (ship_direction.magnitude >= 0.1f)
        {
            if (vertical_input > 0)
            {
                ship_animator.SetFloat("Speed", 1f, 0.3f, Time.deltaTime);
            }
            else if (vertical_input < 0)
            {
                ship_animator.SetFloat("Speed", -1f, 0.3f, Time.deltaTime);
            }

            ship_controller.Move(ship_direction * move_speed * Time.deltaTime);
        }
        else
        {
            ship_animator.SetFloat("Speed", 0f, 0.3f, Time.deltaTime);
        }
    }

    private void VerticalMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ship_controller.Move(Vector3.up * move_speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ship_controller.Move(Vector3.down * move_speed * Time.deltaTime);
        }
    }

    private void Dismount()
    {
        is_grounded = Physics.CheckSphere(ground_check.position, 0.5f, ground_mask);

        ship_animator.SetBool("Landed", is_grounded);

        if (is_grounded)
        {
            game_controller.can_switch_camera = true;
        }
        else
        {
            game_controller.can_switch_camera = false;
        }
    }
}
