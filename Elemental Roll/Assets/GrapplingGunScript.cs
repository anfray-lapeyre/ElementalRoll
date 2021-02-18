using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGunScript : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatisGrappable;
    public Transform gunTip, varCamera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;

    void Awake()
    {
        lr=GetComponent<LineRenderer>();
        lr.positionCount = 0;

    }

    private void LateUpdate()
    {

        DrawRope();
    }

    public void StartGrapple()
    {
        Debug.DrawLine(varCamera.position, varCamera.position + varCamera.forward * 30f, Color.red, 1f);
        RaycastHit hit;
        if(Physics.Raycast(varCamera.position, varCamera.forward, out hit, maxDistance, whatisGrappable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = 1f;

            joint.spring = 50f;
            joint.damper =4f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    private void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);

    }

    public void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
}
