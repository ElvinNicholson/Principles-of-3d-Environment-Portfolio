using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private Break bridge;
    [SerializeField] private Camera player_cam;

    public void breakBridge()
    {
        bridge.playBreakAnim();
    }

    public void switchCam()
    {
        player_cam.enabled = true;
        Destroy(gameObject);
    }
}
