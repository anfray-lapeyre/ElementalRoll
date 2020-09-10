using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSlimeRotationScript : MonoBehaviour
{
    private float rotatingSpeed = 1f;
    private bool mustSpeedUp = false;
    public GameObject[] slimes;

    public GameObject portal;
    public GameObject tracy;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.down, rotatingSpeed);
        if (mustSpeedUp)
        {
            rotatingSpeed += 1f;
        }
    }


    public void SpeedUp()
    {
        mustSpeedUp = true;
        Invoke("Disappear", 2f);
    }

    public void Disappear()
    {
        this.transform.LeanScale(Vector3.zero, 1f);
        Destroy(this.gameObject, 1f);
    }

    void OnDestroy()
    {
        tracy.SetActive(true);
        tracy.transform.LeanScale(Vector3.one * 0.25f, 1f);
        portal.SetActive(true);
        portal.transform.LeanScale(Vector3.one * 0.2f, 0.5f);
        
    }
}
