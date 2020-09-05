using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class findCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        LookAtConstraint lookAtConstraint = this.GetComponent<LookAtConstraint>();
        ConstraintSource constraint = new ConstraintSource();
        constraint.sourceTransform = mainCamera.transform;
        constraint.weight = 1f;
        lookAtConstraint.AddSource(constraint);
    }

}
