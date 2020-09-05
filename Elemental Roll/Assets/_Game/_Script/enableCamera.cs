using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableCamera : MonoBehaviour
{
    public void doEnableCamera()
    {
        Camera cam = this.GetComponent<Camera>();
        cam.enabled = true;
    }

    public void doDisableCamera()
    {
        Camera cam = this.GetComponent<Camera>();
        cam.enabled = false;
    }
}
