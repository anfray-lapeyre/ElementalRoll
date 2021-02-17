using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformScript : MonoBehaviour
{

    public Vector3[] points;
    private int pointNumber = 0;
    private Vector3 currentTarget;

    private float tolerance;
    public float speed;
    public float delayTime;

    private float delayStart;

    public bool automatic;

    void Awake()
    {
        TimeBody tb = this.gameObject.AddComponent<TimeBody>();
        tb.isPlatform = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(points.Length > 0)
        {
            currentTarget = points[0];
        }
        tolerance = speed * Time.deltaTime*1.0001f;
    }

    // Update is called once per frame
    void Update()
    {
         if(transform.localPosition != currentTarget)
        {
            MovePlatform();
        }
        else
        {
            UpdateTarget();
        }
    }

    void MovePlatform()
    {
        Vector3 heading = currentTarget - transform.localPosition;
        transform.localPosition += (heading / heading.magnitude) * speed * Time.deltaTime;
        if(heading.magnitude < tolerance)
        {
            transform.localPosition = currentTarget;
            delayStart = Time.time;
        }
    }

    void UpdateTarget()
    {
        if (automatic)
        {
            if (Time.time - delayStart > delayTime)
            {
                NextPlatform();
            }

        }
    }

    public void NextPlatform()
    {
        pointNumber++;
        if (pointNumber >= points.Length)
        {
             pointNumber = 0;
        }
        currentTarget = points[pointNumber];
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent.parent.parent = transform;
        if(!automatic && transform.localPosition == currentTarget)
        {
            NextPlatform();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent.parent.parent = null;
    }
}
