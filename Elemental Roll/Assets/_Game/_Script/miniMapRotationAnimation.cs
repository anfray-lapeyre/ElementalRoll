using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMapRotationAnimation : MonoBehaviour
{
    private float aimedAngle;
    private float damping=0.0001f;
    // Start is called before the first frame update
    void Start()
    {
        aimedAngle = Random.Range(-180f, 180f);
    }

    // Update is called once per frame
    void Update()
    {
        float currentAngle = transform.eulerAngles.z;

        if (Mathf.DeltaAngle(currentAngle, aimedAngle) < 4f)
        {
            aimedAngle =Random.Range(-180f, 180f);
            if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, aimedAngle)) < 15f)
            {
                aimedAngle += Mathf.Sign(aimedAngle) * 15f;
            }
        }
        currentAngle = Mathf.LerpAngle(currentAngle, aimedAngle, Time.fixedTime* damping);
        transform.eulerAngles = new Vector3(0,0,currentAngle);

    }
}
