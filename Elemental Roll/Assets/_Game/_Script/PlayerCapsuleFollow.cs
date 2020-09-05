using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCapsuleFollow : MonoBehaviour
{
    public GameObject player;
    public Camera mainCamera;

    private void Start()
    {
    }

    private void Update()
    {
        //The player follower is the object managing the FX, he will then rotate based on the camera, and follow the player
        transform.position = player.transform.position;
        transform.rotation = mainCamera.transform.rotation;

        //transform.LookAt(target.transform);
        /* Vector3 desiredPosition = player.transform.position + offset;
         Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);*/
        //transform.position = player.transform.position;//position;
        //float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        //transform.Rotate(0,  - transform.rotation.eulerAngles.y  , 0);
        //Debug.Log(Mathf.Atan2(playerVelocity.x, playerVelocity.y)*Mathf.Rad2Deg);

    }
}
