using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earthElementScript : MonoBehaviour
{
    public FloatVariable rotationSpeed;
    public FloatVariable playerSpeed;
    private float baseYValue;
    private Rigidbody player;
    private bool needsResetRotation = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        baseYValue = transform.position.y;
        player = transform.GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float yangle = Mathf.Atan2(player.velocity.x, player.velocity.z) * Mathf.Rad2Deg ;
        if (playerSpeed.value < 80f)
        {
            transform.eulerAngles = new Vector3(Mathf.Sin(Time.fixedTime * 4) * rotationSpeed.value * 2f, yangle,0);
            if (!needsResetRotation)
            {
                needsResetRotation = true;
            }
        }
        else
        {
            if (needsResetRotation)
            {
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                needsResetRotation = false;
            }
            transform.localPosition =new Vector3(transform.localPosition.x,Mathf.Clamp( baseYValue -(playerSpeed.value - 79f) / 100f, -0.15f,0) , transform.localPosition.z);
        }
    }
}
