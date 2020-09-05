using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    private float minDistance = 0.5f;
    private float maxDistance = 2.817f;
    public GameObject player;
    public float smooth = 10.0f;
    public float distance;

    // Start is called before the first frame update
    void Awake()
    {
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredCameraPos = transform.position - transform.parent.forward*maxDistance;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, desiredCameraPos, out hit, maxDistance))
        {
            distance = maxDistance -  Mathf.Clamp((hit.distance), minDistance, maxDistance);
        }
        else
        {
            distance = 0;
        }
        transform.position = Vector3.Lerp(transform.position, transform.parent.position +( transform.parent.forward * distance), Time.deltaTime * smooth);
    }
}
