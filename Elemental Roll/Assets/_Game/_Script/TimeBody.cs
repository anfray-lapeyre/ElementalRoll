using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{

    private bool isRewinding = false;
    //Actually, Rewind Backward is the actual logical rewinding sense, but I first made the other, so this type of rewinding will be the rewind backward
    private bool isRewindingBackward = false;
    private float recordTime = 7f;

    public bool isPlatform = false;

    private bool isFrozen = false;

    List<PointInTime> pointsInTime;

    Rigidbody rb;
    private float offset=0f;

    // Use this for initialization
    void Start()
    {
        if (pointsInTime == null)
        {
            pointsInTime = new List<PointInTime>();
        }
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    
    void FixedUpdate()
    {
        if (!isFrozen)
        {
            if (isRewinding)
                Rewind();
            else if (isRewindingBackward)
                RewindBackward();
            else
                Record();
        }
        else
        {
            UpdateFreeze();
        }
    }

    public void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[pointsInTime.Count-1];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(pointsInTime.Count-1);
        }
        else
        {
            StopRewind();
        }

    }

    public void RewindBackward()
    {
        if (pointsInTime.Count > 0)
        {
            offset += Time.fixedDeltaTime;
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }

    }

    public void Record()
    {
        if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        if(rb != null && !isPlatform)
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation,rb.velocity));
        else
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    public void StartRewind()
    {
        isRewinding = true;
        if (rb)
        {
             rb.isKinematic = true;
        }
    }

    public void StartRewindBackward()
    {
        isRewindingBackward = true;
        if (rb)
        {
            if (!isPlatform)
                rb.isKinematic = true;
            else
            {
                if (isPlatform && TryGetComponent(out simpleRotatingPlatformScript rotatingPlatform))
                {
                    rotatingPlatform.paused = true;
                }
                if (isPlatform && TryGetComponent(out simpleMovingPlatformScript movingPlatform))
                {
                    movingPlatform.paused = true;
                }
            }
        }

    }

    public void StopRewind()
    {
        if (isRewinding || isRewindingBackward)
        {
            isRewinding = false;
            if (isRewindingBackward)
            {
                if (isPlatform && TryGetComponent(out simpleRotatingPlatformScript rotatingPlatform))
                { 
                    rotatingPlatform.AddOffset(offset*2f);
                    rotatingPlatform.paused = false;
                }
                if (isPlatform && TryGetComponent(out simpleMovingPlatformScript movingPlatform))
                {
                    movingPlatform.AddOffset(offset*2f);
                    movingPlatform.paused = false;
                }
                offset = 0;
                isRewindingBackward = false;
            }
            if (rb && !isPlatform)
            {
                    rb.isKinematic = false;
                    rb.velocity = pointsInTime[0].velocity;
            }
        }
    }

    //We clone the list of points in time
    public List<PointInTime> GetPointsInTime()
    {
        return new List<PointInTime>(pointsInTime);
    }

    //We clone the list into our own points in time
    public void SetPointsInTime(List<PointInTime> _pointsInTime)
    {

        pointsInTime = new List<PointInTime>(_pointsInTime);

    }


    public void Freeze()
    {
        isFrozen = true;
        //This will make sure that we do not need to check pointsinTime Count in Update
        if (pointsInTime.Count <= 0)
        {
            Record();
        }
        if (rb)
        {
            if (!isPlatform)
                rb.isKinematic = true;
        }
        if (isPlatform && TryGetComponent(out simpleRotatingPlatformScript rotatingPlatform))
        {
            rotatingPlatform.paused = true;
        }
        if (isPlatform && TryGetComponent(out simpleMovingPlatformScript movingPlatform))
        {
            movingPlatform.paused = true;
        }
    }

    public void UpdateFreeze()
    {
        offset += Time.fixedDeltaTime;
        PointInTime pointInTime = pointsInTime[0];
        transform.position = pointInTime.position;
        transform.rotation = pointInTime.rotation;
    }

    public void StopFreeze()
    {
        if (isFrozen)
        {
            isFrozen = false;
            if (isPlatform && TryGetComponent(out simpleRotatingPlatformScript rotatingPlatform))
            {
                rotatingPlatform.AddOffset(offset);
                rotatingPlatform.paused = false;

            }
            if (isPlatform && TryGetComponent(out simpleMovingPlatformScript movingPlatform))
            {
                movingPlatform.AddOffset(offset);
                movingPlatform.paused = false;

            }
            offset = 0;
            isRewindingBackward = false;
            if (rb && !isPlatform)
            {
                rb.isKinematic = false;
                rb.velocity = pointsInTime[0].velocity;
            }
        }
    }

}