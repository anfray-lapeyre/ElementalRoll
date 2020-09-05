using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stayOnTopScript : MonoBehaviour
{
    public Transform objectToFollow;
    public Transform orientation;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = objectToFollow.position + Vector3.up * 5f;
        transform.eulerAngles = new Vector3(90, orientation.rotation.eulerAngles.y,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = objectToFollow.position + Vector3.up * 5f;
        transform.eulerAngles = new Vector3(90, orientation.rotation.eulerAngles.y, 0);
    }
}
