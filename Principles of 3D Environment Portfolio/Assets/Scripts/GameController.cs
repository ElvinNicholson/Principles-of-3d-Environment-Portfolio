using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Camera player_camera;
    [SerializeField] private GameObject player_object;
    [SerializeField] private Transform player_transform;

    [SerializeField] private Camera ship_camera;
    [SerializeField] private Transform ship_transform;

    [SerializeField] private Camera claw_camera;

    public bool can_switch_camera = false;

    private void Start()
    {
        player_camera.enabled = true;
        ship_camera.enabled = false;
        claw_camera.enabled = false;
    }

    private void Update()
    {
        SwitchCamera();
        if (!player_camera.enabled)
        {
            player_transform.position = ship_transform.TransformPoint(-8f, 0, 8f);
        }
    }

    private void SwitchCamera()
    {
        if (Input.GetKeyDown(KeyCode.P) && can_switch_camera)
        {
            player_camera.enabled = !player_camera.enabled;
            ship_camera.enabled = !ship_camera.enabled;
            claw_camera.enabled = !claw_camera.enabled;

            if (player_camera.enabled)
            {
                player_object.SetActive(true);
            }
            else
            {
                player_object.SetActive(false);
            }
        }
    }
}
