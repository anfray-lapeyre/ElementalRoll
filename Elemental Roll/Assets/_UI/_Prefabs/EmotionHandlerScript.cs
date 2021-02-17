using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;


public class EmotionHandlerScript : MonoBehaviour
{
    [Header("Player Objects that contains use the material")]
    //We will duplicate the given Materials and apply them on the player
    //That way we can have multiple players that have different appearances without specific dev
    public MeshRenderer[] body;
    public MeshRenderer[] eyes;

    //Next I'll define what contains each
    /* Fire body : 
     * 0 : Fire
     * 
     * Fire eyes : 
     * 
     * 2 eyes
     * 
     * 
     * Ice body : 
     * 
     * 8 shards
     * 
     * Ice eyes :
     * 
     * 2 eyes
     * 
     * 
     * Earth body : 
     * 0 : Exterior 
     * 1 : Interior Core
     * 
     * Earth eyes : 
     * 2 eyes
     * 
     * 
     * Death body : 
     * 0 : skull
     * 1 : Main flame
     * 2 - 4 : eye flames
     * 
     * 
     * Death eyes : 
     * 0 : Main eye
     * 1-3 : eyes
     */

    public int playerNb=0;

    private PlayerController player;
    [Header("Common")]

    public Material playerMaterial;
    public Material playerEyeMaterial;
    public int typeOfPlayer = 0; //0 -> Byle 1-> Tracy 2-> Rocky 3->Death

    [Header("Earth specific")]

    public Material playerSecondMaterial;

    //***********************************************    EYES    ******************************************************
    //ID -> EyeAperture
    private float currentEyeAperture;

    const string EyeAperture = "EyeAperture";
    private int EAID;
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
    const string EyeBrowOrientation = "EyeBrowOrientation";
    private int EBOID;
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
    const string EyeBrowIntensity = "EyeBrowIntensity";
    private int EBIID;
    //Default eyebrow intensity 0
    const float EBIDEFAULT = 0f;
    //Eyebrow appears when intensity goes at 0.6 and is full at 0.9
    const float EBIAPPEARS = 0.6f;
    const float EBIHALF = 0.84f;
    const float EBIFULL = 0.9f;

    //ID -> BlinkIntensity
    //Blinking
    const string BlinkIntensity = "BlinkIntensity";
    private int BIID;
    //Open 0
    const float BIOPEN = 0f;
    //Closed 1
    const float BICLOSED = 1f;

    const float BLINKOFTEN = 4f;
    const float BLINKSOMETIMES = 10f;


    //***********************************************    FIRE    ******************************************************
    //ID -> FlameIntensity
    private float currentFlameIntensity;
    const string FlameIntensity = "FlameIntensity";
    private int FIID;
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
    const string FlameHue = "FlameHue";
    private int FHID;
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
    private iceElementScript iceParts;

    const string IceHue = "IceHue";
    private int IHID;

    //***********************************************    EARTH    ******************************************************
    
        //Earth Exterior displace speed
    const string EarthDisplaceSpeed = "EarthDisplaceSpeed";
    private int EEDSID;
    const float EEDSNORMAL = -2f;
    const float EEDSSLIGHTLYFAST = 3f;
    const float EEDSREALLYFAST = 10f;

    //Earth Exterior Displace Intensity
    const string EarthDisplaceIntensity = "EarthDisplaceIntensity";
    private int EEDIID;
    const float EEDISMALL = -0.5f;
    const float EEDISLIGHTLYSMALL = 0.3f;
    const float EEDINORMAL = 1f;
    const float EEDISLIGHTLYINTENSE = 2.5f;
    const float EEDIINTENSE = 5f;

    //Earth Exterior Hue
    //Will not be used, but we store it just ine case
    const string EarthHue = "EarthHue";
    private int EEHID;

    //Earth Exterior Color intensity
    const string EarthIntensity = "EarthIntensity";
    private int EEIID;
    const float EEIBLACK = 0.2f;
    const float EEINORMAL = 2f;
    const float EEIINTENSE = 5f;

    //Earth Interior Hue
    const string EarthInteriorHue = "EarthInteriorHue";
    private int EIHID;

    const float EIHYELLOW = 15f;
    const float EIHORANGE = 0f;
    const float EIHRED = -5f;

    //Earth Interior Color intensity
    const string EarthInteriorIntensity = "EarthInteriorIntensity";
    private int EIIID;
    const float EIIBLACK = 0f;
    const float EIILESSINTENSE = 0.1f;
    const float EIINORMAL = 0.2f;
    const float EIISLIGHTLYINTENSE = 1f;
    const float EIIINTENSE = 2f;

    //***********************************************    DEATH    ******************************************************
    [Header("Death specific")]
    private Material firstEye;
    private Material secondEye;
    private Material thirdEye;
    private Material firstFire;
    private Material secondFire;
    private Material thirdFire;
    public Material mainFire;

    const string deathFlameHue = "deathFlameHue";
    private int DHID;

    const float DEATHBLUE = -140f;
    const float DEATHPURPLE = -100f;
    const float DEATHPINK = -20f;


    const string deathFlameAlpha = "deathFlameAlpha";
    private int DAID;

    const float DAVERYFULL = 1f;
    const float DAFULL = 0.472f;
    const float DAEMPTY = 0.3f;


    const string deathDisplacementIntensity = "deathDisplacementIntensity";
    private int DDIID;

    const float DDIHIGH = 0.2f;
    const float DDINORMAL = 0.5f;
    const float DDILOW = 0.8f;



   //***********************************************    EMOTIONS   *****************************************************

    private CameraShakeInstance shake;
    // Start is called before the first frame update
    void Awake()
    {
        //We start by cloning the main eye material 
        playerEyeMaterial = new Material(playerEyeMaterial);

        
        player = GetComponent<PlayerController>();

        InitializeMaterialIDs();


        InitializeCharacterEmotion();

        NormalMood();


        //We start the blinking cycle

        Invoke("Blink", Random.Range(BLINKOFTEN,BLINKSOMETIMES));

    }


    public void InitializeMaterialIDs()
    {
        //Eyes
        EAID = Shader.PropertyToID(EyeAperture);
        
        EBOID = Shader.PropertyToID(EyeBrowOrientation);

        EBIID = Shader.PropertyToID(EyeBrowIntensity);

        BIID = Shader.PropertyToID(BlinkIntensity);


        switch (typeOfPlayer)
        {
            case 0: //Byle
                FIID = Shader.PropertyToID(FlameIntensity);
                FHID = Shader.PropertyToID(FlameHue);
                break;
            case 1: //Tracy
                IHID = Shader.PropertyToID(IceHue);
                break;
            case 2: //Rocky
                EEDSID = Shader.PropertyToID(EarthDisplaceSpeed);
                EEDIID = Shader.PropertyToID(EarthDisplaceIntensity);
                EEIID = Shader.PropertyToID(EarthIntensity);
                EEHID = Shader.PropertyToID(EarthHue);
                EIHID = Shader.PropertyToID(EarthInteriorHue);
                EIIID = Shader.PropertyToID(EarthInteriorIntensity);
                break;
            case 3: //Tim
                DHID = Shader.PropertyToID(deathFlameHue);
                DAID = Shader.PropertyToID(deathFlameAlpha);
                DDIID = Shader.PropertyToID(deathDisplacementIntensity);
                break;
        }
    }


    public void InitializeCharacterEmotion()
    {
        //We initialize each value to the deafult
        playerEyeMaterial.SetFloat(BIID, BIOPEN); //Blink Intensity
        playerEyeMaterial.SetFloat(EAID, EADEFAULT); //Eye Aperture
        playerEyeMaterial.SetFloat(EBOID, EBODEFAULT); //EyeBrow Orientation
        playerEyeMaterial.SetFloat(EBIID, EBIDEFAULT); //EyeBrow Intensity
        if (typeOfPlayer == 0)
        {
            foreach (MeshRenderer eye in eyes)
            {
                Material[] mats = new Material[eye.materials.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = playerEyeMaterial;
                }
                eye.materials = mats;
            }

            playerMaterial = new Material(playerMaterial);

            foreach (MeshRenderer bodypart in body)
            {
                Material[] mats = new Material[bodypart.materials.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = playerMaterial;
                }
                bodypart.materials = mats;
            }

            playerMaterial.SetFloat(FHID, DEFAULTRED + playerNb * 45f);
            playerMaterial.SetFloat(FIID, FIDEFAULT);
        }
        else if (typeOfPlayer == 1)
        {
            foreach (MeshRenderer eye in eyes)
            {
                Material[] mats = new Material[eye.materials.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = playerEyeMaterial;
                }
                eye.materials = mats;
            }

            iceParts = this.GetComponentInChildren<iceElementScript>();
            playerMaterial = new Material(playerMaterial);

            foreach (MeshRenderer bodypart in body)
            {
                Material[] mats = new Material[bodypart.materials.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = playerMaterial;
                }
                bodypart.materials = mats;
            }

            playerMaterial.SetFloat(IHID, +playerNb * 45f);

            NormalStrength();
        }
        else if (typeOfPlayer == 2)
        {
            Material[] mats;
            foreach (MeshRenderer eye in eyes)
            {
                mats = new Material[eye.materials.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = playerEyeMaterial;
                }
                eye.materials = mats;
            }

            playerMaterial = new Material(playerMaterial);
            playerSecondMaterial = new Material(playerSecondMaterial);
            //Exterior Core
            MeshRenderer bodypart = body[0];

            mats = new Material[bodypart.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = playerMaterial;
            }
            bodypart.materials = mats;


            //Interior Core
            bodypart = body[1];

            mats = new Material[bodypart.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = playerSecondMaterial;
            }
            bodypart.materials = mats;

            playerMaterial.SetFloat(EEDSID, EEDSNORMAL);
            playerMaterial.SetFloat(EEDIID, EEDINORMAL);
            playerMaterial.SetFloat(EEIID, EEINORMAL);
            playerMaterial.SetFloat(EIHID, EIHORANGE + playerNb * 45f);
            playerMaterial.SetFloat(EIIID, EIINORMAL);
        }
        else if (typeOfPlayer == 3)
        {
            //First we clone the materials
            firstEye = new Material(playerEyeMaterial);
            secondEye = new Material(playerEyeMaterial);
            thirdEye = new Material(playerEyeMaterial);

            mainFire = new Material(mainFire);
            firstFire = new Material(mainFire);
            secondFire = new Material(mainFire);
            thirdFire = new Material(mainFire);

            //Then apply them
            //Main Eye
            MeshRenderer eye = eyes[0];
            Material[] mats = new Material[eye.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = playerEyeMaterial;
            }
            eye.materials = mats;

            //First Eye

            eye = eyes[1];
            mats = new Material[eye.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = firstEye;
            }
            eye.materials = mats;


            //Second Eye
            eye = eyes[2];
            mats = new Material[eye.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = secondEye;
            }
            eye.materials = mats;


            //Third Eye
            eye = eyes[3];
            mats = new Material[eye.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = thirdEye;
            }
            eye.materials = mats;

            //First is the skull
            MeshRenderer bodypart = body[0];

            mats = new Material[bodypart.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = playerMaterial;
            }
            bodypart.materials = mats;

            //Then the main fire & the other 3
            bodypart = body[1];

            mats = new Material[bodypart.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = mainFire;
            }
            bodypart.materials = mats;


            //First secondary fire
            bodypart = body[2];

            mats = new Material[bodypart.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = firstFire;
            }
            bodypart.materials = mats;


            //Second secondary fire
            bodypart = body[3];

            mats = new Material[bodypart.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = secondFire;
            }
            bodypart.materials = mats;


            //Third secondary fire
            bodypart = body[4];

            mats = new Material[bodypart.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = thirdFire;
            }
            bodypart.materials = mats;

            enableEye(1);
            enableEye(2);
            enableEye(3);
            mainFire.SetFloat(DDIID, DDINORMAL); //Death Main Flame Intensity
            mainFire.SetFloat(DAID, DAFULL); //Death Main Flame Alpha
        }
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
        else if (typeOfPlayer == 1)
        {
            iceParts.Tighten();
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial,EEDSID, EEDSSLIGHTLYFAST,0.3f);
            setValue(playerMaterial, EEDIID, EEDISLIGHTLYSMALL, 0.3f);
            setValue(playerMaterial, EEIID, EEINORMAL,0.3f);
            setValue(playerSecondMaterial, EIHID, EIHORANGE + playerNb * 45f, 0.3f);
            setValue(playerSecondMaterial, EIIID, EIILESSINTENSE,0.3f);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire,DDIID, DDILOW,0.3f); //Death Main Flame Intensity
            setValue(mainFire,DAID, DAFULL, 0.3f); //Death Main Flame Alpha
        }

    }

    public void ReallyWorried()
    {
        //Really worried = eyebrows low, slightly sad and slightly large aperture, fire intensity low & blue
        setValue(playerEyeMaterial, EAID, EASLIGHTLYLARGE, 0.1f);
        setValue(playerEyeMaterial, EBOID, EBOSAD, 0.1f);
        setValue(playerEyeMaterial, EBIID, EBIHALF, 0.1f);
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FIID, FIVERYWEAK, 0.2f);
            setValue(playerMaterial, FHID, ORANGE + playerNb * 45f, 0.2f);

        }
        else if (typeOfPlayer == 1)
        {
            iceParts.Tighten();
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial, EEDSID, EEDSSLIGHTLYFAST, 0.1f);
            setValue(playerMaterial, EEDIID, EEDISMALL, 0.1f);
            setValue(playerMaterial, EEIID, EEINORMAL, 0.1f);
            setValue(playerSecondMaterial,EIHID, EIHRED + playerNb * 45f, 0.1f);
            setValue(playerSecondMaterial,EIIID, EIILESSINTENSE, 0.1f);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire,DDIID, DDILOW, 0.3f); //Death Main Flame Intensity
            setValue(mainFire,DAID, DAEMPTY, 0.3f); //Death Main Flame Alpha
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
        else if (typeOfPlayer == 1)
        {
            iceParts.Enlarge();
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial, EEDSID, EEDSSLIGHTLYFAST, 0.3f);
            setValue(playerMaterial, EEDIID, EEDISLIGHTLYINTENSE, 0.3f);
            setValue(playerMaterial, EEIID, EEIINTENSE, 0.3f);
            setValue(playerSecondMaterial, EIHID, EIHORANGE + playerNb * 45f, 0.3f);
            setValue(playerSecondMaterial, EIIID, EIISLIGHTLYINTENSE, 0.3f);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire, DDIID, DDIHIGH, 0.3f); //Death Main Flame Intensity
            setValue(mainFire, DAID, DAFULL, 0.3f); //Death Main Flame Alpha
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
        else if (typeOfPlayer == 1)
        {
            iceParts.EnlargeMax();
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial, EEDSID, EEDSREALLYFAST, 0.3f);
            setValue(playerMaterial, EEDIID, EEDIINTENSE, 0.3f);
            setValue(playerMaterial, EEIID, EEIINTENSE, 0.3f);
            setValue(playerSecondMaterial, EIHID, EIHORANGE + playerNb * 45f, 0.3f);
            setValue(playerSecondMaterial, EIIID, EIIINTENSE, 0.3f);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire, DDIID, DDIHIGH, 0.3f); //Death Main Flame Intensity
            setValue(mainFire, DAID, DAVERYFULL, 0.3f); //Death Main Flame Alpha
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
        else if (typeOfPlayer == 1)
        {
            iceParts.TightenMin();
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial, EEDSID, EEDSNORMAL, 0.3f);
            setValue(playerMaterial, EEDIID, EEDISLIGHTLYSMALL, 0.3f);
            setValue(playerMaterial, EEIID, EEIINTENSE, 0.3f);
            setValue(playerSecondMaterial, EIHID, EIHRED + playerNb * 45f, 0.3f);
            setValue(playerSecondMaterial, EIIID, EIISLIGHTLYINTENSE, 0.3f);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire, DDIID, DDILOW, 0.3f); //Death Main Flame Intensity
            setValue(mainFire, DAID, DAFULL, 0.3f); //Death Main Flame Alpha
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
            setValue(playerMaterial, FHID , DEFAULTRED + playerNb * 45f, 0.4f);
        }
        else if (typeOfPlayer == 1)
        {
            iceParts.backToNormal();
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial, EEDSID, EEDSNORMAL, 0.3f);
            setValue(playerMaterial, EEDIID, EEDINORMAL, 0.3f);
            setValue(playerMaterial, EEIID, EEINORMAL, 0.3f);
            setValue(playerSecondMaterial, EIHID, EIHORANGE + playerNb * 45f, 0.3f);
            setValue(playerSecondMaterial, EIIID, EIINORMAL, 0.3f);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire, DDIID, DDINORMAL, 0.3f); //Death Main Flame Intensity
            setValue(mainFire, DAID, DAFULL, 0.3f); //Death Main Flame Alpha
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
        CameraShaker shaker = this.GetComponentInChildren<CameraShaker>();

        if (shaker != null)
        {
            shaker.DefaultRotInfluence = Vector3.zero;

            shaker.DefaultPosInfluence = new Vector3(0f,(typeOfPlayer != 2)? 2f : 1f, 0f);
            shaker.ShakeOnce(1f, 0.5f, 1f, 1f);
        }
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FIID, FIVERYSTRONG * 1.5f, 0.5f, true);

            setValue(playerMaterial, FHID, MAGMARED + playerNb * 45f, 0.7f,true);
        }
        else if (typeOfPlayer == 1)
        {
            iceParts.Enlarge();
            Invoke("NormalStrength", 0.5f);
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial, EEDSID, EEDSREALLYFAST, 0.5f,true);
            setValue(playerMaterial, EEDIID, EEDIINTENSE, 0.5f, true);
            setValue(playerMaterial, EEIID, EEIINTENSE, 0.5f, true);
            setValue(playerSecondMaterial, EIHID, EIHORANGE + playerNb * 45f, 0.5f, true);
            setValue(playerSecondMaterial, EIIID, EIIINTENSE, 0.5f, true);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire, DDIID, DDIHIGH, 0.5f,true); //Death Main Flame Intensity
            setValue(mainFire, DAID, DAVERYFULL, 0.5f,true); //Death Main Flame Alpha
        }
        setValue(playerEyeMaterial, EAID, EALARGE, 0.5f, true);



    }

    public void Strong()
    {
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FHID , BRIGHTRED + playerNb * 45f, 0.5f);
        }
        else if (typeOfPlayer == 1)
        {
            iceParts.Enlarge();
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial, EEDSID, EEDSREALLYFAST, 0.5f);
            setValue(playerMaterial, EEDIID, EEDIINTENSE, 0.5f);
            setValue(playerMaterial, EEIID, EEIINTENSE, 0.5f);
            setValue(playerSecondMaterial, EIHID, EIHYELLOW + playerNb * 45f, 0.5f);
            setValue(playerSecondMaterial, EIIID, EIIINTENSE, 0.5f);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire, DDIID, DDIHIGH, 0.3f); //Death Main Flame Intensity
            setValue(mainFire, DAID, DAVERYFULL, 0.3f); //Death Main Flame Alpha
        }
    }

    public void NormalStrength()
    {
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FHID, DEFAULTRED + playerNb * 45f, 0.5f);
        }
        else if (typeOfPlayer == 1)
        {
            iceParts.backToNormal();
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial, EEDSID, EEDSNORMAL, 0.5f);
            setValue(playerMaterial, EEDIID, EEDINORMAL, 0.5f);
            setValue(playerMaterial, EEIID, EEINORMAL, 0.5f);
            setValue(playerSecondMaterial, EIHID, EIHORANGE + playerNb * 45f, 0.5f);
            setValue(playerSecondMaterial, EIIID, EIINORMAL, 0.5f);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire, DDIID, DDINORMAL, 0.3f); //Death Main Flame Intensity
            setValue(mainFire, DAID, DAFULL, 0.3f); //Death Main Flame Alpha
        }
    }

    public void Weak()
    {
        if (typeOfPlayer == 0)
        {
            setValue(playerMaterial, FHID, LIGHTBLUE + playerNb * 45f, 0.5f);
        }
        else if (typeOfPlayer == 1)
        {
            iceParts.Tighten();
        }
        else if (typeOfPlayer == 2)
        {
            setValue(playerMaterial, EEDSID, EEDSNORMAL, 0.5f);
            setValue(playerMaterial, EEDIID, EEDISLIGHTLYSMALL, 0.5f);
            setValue(playerMaterial, EEIID, EEIBLACK, 0.5f);
            setValue(playerSecondMaterial, EIHID, EIHRED + playerNb * 45f, 0.5f);
            setValue(playerSecondMaterial, EIIID, EIILESSINTENSE, 0.5f);
        }
        else if (typeOfPlayer == 3)
        {
            setValue(mainFire, DDIID, DDILOW, 0.3f); //Death Main Flame Intensity
            setValue(mainFire, DAID, DAEMPTY, 0.3f); //Death Main Flame Alpha
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
        CameraShaker shaker = this.GetComponentInChildren<CameraShaker>();
        if (shaker != null)
        {
            shaker.DefaultRotInfluence = new Vector3(0f, 0f, 1000f);
            shaker.DefaultPosInfluence = Vector3.zero;

            shaker.ShakeOnce(1f, 0.5f, 1f, 1f);
        }
        PropellUp();
    }

    public void Roll()
    {
        if (shake == null)
        {
            CameraShaker shaker = this.GetComponentInChildren<CameraShaker>();
            if (shaker != null)
            {
                shaker.DefaultRotInfluence = new Vector3(0f, 0f, 1000f);
                shaker.DefaultPosInfluence = Vector3.zero;

                shake = shaker.StartShake(1f, 0.5f, 1f);
            }
            Worried();
        }

    }

    public void StopRolling()
    {
        if (shake != null)
        {
            shake.StartFadeOut(1f);
            shake = null;
            NormalMood();
        }
    }

    public void disableEye(int nb)
    {
        if(typeOfPlayer == 3)
        {
            switch (nb)
            {
                case 1:
                    setValue(firstEye, BIID, BICLOSED, 0.3f);
                    setValue(firstFire, DAID, DAEMPTY, 0.3f);
                    setValue(firstFire, DHID, DEATHPINK + playerNb * 45f, 0.3f);
                    break;
                case 2:
                    setValue(secondEye, BIID, BICLOSED, 0.3f);
                    setValue(secondFire, DAID, DAEMPTY, 0.3f);
                    setValue(secondFire, DHID, DEATHPINK + playerNb * 45f, 0.3f);


                    break;
                case 3:
                    setValue(thirdEye, BIID, BICLOSED, 0.3f);
                    setValue(thirdFire, DAID, DAEMPTY, 0.3f);
                    setValue(thirdFire, DHID, DEATHPINK + playerNb * 45f, 0.3f);

                    break;
            }
        }
    }

    public void enableEye(int nb)
    {
        if(typeOfPlayer == 3)
        {
            switch (nb)
            {
                case 1:
                    setValue(firstEye, BIID, BIOPEN, 0.3f);
                    setValue(firstFire, DAID, DAFULL, 0.3f);
                    setValue(firstFire, DHID, DEATHBLUE + playerNb * 45f, 0.3f);
                    break;
                case 2:
                    setValue(secondEye, BIID, BIOPEN, 0.3f);
                    setValue(secondFire, DAID, DAFULL, 0.3f);
                    setValue(secondFire, DHID, DEATHBLUE + playerNb * 45f, 0.3f);


                    break;
                case 3:
                    setValue(thirdEye, BIID, BIOPEN, 0.3f);
                    setValue(thirdFire, DAID, DAFULL, 0.3f);
                    setValue(thirdFire, DHID, DEATHBLUE + playerNb * 45f, 0.3f);

                    break;
            }
        }
    }

    private void setValue(Material material, int ID, float targetValue, float time, bool pingpong = false)
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
