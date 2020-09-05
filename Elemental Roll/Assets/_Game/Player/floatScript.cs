using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatScript : MonoBehaviour
{
    private float offset;
    private Vector3 basePos;
    public float modifier = 0.8f;
    public bool verticalMovement = true;
    public bool horizontalMovement = true;
    // Start is called before the first frame update
    void Start()
    {
        basePos = transform.localPosition;
        offset = Random.value*10;
        Move();
    }

    private void Move()
    {
            
            transform.localPosition = new Vector3((horizontalMovement) ? basePos.x + Mathf.Sin(Time.fixedTime + offset) / (16f*modifier) : transform.localPosition.x, (verticalMovement)? basePos.y + Mathf.Cos(Time.fixedTime + offset) / (18f*modifier) : transform.localPosition.y, (horizontalMovement) ?  basePos.z + Mathf.Sin(Time.fixedTime + offset) / (20f*modifier) : transform.localPosition.z);

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
