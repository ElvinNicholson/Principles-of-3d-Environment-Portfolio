using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private ObjectiveController obj_controller;

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
    private bool is_near_ground;
    private float ground_distance;
    [SerializeField] private LayerMask ground_mask;
    [SerializeField] private Transform ground_check;
    [SerializeField] private Transform ground_raycast;

    private float claw_lower_raise_val = 0f;
    private bool is_claw_open = false;
    private bool is_claw_used = false;
    [HideInInspector] public bool is_claw_lowered = false;
    [SerializeField] private Transform claw_transform;

    [SerializeField] private ParticleSystem[] ship_particles;
    [SerializeField] private Transform land_particle_transform;

    private float cam_y = 10f;
    float ref_vel = 0f;

    [SerializeField] private GameObject ship_light;

    private void Start()
    {
        foreach (ParticleSystem particle in ship_particles)
        {
            var emission = particle.emission;
            emission.enabled = false;
        }
    }

    private void Update()
    {
        HorizontalMovement();
        lightControl();
        if (ship_camera.enabled)
        {
            VerticalMovement();
            ClawInteractions();
            ClawControls();
            Dismount();
            landingParticleHandler();
            cameraControl();
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

    private void ClawControls()
    {
        if (!is_grounded)
        {
            // Lower Claw
            if (Input.GetKey(KeyCode.Alpha1) && !is_claw_used)
            {
                LowerClaw();
            }

            // Raise Claw
            else if (Input.GetKey(KeyCode.Alpha2) && !is_claw_used)
            {
                RaiseClaw();
            }

            // Open Claw
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                is_claw_open = true;
                ship_animator.SetBool("ClawOpen", is_claw_open);
            }

            // Close Claw
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                is_claw_open = false;
                ship_animator.SetBool("ClawOpen", is_claw_open);
            }
        }
        else if (!is_claw_used && is_grounded)
        {
            close_claw();
        }

        if (claw_lower_raise_val > 0f)
        {
            is_claw_lowered = true;
        }
        else
        {
            is_claw_lowered = false;
        }
    }

    public void close_claw()
    {
        RaiseClaw();

        is_claw_open = false;
        is_claw_lowered = false;
        claw_lower_raise_val = 0f;
        ship_animator.SetBool("ClawOpen", is_claw_open);
    }

    private void ClawInteractions()
    {
        Ray ray = new Ray(claw_transform.position, claw_transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3))
        {
            PickUpObject pickUpObject = hit.collider.GetComponent<PickUpObject>();

            if (pickUpObject != null)
            {
                if (is_claw_open && Input.GetKeyDown(KeyCode.Alpha4))
                {
                    pickUpObject.object_transform.parent = claw_transform;
                    pickUpObject.picked_up = true;
                    is_claw_used = true;
                    ship_controller.height = 20;
                    obj_controller.carPickedUp();
                }

                if (!is_claw_open && Input.GetKeyDown(KeyCode.Alpha3) && pickUpObject.object_transform.parent == claw_transform)
                {
                    pickUpObject.object_transform.parent = null;
                    pickUpObject.picked_up = false;
                    is_claw_used = false;
                    ship_controller.height = 5;
                }
            }
        }
    }

    private void Dismount()
    {
        is_grounded = Physics.CheckSphere(ground_check.position, 0.5f, ground_mask);

        ship_animator.SetBool("Landed", is_grounded);

        if (is_grounded && !is_claw_used)
        {
            game_controller.can_switch_camera = true;
            foreach(ParticleSystem particle in ship_particles)
            {
                var emission = particle.emission;
                emission.enabled = false;
            }
        }
        else
        {
            game_controller.can_switch_camera = false;
            foreach (ParticleSystem particle in ship_particles)
            {
                var emission = particle.emission;
                emission.enabled = true;
            }
        }
    }

    private void LowerClaw()
    {
        if (claw_lower_raise_val < 0.99f)
        {
            claw_lower_raise_val += Time.deltaTime;
        }
        else
        {
            claw_lower_raise_val = 0.99f;
        }

        ship_animator.SetFloat("LowerRaiseClaw", claw_lower_raise_val);
    }

    private void RaiseClaw()
    {
        if (claw_lower_raise_val > 0f)
        {
            claw_lower_raise_val -= Time.deltaTime;
        }
        else
        {
            claw_lower_raise_val = 0f;
        }

        ship_animator.SetFloat("LowerRaiseClaw", claw_lower_raise_val);
    }

    private void findGroundDistance()
    {
        Ray ray = new Ray(ground_raycast.position, ground_raycast.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10, ground_mask))
        {
            if (hit.transform.gameObject.layer == 6)
            {
                ground_distance = hit.distance;
                is_near_ground = true;
            }
        }
        else
        {
            is_near_ground = false;
        }
    }

    private void landingParticleHandler()
    {
        findGroundDistance();

        land_particle_transform.position = ground_check.TransformPoint(0, -ground_distance + 2f, -5.5f);

        var emission = ship_particles[0].emission;
        var shape = ship_particles[0].shape;

        if (is_near_ground)
        {
            shape.radius = 14 - ground_distance;
        }
        else
        {
            emission.enabled = false;
        }
    }

    private void cameraControl()
    {
        if (is_claw_lowered)
        {
            float new_y = 5f;
            cam_y = Mathf.SmoothDamp(cam_y, new_y, ref ref_vel, 0.5f);
            ship_camera.transform.localPosition = new Vector3(0f, cam_y, -33.5f);
        }
        else
        {
            float new_y = 10f;
            cam_y = Mathf.SmoothDamp(cam_y, new_y, ref ref_vel, 0.5f);
            ship_camera.transform.localPosition = new Vector3(0f, cam_y, -33.5f);
        }
    }

    private void lightControl()
    {
        if (ship_camera.enabled)
        {
            ship_light.SetActive(true);
        }
        else
        {
            ship_light.SetActive(false);
        }
    }
}
