using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;


public class EmotionHandlerScript : MonoBehaviour
{
    public Material playerMaterial;
    public Material playerEyeMaterial;
    public int typeOfPlayer = 0; //0 -> Byle 1-> Tracy 2-> Rocky 3->Death

    //***********************************************    EYES    ******************************************************
    //ID -> EyeAperture
    private float currentEyeAperture;
    const string EAID = "EyeAperture";
    //Default eye aperture 0.06
    const float EADEFAULT = 0.06f;
    //Thin eye aperture 0.03
    const float EATHIN = 0.03f;
    //Slightly large aperture 0.07
    const float EASLIGHTLYLARGE = 0.07f;

    //Large eye aperture 0.09
    const float EALARGE = 0.09f;

    //ID -> EyeBrowOrientation
    private float currentEyeBrowOrientation;
    const string EBOID = "EyeBrowOrientation";
    //Default eyebrow orientation 1.52
    const float EBODEFAULT = 1.52f;
    //Slightly angry 1.62
    const float EBOSLIGHTANGER = 1.62f;
    //Really angry 1.74
    const float EBOSTRONGANGER = 1.74f;
    //Tired/Slightly sad 1.46
    const float EBOTIRED = 1.46f;
    //Really Sad 1.34
    const float EBOSAD = 1.34f;

    //ID -> EyeBrowIntensity
    private float currentEyeBrowIntensity;
    const string EBIID = "EyeBrowIntensity";
    //Default eyebrow intensity 0
    const float EBIDEFAULT = 0f;
    //Eyebrow appears when intensity goes at 0.6 and is full at 0.9
    const float EBIAPPEARS = 0.6f;
    const float EBIFULL = 0.9f;

    //ID -> BlinkIntensity
    //Blinking
    const string BIID = "BlinkIntensity";
    //Open 0
    const float BIOPEN = 0f;
    //Closed 1
    const float BICLOSED = 1f;

    const float BLINKOFTEN = 4f;
    const float BLINKSOMETIMES = 10f;


    //***********************************************    FIRE    ******************************************************
    //ID -> FlameIntensity
    private float currentFlameIntensity;
    const string FIID = "FlameIntensity";
    //Flame Intensity 
    //Really Weak 0.003
    const float FIVERYWEAK = 0.003f;
    //Weak 0.015
    const float FIWEAK = 0.005f;
    //Normal 0.023
    const float FIDEFAULT = 0.008f;
    //Strong 0.032
    const float FISTRONG = 0.012f;
    //Really Strong  0.038
    const float FIVERYSTRONG = 0.017f;

    //ID -> FlameHue
    const string FHID = "FlameHue";
    //Hue

    //Bright Red 60
    const float BRIGHTRED = 60f;
    //Magma Red 45
    const float MAGMARED = 45f;
    //Reversed Orange 30
    const float REVERSEDORANGE = 30f;
    //Orange 10
    const float ORANGE = 10f;
    //Normal 0
    const float DEFAULTRED = 0f;
    //Pink -41
    const float PINK = -41f;
    //Purple -90
    const float PURPLE = -90f;
    //Dark Blue -120
    const float DARKBLUE = -120f;
    //Light Blue -140
    const float LIGHTBLUE = -140f;
    //Ghoulish Blue -200
    const float GHOULISHBLUE = -200f;
    //Slimy Green -230
    const float SLIMYGREEN = -230f;
    //Lime -275
    const float LIME = -275f;

    //***********************************************    ICE    ******************************************************




    //***********************************************    EARTH    ******************************************************




    //***********************************************    DEATH    ******************************************************


    private CameraShakeInstance shake;
    // Start is called before the first frame update
    void Awake()
    {
        //We initialize each value to the deafult
        playerEyeMaterial.SetFloat(BIID, BIOPEN); //Blink Intensity
        playerEyeMaterial.SetFloat(EAID, EADEFAULT); //Eye Aperture
        playerEyeMaterial.SetFloat(EBOID, EBODEFAULT); //EyeBrow Orientation
        playerEyeMaterial.SetFloat(EBIID, EBIDEFAULT); //EyeBrow Intensity
        if (typeOfPlayer == 0)
        {
            playerMaterial.SetFloat(FHID, DEFAULTRED);
            playerMaterial.SetFloat(FIID, FIDEFAULT);
        }

        //We start the blinking cycle

        Invoke("Blink", Random.Range(BLINKOFTEN,BLINKSOMETIMES));
        
    }

    public void Worried()
    {
        //Worried = eyebrows low, slightly sad and slightly large aperture, fire intensity normal

        setValue(playerEyeMaterial, EAID, EASLIGHTLYLARGE, 0.3f);
        setValue(playerEyeMaterial, EBOID, EBOTIRED, 0.3f);
        setValue(playerEyeMaterial, EBIID, EBIFULL, 0.3f);
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FIID, FIWEAK, 0.3f);
        }

    }

    public void Angry()
    {
        //Angry = eyebrows low, slightly angry and normal aperture, fire intensity slightly high

        setValue(playerEyeMaterial, EAID, EADEFAULT, 0.3f);
        setValue(playerEyeMaterial, EBOID, EBOSLIGHTANGER, 0.3f);
        setValue(playerEyeMaterial, EBIID, EBIFULL, 0.3f);
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FIID, FISTRONG, 0.3f);
        }
    }

    public void ReallyAngry()
    {
        //Angry = eyebrows low,  angry , small aperture && fire burning
        setValue(playerEyeMaterial, EAID, EATHIN, 0.3f);
        setValue(playerEyeMaterial, EBOID, EBOSTRONGANGER, 0.3f);
        setValue(playerEyeMaterial, EBIID, EBIFULL, 0.3f);
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FIID, FIVERYSTRONG, 0.3f);
        }
    }

    public void Tired()
    {
        Worried();
    }

    public void Sad()
    {
        //SAD = eyebrows low,  sad , high aperture && fire normal
        setValue(playerEyeMaterial, EAID, EALARGE, 0.3f);
        setValue(playerEyeMaterial, EBOID, EBOSAD, 0.3f);
        setValue(playerEyeMaterial, EBIID, EBIFULL, 0.3f);
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FIID, FIVERYWEAK, 0.3f);
        }
    }


    public void NormalMood()
    {
        //We reset eye aperture, eyebrow orientation, eyebrow intensity, fire intensity normal
        setValue(playerEyeMaterial, EAID, EADEFAULT, 0.3f);
        setValue(playerEyeMaterial, EBOID, EBODEFAULT, 0.3f);
        setValue(playerEyeMaterial, EBIID, EBIDEFAULT, 0.3f);
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FIID, FIDEFAULT, 0.3f);
            setValue(playerMaterial, FHID, DEFAULTRED, 0.5f);
        }

    }

    public void NormalEye()
    {
        //We reset eye aperture, eyebrow orientation, eyebrow intensity
        setValue(playerEyeMaterial, EAID, EADEFAULT, 0.3f);
        setValue(playerEyeMaterial, EBOID, EBODEFAULT, 0.3f);
        setValue(playerEyeMaterial, EBIID, EBIDEFAULT, 0.3f);
    }

    public void Bonus()
    {
        Invoke("PowerUp", 0.5f);
    }

    public void PowerUp()
    {
        CameraShaker.GetInstance("FaceCamera").DefaultRotInfluence = Vector3.zero;
        CameraShaker.GetInstance("FaceCamera").DefaultPosInfluence = new Vector3(0f, 2f, 0f);
        CameraShaker.GetInstance("FaceCamera").ShakeOnce(1f, 0.5f, 1f, 1f);

        if (typeOfPlayer == 0)
        {
            LeanTween.value(playerMaterial.GetFloat(FIID), FIVERYSTRONG * 1.5f, 0.5f).setLoopPingPong(1).setEaseOutElastic().setOnUpdate((float currentValue) => { playerMaterial.SetFloat(FIID, currentValue); });
        }
        setValue(playerEyeMaterial, EAID, EALARGE, 0.5f, true);


    }

    public void Strong()
    {
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FHID, BRIGHTRED, 0.5f);
        }
    }

    public void NormalStrength()
    {
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FHID, DEFAULTRED, 0.5f);
        }
    }

    public void Weak()
    {
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FHID, LIGHTBLUE, 0.5f);
        }
    }

    public void Blink()
    {
        setValue(playerEyeMaterial, BIID, BICLOSED, 0.3f, true);
        Invoke("Blink", Random.Range(BLINKOFTEN, BLINKSOMETIMES));
    }


    public void PropellUp()
    {
        Sad();
        Invoke("NormalMood", 1f);
    }


    public void Shock()
    {
        CameraShaker.GetInstance("FaceCamera").DefaultRotInfluence = new Vector3(0f, 0f, 1000f);
        CameraShaker.GetInstance("FaceCamera").DefaultPosInfluence = Vector3.zero;

        CameraShaker.GetInstance("FaceCamera").ShakeOnce(1f, 0.5f, 1f, 1f);
        PropellUp();
    }

    public void Roll()
    {
        if (shake == null)
        {
            CameraShaker.GetInstance("FaceCamera").DefaultRotInfluence = new Vector3(0f, 0f, 1000f);
            CameraShaker.GetInstance("FaceCamera").DefaultPosInfluence = Vector3.zero;

            shake = CameraShaker.GetInstance("FaceCamera").StartShake(1f, 0.5f, 1f);
            Worried();
        }

    }

    public void StopRolling()
    {
        if (shake != null)
        {
            print("Stop");
            shake.StartFadeOut(1f);
            shake = null;
            NormalMood();
        }
    }

    private void setValue(Material material, string ID, float targetValue, float time, bool pingpong = false)
    {
        if (pingpong)
        {
            LeanTween.value(material.GetFloat(ID), targetValue, time).setLoopPingPong(1).setOnUpdate((float currentValue) => { material.SetFloat(ID, currentValue);  });
        }
        else
        {
            LeanTween.value(material.GetFloat(ID), targetValue, time).setOnUpdate((float currentValue) => { material.SetFloat(ID, currentValue); });
        }
    }

}
