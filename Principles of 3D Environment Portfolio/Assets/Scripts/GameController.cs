using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Camera player_camera;
    [SerializeField] private Camera ship_camera;

    public bool can_switch_camera = false;

    private void Start()
    {
        player_camera.enabled = true;
        ship_camera.enabled = false;
    }

    private void Update()
    {
        SwitchCamera();
    }

    private void SwitchCamera()
    {
        if (Input.GetKeyDown(KeyCode.E) && can_switch_camera)
        {
            player_camera.enabled = !player_camera.enabled;
            ship_camera.enabled = !ship_camera.enabled;
        }
    }
}
