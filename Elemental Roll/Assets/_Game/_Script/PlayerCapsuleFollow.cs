using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorHelper;
using static AccelerationHelper;

/*Player FOLLOWER
 * More than just a simple follower, this object will smooth the movements of the player to adapt it to the camera
 * 
 * It will tilt on the sides, up and down to make all the camera movements for her.
 * 
 * This way, the camera is entirely ignorant of what is happening, she just follows this object around as a 3rd player camera
 * */


public class PlayerCapsuleFollow : MonoBehaviour
{
    public GameObject player;
    private Rigidbody playerRigid;
    private Vector3 offset;
    private float xangle; //A buffer of the old xangle in order to dampen it correctly

    public float damping = 3f;
    private void Start()
    {//We store the initial offset between the player and the camera in order to keep it during the whole level
        offset = player.transform.position - transform.position;
        //We get the player's RigidBody
        playerRigid = player.GetComponent<Rigidbody>();
    }

    private void Update()
    {

        handleFollow();

    }

    public void UpdateRigid()
    {
        playerRigid = player.GetComponent<Rigidbody>();

    }

    /*Follows the player and tilts*/
    private void handleFollow()
    {
        //We get the maximum vertical angle the player can have
        float maxAngle = maxVerticalAngle(playerRigid.velocity);

        //The player's current Y angle
        float currentAngle = transform.eulerAngles.y;

        //The desired angle to go to
        float desiredAngle = Mathf.Atan2(playerRigid.velocity.x, playerRigid.velocity.z) * Mathf.Rad2Deg;


        //We then limit the desired angle to the max angles
        desiredAngle = (Mathf.Abs(Mathf.DeltaAngle(currentAngle, desiredAngle)) < maxAngle) ? desiredAngle : currentAngle + Mathf.Sign(Mathf.DeltaAngle(currentAngle, desiredAngle)) * maxAngle;

        //3 proved to be the perfect damping value

        //LERP
        float yangle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
        //We slightly rotate the camera to give a Tilt impression
        float zangle = Mathf.LerpAngle(transform.eulerAngles.z, Mathf.DeltaAngle(yangle, desiredAngle) / 1.2f, Time.deltaTime * damping / 2f);
        xangle = Mathf.LerpAngle(xangle, -(player.GetComponent<PlayerController>().moveVertical) * 10f, Time.deltaTime * damping);

        //We put that in a quaternion that will be applied on the camera's position
        Quaternion rotation = Quaternion.Euler(0, yangle, 0);
        transform.position = player.transform.position - (rotation * offset);

        transform.position += ((Vector3.up).normalized) * Mathf.Max((-playerRigid.velocity.y) / 30f,-20f);


        transform.LookAt(player.transform);

        transform.RotateAround(player.transform.position, transform.right, xangle);

        float correctAngle = 0f;


        //We limit the tilt angle
        correctAngle = (zangle < 40f || zangle > 320f) ? zangle : (zangle < 180f) ? 40f : 320f;

        //We rotate and slightly put the player on the bottom of the frame
        transform.Rotate(-17, 0, correctAngle);

    }

}
