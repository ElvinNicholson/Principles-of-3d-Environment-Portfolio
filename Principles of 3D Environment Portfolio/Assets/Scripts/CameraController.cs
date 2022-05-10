using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cam_trans;
    [SerializeField] private Transform min_cam_pos;
    [SerializeField] private Transform max_cam_pos;

    private float camera_distance;
    private float x_distance;

    public bool left_collide = false;
    public bool right_collide = false;

    private void Update()
    {
        cameraOcclusion();
    }

    private void cameraOcclusion()
    {
        RaycastHit hit;

        float x;
        float y;

        float ref_vel = 0f;

        if (Physics.Linecast(min_cam_pos.position, max_cam_pos.position, out hit))
        {
            camera_distance = Mathf.Clamp(hit.distance - 0.35f, -0.35f, 3f);
            y = Mathf.Clamp(hit.distance, 1f, 1.5f);
            x = Mathf.Clamp(hit.distance, 0f, 0.5f);
        }
        else
        {
            camera_distance = 3f;
            y = 1.5f;
            x = 0;
        }

        if (Physics.Raycast(cam_trans.position, Quaternion.Euler(0, 90, 0) * cam_trans.forward, 0.5f))
        {
            right_collide = true;
            left_collide = false;
        }
        else if (Physics.Raycast(cam_trans.position, Quaternion.Euler(0, -90, 0) * cam_trans.forward, 0.5f))
        {
            right_collide = false;
            left_collide = true;
        }

        if (right_collide)
        {
            x = -x;
        }

        float damp_x = Mathf.SmoothDamp(x_distance, x, ref ref_vel, 0.05f);
        x_distance = damp_x;

        cam_trans.localPosition = new Vector3(x_distance, y, -camera_distance);
    }
}
