using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingCrownScript : MonoBehaviour
{
    private float offset;
    private Vector3 baseOffset;
    public float modifier = 0.8f;
    private Transform player;
    private Quaternion baseRotation;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.transform;
        baseOffset = player.position - transform.position;
        baseRotation = transform.rotation;
        offset = Random.value * 10;
        Move();
    }

    private void Move()
    {

        transform.position = player.position - baseOffset + new Vector3( 0,  Mathf.Cos((Time.fixedTime + offset)*1.5f) / (18f * modifier), 0);
        transform.rotation = baseRotation;
        transform.Rotate(Vector3.forward, Mathf.Sin((Time.fixedTime + offset) * 2f) *6f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
