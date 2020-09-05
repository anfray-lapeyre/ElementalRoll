using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorHelper;
using UnityEngine.InputSystem;


public class AlternateControlScript : MonoBehaviour
{
    [HideInInspector]
    public float moveVertical = 0f;
    [HideInInspector]
    public float moveHorizontal = 0f;

    private GameObject playerCamera;
    private Rigidbody player;
    public float invertSpeedModifier = 2f;
    private Vector3 baseRotation;

    public float maxAngle = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Rigidbody>();
        playerCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.parent.gameObject;
        baseRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        handleMovement();   
    }

    public void OnMove(InputValue value)
    {
        moveHorizontal = value.Get<Vector2>().x;
        moveVertical = value.Get<Vector2>().y;
    }

    void handleMovement()
    {
        //We put that in a vector organized as a velocity, to apply it as a force
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //We rotate it in accordance to the camera, for the force to be applied in a logical manner
        movement = RotateY(movement, Mathf.Deg2Rad * playerCamera.transform.rotation.eulerAngles.y);

        //If the player is trying to go in a different direction than its actual movement, we intensify it to slow down easily
        movement = new Vector3(Mathf.Sign(player.velocity.x) == Mathf.Sign(movement.x) ? movement.x : movement.x * invertSpeedModifier, movement.y, Mathf.Sign(player.velocity.z) == Mathf.Sign(movement.z) ? movement.z : movement.z * invertSpeedModifier);

        if (baseRotation.x - maxAngle < transform.eulerAngles.x + movement.x && transform.eulerAngles.x + movement.x < baseRotation.x + maxAngle)
        {
            transform.RotateAround(player.position, Vector3.forward, movement.x);
        }
        if (baseRotation.z - maxAngle < transform.eulerAngles.z - movement.z && transform.eulerAngles.z - movement.z < baseRotation.z + maxAngle)
        {
            transform.RotateAround(player.position, Vector3.right, -movement.z);
        }


        Debug.Log(movement);
    }
}
