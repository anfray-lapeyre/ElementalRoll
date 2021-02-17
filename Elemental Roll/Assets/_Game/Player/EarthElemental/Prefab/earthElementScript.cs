using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earthElementScript : MonoBehaviour
{
   
    private float baseYValue;
    private Rigidbody player;
    private bool needsResetRotation = false;
    private PlayerController playerData;
    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        baseYValue = transform.localPosition.y;
        player = transform.GetComponentInParent<Rigidbody>();
        playerData = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isKinematic)
        {
            float yangle = Mathf.Atan2(player.velocity.x, player.velocity.z) * Mathf.Rad2Deg;
            if (ActualSave.actualSave.stats[playerData.playerNb].playerSpeed < 80f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, baseYValue - ((ActualSave.actualSave.stats[playerData.playerNb].playerSpeed) / 80f) * 0.1f, transform.localPosition.z);
                transform.eulerAngles = new Vector3(Mathf.Sin(Time.fixedTime * 4) * ActualSave.actualSave.stats[playerData.playerNb].playerRotationSpeed * 2f, yangle, 0);
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
                transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Clamp(baseYValue - (ActualSave.actualSave.stats[playerData.playerNb].playerSpeed - 79f) / 100f, -0.15f, 0), transform.localPosition.z);
            }
        }
    }
}
