using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cam_trans;
    [SerializeField] private Transform cam_parent_trans;

    private Vector3 camera_direction;
    private float camera_distance;

    private void Start()
    {
        camera_direction = cam_trans.localPosition.normalized;
    }

    private void Update()
    {
        cameraOcclusion();
    }

    private void cameraOcclusion()
    {
        RaycastHit hit;

        if (Physics.Linecast(cam_parent_trans.position, cam_trans.position, out hit))
        {
            camera_distance = Mathf.Clamp(hit.distance, 0.5f, 3f);
        }
        else
        {
            camera_distance = 3f;
        }

        cam_trans.localPosition = new Vector3(0, camera_direction.y * camera_distance, -camera_distance);
    }
}
