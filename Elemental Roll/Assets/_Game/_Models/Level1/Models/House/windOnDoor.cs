using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windOnDoor : MonoBehaviour
{
    public float amplitude = 1f;
    public float speed = 0.5f;
    private float offset;
    private void Start()
    {
        offset = transform.eulerAngles.y;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.000f)
        {
            transform.Rotate(Vector3.right, Mathf.Sin(Time.time * speed) * amplitude);
        }
    }
}
