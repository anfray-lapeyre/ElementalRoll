using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireElementScript : MonoBehaviour
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
        playerData = transform.GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float yangle = Mathf.Atan2(player.velocity.x, player.velocity.z) * Mathf.Rad2Deg + 180f;
        if (ActualSave.actualSave.stats[playerData.playerNb].playerSpeed < 80f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, baseYValue - ((ActualSave.actualSave.stats[playerData.playerNb].playerSpeed) / 80f) * 0.1f, transform.localPosition.z);
            transform.eulerAngles = new Vector3(Mathf.Sin(Time.fixedTime * 4) * ActualSave.actualSave.stats[playerData.playerNb].playerRotationSpeed * 2f, yangle,0);
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
            transform.localPosition =new Vector3(transform.localPosition.x,baseYValue - Mathf.Min(((ActualSave.actualSave.stats[playerData.playerNb].playerSpeed) / 100f) * 0.3f,0.3f), transform.localPosition.z);
        }
    }
}
