using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGunScript : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatisGrappable;
    public Transform gunTip, varCamera, player;
    private float maxDistance = 50f;
    private SpringJoint joint;
    private float lastDistance = 999f;

    void Awake()
    {
        lr=GetComponent<LineRenderer>();
        lr.positionCount = 0;

    }

    private void FixedUpdate()
    {
        if (joint)
        {
            float tmpDist = Vector3.Distance(player.position, grapplePoint);
            if (tmpDist > lastDistance || tmpDist <=1f)
            {

                Rigidbody r = player.GetComponent<Rigidbody>();
                if (r)
                    r.velocity = r.velocity / 10f;
                StopGrapple();
                player.GetComponent<PlayerController>().SetPowerInUse(false);
            }
            lastDistance = Vector3.Distance(player.position, grapplePoint);

        }
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

            joint.spring = 1000f;
            joint.damper =30f;
            joint.massScale = 1f;
            joint.connectedMassScale = 10f;

            lr.positionCount = 2;

            Time.timeScale = 0.6f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

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
        lastDistance = 999f;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * .02f;

    }
}
