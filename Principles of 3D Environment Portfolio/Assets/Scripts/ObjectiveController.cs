using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveController : MonoBehaviour
{
    [SerializeField] private Text obj_text;
    [SerializeField] private Image marker;
    [SerializeField] private Transform marker_target;
    [SerializeField] private Camera player_cam;
    [SerializeField] private Camera ship_cam;

    private bool marker_enabled = false;

    private void Update()
    {
        updateMarker();
    }

    public void carPickedUp()
    {
        obj_text.text = "Drop off the car in the safe zone";
        enableMarker();
    }

    public void complete()
    {
        obj_text.text = "Rescue successful!";
        disableMarker();
    }

    public void enableMarker()
    {
        marker_enabled = true;
        marker.enabled = true;
    }

    public void disableMarker()
    {
        marker_enabled = false;
        marker.enabled = false;
    }

    private void updateMarker()
    {
        if (player_cam.isActiveAndEnabled && marker_enabled)
        {
            moveMarker(player_cam);
        }
        else if (ship_cam.isActiveAndEnabled && marker_enabled)
        {
            moveMarker(ship_cam);
        }
    }

    private void moveMarker(Camera camera)
    {
        marker.transform.position = camera.WorldToScreenPoint(marker_target.position);

        if (Vector3.Dot((marker_target.position - camera.transform.position), camera.transform.forward) < 0)
        {
            marker.enabled = false;
        }
        else
        {
            marker.enabled = true;
        }
    }
}
