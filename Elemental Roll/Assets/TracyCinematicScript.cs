using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracyCinematicScript : MonoBehaviour
{
    public GameObject icePlayer;
    public GameObject firePlayer;
    private EmotionHandlerScript icePlayerEmotion;
    private EmotionHandlerScript firePlayerEmotion;

    public GameObject icePlayerLookAt;
    public GameObject firePlayerLookAt;
    private bool fireFollowIce = false;
    private bool iceLookEverywhere = false;
    public Vector3[] eyePositions;
    private int eyePositionAim = 0;
    public float eyeSpeed = 1f;
    private bool hasBeenInvoked = false;

    public GameObject firstParticles;
    public GameObject secondParticles;
    // Start is called before the first frame update
    void Start()
    {
        firePlayerEmotion = firePlayer.GetComponent<EmotionHandlerScript>();
        firePlayerEmotion.NormalMood();
        firePlayerEmotion.NormalStrength();
        firePlayerEmotion.NormalEye();
    }

    public void setFirstParticle()
    {
        firstParticles.SetActive(true);

    }

    public void cancelFirstParticle()
    {
        firstParticles.SetActive(false);
    }

    public void setSecondParticle()
    {
        secondParticles.SetActive(true);

    }


    public void cancelSecondParticle()
    {
        secondParticles.SetActive(false);

    }

    public void fireGoUpForCamera()
    {
        firePlayer.transform.LeanMoveLocalY(0f, 0.6f).setLoopPingPong(1);
    }


    public void makeIceComeOut()
    {
        icePlayerEmotion = icePlayer.GetComponent<EmotionHandlerScript>();

        icePlayerEmotion.ReallyAngry();
        icePlayerLookAt.transform.position = firePlayer.transform.position;
        icePlayer.transform.LeanMoveLocalZ(0.7f, 0.5f);
        fireFollowIce = true;
    }


    public void firePowerUp()
    {
        firePlayerEmotion.PowerUp();
        firePlayer.transform.LeanMoveLocalY(-1.5f, 0.2f).setLoopPingPong(1);
    }

    public void goReallyScary()
    {
        firePlayer.transform.LeanMoveLocalY(-1.5f, 0.2f).setLoopPingPong(1);
        firePlayerEmotion.ReallyAngry();
        firePlayerEmotion.Strong();
        this.GetComponent<AudioSource>().Play();

    }

    public void fireNormalStrength()
    {
        firePlayerEmotion.NormalStrength();
        firePlayerEmotion.Angry();
    }

    public void LookAtFire()
    {
        iceLookEverywhere = false;
    }

    public void iceOK()
    {
        icePlayerEmotion.PowerUp();
        firePlayer.transform.LeanMoveLocalY(-1.5f, 0.2f).setLoopPingPong(1);
        icePlayerEmotion.NormalEye();
    }

    public void goReallyWorried()
    {
        icePlayerEmotion.Sad();
        icePlayerEmotion.Weak();
        iceLookEverywhere = true;
    }

    public void changeEyePosition()
    {
        int eye = (int)Random.Range(0, eyePositions.Length-1);
        eyePositionAim = (eye == eyePositionAim) ? (eyePositionAim+1% (eyePositions.Length - 1)) : eye;
        hasBeenInvoked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireFollowIce)
        {
            firePlayerLookAt.transform.position = icePlayer.transform.position;
        }
        if (iceLookEverywhere)
        {
            if(Vector3.Distance(icePlayerLookAt.transform.position,eyePositions[eyePositionAim]) < 0.1f && !hasBeenInvoked)
            {
                Invoke("changeEyePosition", Random.Range(1f,3f));
                hasBeenInvoked = true;
            }
            icePlayerLookAt.transform.position = Vector3.Lerp(icePlayerLookAt.transform.position, eyePositions[eyePositionAim], Time.deltaTime*eyeSpeed);
        }
        else
        {
            icePlayerLookAt.transform.position = firePlayer.transform.position;
        }
    }
}
