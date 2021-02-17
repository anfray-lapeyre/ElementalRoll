using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorHelper;


public static class AccelerationHelper
{
    //We apply a rotation on the Y axis
    public static Vector3 applySpeedCurve(Vector3 _playerVelocity, Vector3 _movement, float _camAngle ,Character _character)
    {
        //We put that in a vector organized as a velocity, to apply it as a force
        Vector3 movement = new Vector3(_movement.x, 0.0f, _movement.z);
        //We rotate it in accordance to the camera, for the force to be applied in a logical manner
        movement = RotateY(movement, Mathf.Deg2Rad * _camAngle);
        //If the player is trying to go in a different direction than its actual movement, we intensify it to slow down easily
        movement = new Vector3(Mathf.Sign(_playerVelocity.x) == Mathf.Sign(movement.x) ? movement.x : movement.x * _character.invertSpeedModifier, movement.y, Mathf.Sign(_playerVelocity.z) == Mathf.Sign(movement.z) ? movement.z : movement.z * _character.invertSpeedModifier);




        //If the player is going fast enough, we amplify the speed in order to give a sensation of higher "horse power"
        float speedAmplifier = Mathf.Max(Mathf.Min(Mathf.Exp(Mathf.Abs(_playerVelocity.x) + Mathf.Abs(_playerVelocity.z)) / 2f, 1.3f), 0);
        if (_movement.x != 0f && _movement.z == 0f && Mathf.Abs(_playerVelocity.x) + Mathf.Abs(_playerVelocity.z) > 10f)
        {
            speedAmplifier /= 2f;
        }
        
        return movement * speedAmplifier * _character.speedModifier;
    }


    public static float maxVerticalAngle(Vector3 playerVelocity)
    {
        return 70f * Mathf.Min((Mathf.Abs(playerVelocity.x) + Mathf.Abs(playerVelocity.z)), 3f) / 3f;
    }
}
