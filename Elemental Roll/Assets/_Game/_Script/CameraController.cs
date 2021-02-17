using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorHelper;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float damping = 2f;
    private Vector3 offset;
    private Rigidbody playerRigid;
    private float xangle;
    private float lookHorizontal = 0f;
    private float lookVertical = 0f;
    private float playerControlsCamera = 0f;
    //private bool hasCameBack;
    //private float mouseUsed=-1f;


    private void Start()
    {
        //We store the initial offset between the player and the camera in order to keep it during the whole level
        offset = player.transform.position - transform.position;
        //We get the player's RigidBody
        playerRigid = player.GetComponent<Rigidbody>();

    }

    public void OnLook(InputValue value, bool isMouse)
    {
        lookHorizontal = value.Get<Vector2>().x;
        lookVertical = value.Get<Vector2>().y;

        /*if(lookHorizontal != 0f || lookVertical != 0f)
        {

            playerControlsCamera = 5f;

            hasCameBack = false;
        }*/

    }



  

    private void rotateCameraWithVelocity()
    {
        //We get the players velocity
        Vector2 playerVelocity = new Vector2(playerRigid.velocity.x, playerRigid.velocity.z);
        //The player's current Y angle
        float currentAngle = transform.eulerAngles.y;
        //The desired angle to go to
        float desiredAngle = Mathf.Atan2(playerVelocity.x, playerVelocity.y) * Mathf.Rad2Deg;
        //The max angle is defined based on the player's velocity to avoid weird turns when the player's stopped
        float maxAngle = 70f * Mathf.Min((Mathf.Abs(playerVelocity.x) + Mathf.Abs(playerVelocity.y)), 3f) / 3f;
        //We then limit the desired angle to the max angles
        desiredAngle = (Mathf.Abs(Mathf.DeltaAngle(currentAngle, desiredAngle)) < maxAngle) ? desiredAngle : currentAngle + Mathf.Sign(Mathf.DeltaAngle(currentAngle, desiredAngle)) * maxAngle;


        /* if(!hasCameBack && Mathf.Abs(Mathf.DeltaAngle(currentAngle, desiredAngle)) < 10f)
         {
             hasCameBack = true;
         }

         float realDamping = (hasCameBack) ? damping : damping / 100f;*/
        float realDamping = 3f;

        //LERP
        float yangle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * realDamping);
        //We slightly rotate the camera to give a Tilt impression
        float zangle =  Mathf.LerpAngle(transform.eulerAngles.z, Mathf.DeltaAngle(yangle, desiredAngle), Time.deltaTime * realDamping /1f);
        xangle = Mathf.LerpAngle(xangle, -(player.GetComponent<PlayerController>().moveVertical) * 10f, Time.deltaTime * realDamping * 2);


        //We put that in a quaternion that will be applied on the camera's position
        Quaternion rotation = Quaternion.Euler(0, yangle, 0);
        transform.position = player.transform.position - (rotation * offset);


        transform.position += ((-Physics.gravity).normalized) * (-playerRigid.velocity.y) / 10f;

        transform.LookAt(player.transform);

        transform.RotateAround(player.transform.position, transform.right, xangle);

        float correctAngle = 0f;


        //We limit the tilt angle
        correctAngle = (zangle < 40f || zangle > 320f) ? zangle : (zangle < 180f) ? 40f : 320f;

        //We rotate and slightly put the player on the bottom of the frame
        transform.Rotate(-17, 0, correctAngle);

        transform.position -= RotateY(new Vector3(Mathf.Sin(correctAngle * Mathf.Deg2Rad) * 0.8f, 0, 0), Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
    }

    private void FixedUpdate()
    {
        if (playerControlsCamera > 0f)
        {
            playerControlsCamera -= Time.fixedDeltaTime;
        }
    }

    //After all the updates
    private void LateUpdate()
    {
        //Camera control with joystick & mouse disabled
        /*if (playerControlsCamera > 0f)
        {
            transform.position = player.transform.position - offset;
            rotateCameraWithJoystick();
        }
        else
        {*/
            rotateCameraWithVelocity();
            
        //}
            


    }


    // ------------------------ LEGACY CODE ENABLING CAMERA CONTROL WITH JOYSTICK -----------------------------------------

    /* private void rotateCameraWithJoystick()
   {
       float currentAngle = transform.eulerAngles.y;
       //The desired angle to go to
       float desiredAngle = transform.eulerAngles.y + lookHorizontal;

       float yangle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

       float zangle = Mathf.LerpAngle(transform.eulerAngles.z, 0f, Time.deltaTime * damping / 2);

       if (mouseUsed >= 0f)
       {

           mouseUsed -= Time.fixedTime;
           if (Mathf.Abs(lookVertical) > 0)
           {
               xangle = Mathf.Clamp(Mathf.LerpAngle(xangle, -(player.GetComponent<PlayerController>().moveVertical) * 10f + lookVertical, Time.deltaTime * damping * 2),-30f,80f);
           }


       }
       else
       {
           xangle = Mathf.Clamp(Mathf.LerpAngle(xangle, -(player.GetComponent<PlayerController>().moveVertical) * 10f + lookVertical, Time.deltaTime * damping * 2), -30f,80f);
       }



       //We put that in a quaternion that will be applied on the camera's position
       Quaternion rotation = Quaternion.Euler(0, yangle, 0);
       transform.position = player.transform.position - (rotation * offset);

       transform.position += ((-Physics.gravity).normalized) * (-playerRigid.velocity.y) / 10f;


       transform.LookAt(player.transform);

       transform.RotateAround(player.transform.position, transform.right, xangle);

       float correctAngle = (zangle < 30f || zangle > 330f) ? zangle : (zangle < 180f) ? 30f : 330f;
       transform.Rotate(-17, 0, correctAngle);
       transform.position -= RotateY(new Vector3(Mathf.Sin(correctAngle * Mathf.Deg2Rad) * 0.8f, 0, 0), Mathf.Deg2Rad * transform.rotation.eulerAngles.y);

   }*/
}
