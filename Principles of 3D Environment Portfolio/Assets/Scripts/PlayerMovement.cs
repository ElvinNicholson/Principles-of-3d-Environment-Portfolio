using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController player_controller;
    public Transform main_cam;

    public float speed = 6f;

    float smoothTurnVelocity;

    void Update()
    {
        Vector3 player_direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (player_direction.magnitude >= 0.1f)
        {
            float player_targetAngle = Mathf.Atan2(player_direction.x, player_direction.z) * Mathf.Rad2Deg + main_cam.eulerAngles.y;
            float player_angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, player_targetAngle, ref smoothTurnVelocity, 0.2f);
            transform.rotation = Quaternion.Euler(0f, player_angle, 0f);

            Vector3 direction = Quaternion.Euler(0f, player_targetAngle, 0f) * Vector3.forward;
            player_controller.Move(direction * speed * Time.deltaTime);
        }
    }
}
