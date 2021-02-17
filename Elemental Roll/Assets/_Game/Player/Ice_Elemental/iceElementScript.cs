using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceElementScript : MonoBehaviour
{

    private float baseYValue;
    private Rigidbody player;
    private bool needsResetRotation = false;

    public GameObject[] playerParts;
    private Vector3[] startOffset;
    private Quaternion[] startRotation;
    private int placement; //0=Tight; 1=Normal; 2= Large
    private PlayerController playerData;

    // Start is called before the first frame update
    void Start()
    {
        startOffset = new Vector3[playerParts.Length];
        startRotation = new Quaternion[playerParts.Length];
        for(int i=0; i < playerParts.Length; i++)
        {
            startOffset[i] = playerParts[i].transform.localPosition;
            startRotation[i] = playerParts[i].transform.localRotation;
        }


        transform.eulerAngles = new Vector3(0, 180, 0);
        baseYValue = transform.localPosition.y;
        player = transform.GetComponentInParent<Rigidbody>();
        playerData = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {

            float yangle = Mathf.Atan2(player.velocity.x, player.velocity.z) * Mathf.Rad2Deg + 180f;
            if (ActualSave.actualSave.stats[playerData.playerNb].playerSpeed < 80f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, baseYValue - ((ActualSave.actualSave.stats[playerData.playerNb].playerSpeed) / 80f) * 0.1f, transform.localPosition.z);

                transform.eulerAngles = new Vector3(Mathf.Sin(Time.fixedTime * 4) * ActualSave.actualSave.stats[playerData.playerNb].playerRotationSpeed * 2, yangle, 0);
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
        

        switch (placement)
        {
            case 0:

                for (int i = 0; i < playerParts.Length; i++)
                {
                   playerParts[i].transform.localPosition = Vector3.Lerp(playerParts[i].transform.localPosition, startOffset[i] * 0.85f , Time.deltaTime * 4f);

                }
                break;
            case 1:
                for (int i = 0; i < playerParts.Length; i++)
                {
                   playerParts[i].transform.localPosition = Vector3.Lerp(playerParts[i].transform.localPosition, startOffset[i]*0.93f, Time.deltaTime * 4f);

                }
                break;
            case 2:

                for (int i = 0; i < playerParts.Length; i++)
                {
                    playerParts[i].transform.localPosition = Vector3.Lerp(playerParts[i].transform.localPosition, startOffset[i] * 1.1f, Time.deltaTime * 4f);

                }
                break;
            case 3:
                for (int i = 0; i < playerParts.Length; i++)
                {
                    playerParts[i].transform.localPosition = Vector3.Lerp(playerParts[i].transform.localPosition, startOffset[i] * 1.3f, Time.deltaTime * 4f);

                }
                break;
            default:
                for (int i = 0; i < playerParts.Length; i++)
                {
                   playerParts[i].transform.localPosition = Vector3.Lerp(playerParts[i].transform.localPosition, startOffset[i] * 1.5f , Time.deltaTime * 4f);

                }
                break;
        }
    }

    public void TightenMin()
    {
        placement = 0;
    }

    public void Tighten()
    {
        placement = 1;
    }

    public void backToNormal()
    {
        placement = 2;
        
    }

    public void Enlarge()
    {
        placement = 3;
    }

    public void EnlargeMax()
    {
        placement = 4;
    }
}
