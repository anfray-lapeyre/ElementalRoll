using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireElementScript : MonoBehaviour
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
        baseYValue = transform.localPosition.y;
        player = transform.GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float yangle = Mathf.Atan2(player.velocity.x, player.velocity.z) * Mathf.Rad2Deg + 180f;
        if (playerSpeed.value < 80f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, baseYValue - ((playerSpeed.value) / 80f) * 0.04f, transform.localPosition.z);
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
            transform.localPosition =new Vector3(transform.localPosition.x,baseYValue - Mathf.Min(((playerSpeed.value) / 100f) * 0.3f,0.3f), transform.localPosition.z);
        }
    }
}
