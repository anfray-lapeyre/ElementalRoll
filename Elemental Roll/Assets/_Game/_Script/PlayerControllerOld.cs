using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorHelper;
using static LevelLoader;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using TMPro;
using EZCameraShake;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PlayerControllerOld : Observer
{
    private bool loading = false;
    public float speedModifier = 1f;
    public float invertSpeedModifier = 2f;
    public float jumpForce = 11f;
    public GameObject playerCamera;
    private Rigidbody player;
    public ParticleSystem fallSparks;
    public ParticleSystem speedSparks;
    public ParticleSystem grindSparks;
    public FloatVariable playerScore;
    [HideInInspector]
    public float moveVertical = 0f;
    [HideInInspector]
    public float moveHorizontal = 0f;
    private Vector3 respawnPosition;
    private AudioSource bumpSource;
    public float baseVolume = 1f;

    public IntVariable currentLevel;

    private Vector3 lastVelocity;

    public FloatVariable playerSpeed;
    public FloatVariable playerRotationSpeed;
    public GameHandlerScript handler;
    private bool mustPropellUp = false;
    private bool finishedLevel = false;
    private bool hasControls = false;

    private TextMeshProUGUI bonusCount;
    public UIParticleSystem bonusAnimation;
    private int bonusGathered = 0;

    public GameObject LevelLoader; //Base
    private LevelLoader _levelLoader; //Instantiated child

    public GameObject PauseMenu;
    private bool inOptions=false;

    private EmotionHandlerScript emotions;

    public GameObject victoryText;

    public FloatVariable powerGauge;
    public int power = 0;//0=Fire; Ice=1; Rock =2; Death =3
    public FloatVariable maxPowerTime;
    public float powerTime= 500f;
    public GameObject Boom;
    public GameObject propelBoom;
    private int invokingTime;

    private outroAnimationScript victory;
    private SphereCollider playerCollider;

    private Transform StartParent;

    private bool hasUsedPower = false;

    private float finalTime = 999f;

    private int characterUnlocked = 0;
    private bool hasPlayerTouched = false;

    private GameObject persistantHandler;

    public Cinemachine.CinemachineBrain cinemachineBrain;

    public Cinemachine.CinemachineVirtualCamera normalView;
    public Cinemachine.CinemachineVirtualCamera topView;


    //This will contain an object that will enable our player to look somewhere in particular
    public Transform[] eyes;

    private ShoutHandlerScript shoutHandler;
    private bool isLookingDown = false;
    //Value dedicated to Tim
    private bool isRewinding = false;

    private void Start()
    {
        persistantHandler = GameObject.FindGameObjectsWithTag("PersistentObject")[0];
        persistantHandler.GetComponent<InputHandler>().addObserver(this);
        StartParent = this.transform.parent;
        shoutHandler = playerCamera.GetComponentInChildren<ShoutHandlerScript>();
        playerCollider = this.GetComponent<SphereCollider>();
        emotions = this.GetComponent<EmotionHandlerScript>();
        // We get the player's rigidbody's component
        player = GetComponent<Rigidbody>();
        playerRotationSpeed.SetValue(0f);
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawn");
        if(spawners.Length>0)
        { 
            respawnPosition = spawners[0].transform.position;
        }
        else
        {
            respawnPosition = Vector3.one;
        }
        /*GameObject victory = GameObject.FindGameObjectsWithTag("Finish")[0];
        transform.LookAt(victory.transform);*/
        bumpSource = this.gameObject.GetComponentInChildren<AudioSource>();
        baseVolume = bumpSource.volume;    
        bonusCount = GameObject.FindGameObjectsWithTag("BonusCount")[0].GetComponent<TextMeshProUGUI>();

        GameObject levelLoader = Instantiate(LevelLoader);
        _levelLoader = levelLoader.GetComponent<LevelLoader>();
        powerGauge.value = powerTime;
        maxPowerTime.value = powerTime;
    }

    override public void OnNotify(GameObject entity, object notifiedEvent)
    {
        switch (notifiedEvent.GetType().ToString())
        {
            case "MoveCommand":
                if(((MoveCommand)notifiedEvent).isNotJoystick())
                    OnMove(((MoveCommand)notifiedEvent).getMove());
                break;
            case "SpellCommand":
                OnSpecialAction(((SpellCommand)notifiedEvent).isPressed());
                break;
            case "RestartCommand":
                OnRestart(((RestartCommand)notifiedEvent).isPressed());
                break;
            case "PauseCommand":
                OnPause(((PauseCommand)notifiedEvent).isPressed());
                break;
            case "EagleViewCommand":
                //OnPause(((EagleViewCommand)notifiedEvent).isPressed());
                break;
            case "TopViewCommand":
                OnTopView(((TopViewCommand)notifiedEvent).isPressed());
                break;
            default:
                break;
        }
    }

    public void OnMove(Vector2 value)
    {

        if (!inOptions)
        {
            moveHorizontal = value.x;
            moveVertical = value.y;
        }
    }

    public void OnLook(InputValue value)
    {
        if (!inOptions)
        {
            playerCamera.GetComponent<CameraController>().OnLook(value, false);
        }
    }

    public void OnLookMouse(InputValue value)
    {
        if (!inOptions)
        {
            playerCamera.GetComponent<CameraController>().OnLook(value, true);
        }
    }

    public void OnRestart(bool value)
    {
        if (value)
        {
            if (!inOptions)
            {
                if (hasControls)
                    Restart();
            }
        }
    }

    public void OnTopView(bool value)
    {
        if (value)
        {
            normalView.Priority = 10;
            topView.Priority = 15;
        }
        else
        {
            topView.Priority = 10;
            normalView.Priority = 15;
        }
    }

    public void OnSpecialAction(bool value)
    {
        if (value && !inOptions)
        {
            if (hasControls)
            {
                if (powerGauge.value >= powerTime)
                {
                    hasUsedPower = true;
                    hasPlayerTouched = true;
                    switch (power)
                    {
                        case 1: //Ice
                                //Ice platform generation
                            powerGauge.value = 0f;

                            emotions.PowerUp();
                            shoutHandler.PlayAudio(true);

                            if (Gamepad.current != null)
                            {
                                Gamepad.current.SetMotorSpeeds(0.1f, 0.8f);
                            }
                            invokingTime = 15;
                            InvokeIce();
                            break;
                        case 2:
                            //Earth
                            //No idea
                            shoutHandler.PlayAudio(true);
                            emotions.PowerUp();
                            powerGauge.value = 0f;

                            //player.AddForce(-player.velocity);

                            player.isKinematic = true;
                            GameObject instantiatedEarthBoom = Instantiate(Boom, fallSparks.transform);
                            Destroy(instantiatedEarthBoom, 2f);
                            Invoke("EarthMovesAgain", 1.5f);
                            break;
                        case 3:
                            //Death 
                            //Can rewing time thrice per level

                            //We remove a third of the power

                            powerGauge.value = Mathf.Max(powerGauge.value - maxPowerTime.value / 3f,0);
                            //We make sure that when the power gauge reaches 0, the player cannot use the power anymore
                            shoutHandler.PlayAudio(true);

                            DeathRewind();
                            Invoke("DeathStopRewind", 3f);

                            break;
                        default: //Fire
                                 //Upward propulsion
                            powerGauge.value = 0f;

                            emotions.PowerUp();
                            shoutHandler.PlayAudio(true);
                            if (player.velocity.y < 0)
                            {
                                player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
                            }
                            player.AddForce(new Vector3(0f, 0.3f * jumpForce, 0f));
                            if (Gamepad.current != null)
                            {
                                Gamepad.current.SetMotorSpeeds(0.1f, 0.8f);
                            }
                            GameObject instantiatedBoom = Instantiate(Boom);
                            instantiatedBoom.transform.position = this.transform.position;
                            Destroy(instantiatedBoom, 2f);
                            break;
                    }
                }
            }
        }
        else if(!value && !inOptions && isRewinding){
            DeathStopRewind();
        }
    }

    public void InvokeIce()
    {
        if (invokingTime > 0)
        {
            invokingTime--;
            if (player.velocity.y < 0)
                player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
            GameObject instantiatedPlatform = Instantiate(Boom);
            instantiatedPlatform.transform.position = this.transform.position - Vector3.up * 0.55f + new Vector3(player.velocity.normalized.x, 0F, player.velocity.normalized.z);
            Destroy(instantiatedPlatform, 5.5f);
            Invoke("InvokeIce", 0.1f);
        }
    }


    public void EarthMovesAgain()
    {
        player.isKinematic = false;
    }

    public void DeathRewind()
    {
        isRewinding = true;
        TimeBody[] replays = Resources.FindObjectsOfTypeAll(typeof(TimeBody)) as TimeBody[];
        for (int i = 0; i < replays.Length; i++)
        {
            replays[i].StartRewindBackward();
        }

        
        
        Volume[] volume = Resources.FindObjectsOfTypeAll(typeof(Volume)) as Volume[];
        if (volume.Length > 0 && volume[0].profile.TryGet(out ColorAdjustments colorAdjustments))
        {

            colorAdjustments.saturation.overrideState = true;
            LeanTween.value(0f, -70f, 0.3f).setOnUpdate((float val) =>
              {
                  colorAdjustments.saturation.value = val;
              });
            AudioSource source = persistantHandler.GetComponent<AudioSource>();
            AudioSource soundSource = shoutHandler.GetComponent<AudioSource>();
            float characterPitch = new Character().Death().pitch;
            LeanTween.value(1f,-0.9f,0.3f).setOnUpdate((float val) =>
            {
               source.pitch = val;
                soundSource.pitch = val * characterPitch;
                
            });





        }
    }

    public void DeathStopRewind()
    {
        if (isRewinding)
        {
            isRewinding = false;
            TimeBody[] replays = Resources.FindObjectsOfTypeAll(typeof(TimeBody)) as TimeBody[];
            for (int i = 0; i < replays.Length; i++)
            {
                replays[i].StopRewind();
            }
            //This makes it possible again for Tim to use its powers
            powerTime = Mathf.Max(powerGauge.value - 0.01f, 0.5f);
            Volume[] volume = Resources.FindObjectsOfTypeAll(typeof(Volume)) as Volume[];
            if (volume.Length > 0 && volume[0].profile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.saturation.overrideState = true;
                LeanTween.value(-70f, 0f, 0.3f).setOnUpdate((float val) =>
                {
                    colorAdjustments.saturation.value = val;
                });
                AudioSource source = persistantHandler.GetComponent<AudioSource>();
                AudioSource soundSource = shoutHandler.GetComponent<AudioSource>();
                float characterPitch = new Character().Death().pitch;
                LeanTween.value(-0.9f, 1f, 0.3f).setOnUpdate((float val) =>
                {
                    source.pitch = val;
                    soundSource.pitch = val * characterPitch;

                });
            }
            Debug.Log("disableEye : " + (int)powerGauge.value+1);
            emotions.disableEye((int)powerGauge.value+1);

        }
    }


    public void Restart()
    {
        if (!finishedLevel && !loading)
        {
            _levelLoader.LoadNextLevel(Mathf.Abs(currentLevel.value), true);//In case the value is negative while restarting, it means we are in level selection mode, we can put any positive value here
            loading = true;
        }
            
    }

    public void OnPause(bool value)
    {
        if (value)
        {
            if (!inOptions)
            {
                Instantiate(PauseMenu, this.transform);
                inOptions = true;
            }
        }
        
    }

    public void QuitOptions()
    {
        inOptions = false;
    }

    private void handleMovement()
    {
        //We get if the player is trying to move horizontally and vertically
        //moveHorizontal = Input.GetAxis("Horizontal");
        //moveVertical = Input.GetAxis("Vertical");

        //We put that in a vector organized as a velocity, to apply it as a force
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if(!hasPlayerTouched && movement.magnitude != 0f)
        {
            hasPlayerTouched = true;
        }
        //We rotate it in accordance to the camera, for the force to be applied in a logical manner
        movement = RotateY(movement, Mathf.Deg2Rad * playerCamera.transform.rotation.eulerAngles.y);

        //If the player is trying to go in a different direction than its actual movement, we intensify it to slow down easily
        movement = new Vector3(Mathf.Sign(player.velocity.x) == Mathf.Sign(movement.x) ? movement.x : movement.x * invertSpeedModifier, movement.y, Mathf.Sign(player.velocity.z) == Mathf.Sign(movement.z) ? movement.z : movement.z * invertSpeedModifier);

        //If the player is going fast enough, we amplify the speed in order to give a sensation of higher "horse power"
        float speedAmplifier = Mathf.Max(Mathf.Min(Mathf.Exp(Mathf.Abs(player.velocity.x) + Mathf.Abs(player.velocity.y)) / 2f, 1.3f), 0);
        if(moveHorizontal!= 0f && moveVertical == 0f && Mathf.Abs(player.velocity.x) + Mathf.Abs(player.velocity.y)>10f)
        {
            speedAmplifier /= 2f;
        }

        //We will handle here the player's physical referential (if you're on a moving platform, you move with it)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerCollider.bounds.extents.y + 0.1f))
        {
            GameObject RaycastReturn = hit.collider.gameObject;
            if (RaycastReturn.GetComponent<Rigidbody>())
            {
                movement += RaycastReturn.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime; //simpleMovingPlatformScript handling
              }

            if (RaycastReturn.GetComponent<simpleRotatingPlatformScript>())
            {
                //Make the object parent here ! 
                if(this.transform.parent != RaycastReturn.transform)
                    this.transform.SetParent(RaycastReturn.transform, true);
            }
            else
            {
                if (this.transform.parent != StartParent)
                    this.transform.SetParent(StartParent, true);
            }

        }
        else
        {
            if(this.transform.parent != StartParent)
                this.transform.SetParent(StartParent, true);
        }



        //We add it to the player's rigidbody as a Force
        player.AddForce(movement * speedModifier * speedAmplifier);

    }

    private void FixedUpdate()
    {
        
        if (!finishedLevel)
        {
            //We do not increment time if power ==3 because Tim does not need to have it refill. 
            if (powerGauge.value < powerTime && power != 3)
            {
                powerGauge.value += Time.fixedDeltaTime;
            }
            //We handle the player's movement
            if (hasControls)
            {
                handleMovement();
                //We update the speed base on the sum of the player's velocity vector
                if (hasPlayerTouched)
                {
                    playerSpeed.SetValue((Mathf.Abs(player.velocity.x) + Mathf.Abs(player.velocity.y) + Mathf.Abs(player.velocity.z)) * 3f);
                    playerRotationSpeed.SetValue(Mathf.Abs(player.angularVelocity.x) + Mathf.Abs(player.angularVelocity.y) + Mathf.Abs(player.angularVelocity.z));
                    //checkGravity();
                    lastVelocity = player.velocity;
                }
                else
                {
                    player.velocity = player.velocity * 0.2f;
                }
            }
        }
        else
        {
            if (!mustPropellUp)
            {

                player.velocity =new Vector3(player.velocity.x/(1+0.1f/player.velocity.magnitude),player.velocity.y/1.2f, player.velocity.z /(1+0.1f / player.velocity.magnitude));
            }
            else
            {
                player.AddForce(Vector3.up * 15f);
                if (Gamepad.current != null)
                {
                    Gamepad.current.SetMotorSpeeds(0.2f, 0.2f);
                }
                
            }
        }

    }

    private void Update()
    {
        //Handling grind sparks. If the player is too fast and touching the ground
        RaycastHit hit;
        var grindEmission = grindSparks.emission;
        Vector3 direction = Physics.gravity;
        bool isEmittingGrindSparks = false;
        if (Physics.Raycast(transform.position, direction, out hit, 0.67f) && playerSpeed.value > 50f)
        {
            isEmittingGrindSparks = true;
            grindEmission.rateOverTime = Mathf.Min(Mathf.Max((playerSpeed.value - 50f), 0f) / 2f, 20f);
            
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(1f, 1f);
            }

        }

       
        grindEmission.enabled = isEmittingGrindSparks;




        //Handling speed particles
        bool isEmittingSpeedSparks = false;
        ParticleSystem.EmissionModule emission = speedSparks.emission;
        if (playerSpeed.value >= 60f)
        {
            //CameraShaker.GetInstance("MainCamera").ShakeOnce(Mathf.Min((playerSpeed.value-40f)/100f,0.3f), 4f, 0.1f, 0.1f);
            isEmittingSpeedSparks = true;
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0.2f, 0.2f);
            }

            if (playerSpeed.value > 80f)
            {
                if(emotions)
                    emotions.Roll();

            }
            else
            {
                if(emotions)
                    emotions.StopRolling();
            }
        }
        else
        {
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0f, 0f);
            }

        }
        emission.rateOverTime = Mathf.Min(Mathf.Max((playerSpeed.value - 40f), 0f) / 5f, 15f);
        emission.enabled = isEmittingSpeedSparks;



        //Handling being scared when on the border of platform
        if (isPlayerOnBorder())
        {
            if (!isLookingDown)
            {
                emotions.ReallyWorried();

                for (int i = 0; i < eyes.Length; i++)
                {
                    eyes[i].localEulerAngles = (new Vector3(20, 180, 0));
                }
                isLookingDown = true;
            }


            //lookAtTransform.localPosition =
        }
        else
        {
            if (isLookingDown)
            {
                for (int i = 0; i < eyes.Length; i++)
                {
                    eyes[i].localEulerAngles = (new Vector3(0, 180, 0));
                }
                isLookingDown = false;
                emotions.NormalMood();

            }
        }

    }

    private bool isPlayerOnBorder(){
        RaycastHit borderhit;
        float playerSize = playerCollider.bounds.extents.x /1.2f;
       
        bool result = !Physics.Raycast(transform.position + playerSize* fallSparks.transform.right, Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        result = result || !Physics.Raycast(transform.position + playerSize * -fallSparks.transform.right, Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        result = result || !Physics.Raycast(transform.position + playerSize * (-fallSparks.transform.right+ fallSparks.transform.forward), Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        result = result || !Physics.Raycast(transform.position + playerSize * 0.8f*(fallSparks.transform.right + fallSparks.transform.forward), Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        result = result || !Physics.Raycast(transform.position + playerSize * fallSparks.transform.forward, Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        //We make sure there that the player gets worried ONLY if it is on the ground.
        result = result && Physics.Raycast(transform.position, Physics.gravity, out borderhit, playerCollider.bounds.extents.y + 0.1f);
        return result;
    }


    public void enableVictory()
    {
        if (finishedLevel)
        {
            if(characterUnlocked==0) //If characterUnlocked is at zero, either there is no unlocked character, or we still haven't calculated it, so we calculate
                characterUnlocked = isACharacterUnlocked(); //This ensures that when a character is unlocked, it is always considered
            SaveProgress();
            if (characterUnlocked==0) //If no character is unlocked, we can load the level. If a character is loaded, we do not want to do that
            {
                _levelLoader.ShowLoader();
                _levelLoader.LoadNextLevel(Mathf.Abs(currentLevel.value));//In case the value is negative while restarting, it means we are in level selection mode, we can put any positive value here
            }
        }
    }

    private int isACharacterUnlocked()
    {
        if (currentLevel.value < 0)
        {
            int level = -currentLevel.value - 1;
            if (ActualSave.actualSave.levels[level].collectedSlime < bonusGathered)
            {
                if (ActualSave.actualSave.getSlimesCollected() < ActualSave.actualSave.slimesToUnlock(1) && (ActualSave.actualSave.getSlimesCollected() - ActualSave.actualSave.levels[level].collectedSlime + bonusGathered) >= ActualSave.actualSave.slimesToUnlock(1))
                {
                    return 1;
                }
                else if (ActualSave.actualSave.getSlimesCollected() < ActualSave.actualSave.slimesToUnlock(2) && (ActualSave.actualSave.getSlimesCollected() - ActualSave.actualSave.levels[level].collectedSlime + bonusGathered) >= ActualSave.actualSave.slimesToUnlock(2))
                {
                    return 2;
                }
                else if (ActualSave.actualSave.getSlimesCollected() < ActualSave.actualSave.slimesToUnlock(3) && (ActualSave.actualSave.getSlimesCollected() - ActualSave.actualSave.levels[level].collectedSlime + bonusGathered) >= ActualSave.actualSave.slimesToUnlock(3))
                {
                    return 3;
                }
            }
        }
        else
        {

            if (ActualSave.actualSave.levels[currentLevel.value - 1].collectedSlime < bonusGathered)
            {

                if (ActualSave.actualSave.getSlimesCollected() < ActualSave.actualSave.slimesToUnlock(1) && (ActualSave.actualSave.getSlimesCollected() - ActualSave.actualSave.levels[currentLevel.value - 1].collectedSlime + bonusGathered) >= ActualSave.actualSave.slimesToUnlock(1))
                {
                    return 1;

                }
                else if (ActualSave.actualSave.getSlimesCollected() < ActualSave.actualSave.slimesToUnlock(2) && (ActualSave.actualSave.getSlimesCollected() - ActualSave.actualSave.levels[currentLevel.value - 1].collectedSlime + bonusGathered) >= ActualSave.actualSave.slimesToUnlock(2))
                {
                    return 2;
                }
                else if (ActualSave.actualSave.getSlimesCollected() < ActualSave.actualSave.slimesToUnlock(3) && (ActualSave.actualSave.getSlimesCollected() - ActualSave.actualSave.levels[currentLevel.value - 1].collectedSlime + bonusGathered) >= ActualSave.actualSave.slimesToUnlock(3))
                {
                    return 3;
                }
            }
        }
        return 0;
    }

    private void SaveProgress()
    {
        if (currentLevel.value < 0)
        {
            int level = -currentLevel.value - 1;
            if (ActualSave.actualSave.levels[level].collectedSlime < bonusGathered)
            {
                if (characterUnlocked==1)
                {
                    Invoke("loadTracy", 0.5f);
                }else if (characterUnlocked == 2)
                {
                    Invoke("loadRocky", 0.5f);
                }else if (characterUnlocked == 3)
                {
                    Invoke("loadTim", 0.5f);
                }
                if (ActualSave.actualSave.levels[level].hasUsedPower)
                {
                    ActualSave.actualSave.levels[level].hasUsedPower = hasUsedPower;
                }
                ActualSave.actualSave.levels[level].collectedSlime = bonusGathered;
                if (ActualSave.actualSave.levels[level].bestTime > finalTime)
                {
                    ActualSave.actualSave.levels[level].bestTime = finalTime;
                }
            }
            //ActualSave.actualSave.levels[level].collectedSlime = 0;//Take it out after

        }
        else
        {

            ActualSave.actualSave.levels[currentLevel.value-1].beaten = true;
            if (ActualSave.actualSave.levels[currentLevel.value - 1].collectedSlime < bonusGathered)
            {

                if (characterUnlocked == 1)
                {
                    Invoke("loadTracy", 0.5f);
                }
                else if (characterUnlocked == 2)
                {
                    Invoke("loadRocky", 0.5f);
                }
                else if (characterUnlocked == 3)
                {
                    Invoke("loadTim", 0.5f);
                }
                if (ActualSave.actualSave.levels[currentLevel.value - 1].hasUsedPower)
                {
                    ActualSave.actualSave.levels[currentLevel.value - 1].hasUsedPower = hasUsedPower;
                }
                ActualSave.actualSave.levels[currentLevel.value - 1].collectedSlime = bonusGathered;
                if (ActualSave.actualSave.levels[currentLevel.value - 1].bestTime > finalTime)
                {
                    ActualSave.actualSave.levels[currentLevel.value - 1].bestTime = finalTime;
                }
            }
            if (LevelLoader)
            {
                LevelLoader.GetComponent<LevelLoader>().handleDifficultySaveData(currentLevel.value-1);

            }
            else
            {
                //In case there's an error and Level Loader is not spawned, we'll assume it's our fault and give max completion
                Debug.LogError("There has been an error, LevelLoader is not instantiated.");
                ActualSave.actualSave.levels[currentLevel.value-1].beatenInDifficultLife=true;
                ActualSave.actualSave.levels[currentLevel.value-1].beatenInEasyLife = true;
                ActualSave.actualSave.levels[currentLevel.value-1].beatenInNormalLife = true;
                ActualSave.actualSave.levels[currentLevel.value-1].beatinInDifficultTime = true;
                ActualSave.actualSave.levels[currentLevel.value-1].beatinInEasyTime = true;
                ActualSave.actualSave.levels[currentLevel.value-1].beatinInNormalTime = true;
                ActualSave.actualSave.levels[currentLevel.value-1].hasUsedPower = false;

            }

        }

        SaveSystem.SaveGame(ActualSave.actualSave, ActualSave.saveSlot);
    }

    public void loadTracy()
    {
        _levelLoader.ShowLoader();
        _levelLoader.GetComponent<LevelLoader>().LoadCharacterCutscene(1);
    }

    public void loadRocky()
    {
        _levelLoader.ShowLoader();
        _levelLoader.GetComponent<LevelLoader>().LoadCharacterCutscene(2);
    }

    public void loadTim()
    {
        _levelLoader.ShowLoader();
        _levelLoader.GetComponent<LevelLoader>().LoadCharacterCutscene(3);
    }

    private void PropellUp()
    {
        mustPropellUp = true;
        emotions.PowerUp();
        if (player.velocity.y < 0)
        {
            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
        }
        player.AddForce(new Vector3(0f, 0.3f * jumpForce, 0f));
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.1f, 0.8f);
        }
        GameObject instantiatedBoom = Instantiate(propelBoom);
        instantiatedBoom.transform.position = this.transform.position;
        Destroy(instantiatedBoom, 2f);
        Invoke("BoomAgain", 7f);
    }

    public void BoomAgain()
    {
        GameObject instantiatedBoom = Instantiate(propelBoom);
        instantiatedBoom.transform.position = this.transform.position;
        Destroy(instantiatedBoom, 2f);
    }

    private void IncrementBonusCount()
    {
        bonusCount.text = bonusGathered + "";
    }
    
    private void BonusAnimation()
    {
        bonusAnimation.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bonus")
        {
            bonusGathered++;
            if (victory != null)
            {
                victory.slimesCollected = bonusGathered;
            }
            //We destroy the object and add points in the score
            Destroy(other.gameObject,0.0f);
            playerScore.SetValue(playerScore.value + 100);
            Invoke("BonusAnimation", 0.3f);
            Invoke("IncrementBonusCount", 0.5f);
            if(emotions)
                emotions.Bonus();
            shoutHandler.PlayAudio(true);

        }
        else if(other.tag == "Respawn")
        {
            Restart();
            /*//We respawn the player at the origin
            transform.position = respawnPosition;
            player.velocity = Vector3.zero;
            //If the player respawns he died, so he loses 50 points
            playerScore.SetValue(playerScore.value -50);*/

        }
        else if (other.tag == "Propulser")
        {
            //Upward propulsion
            if(emotions)
                emotions.PropellUp();
            shoutHandler.PlayAudio(false);

            player.AddForce(new Vector3(0f,1f*jumpForce,0f));
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0.1f, 0.8f);
            }
         }
        else if (other.tag == "Finish")
        {
            //If the player goes through the gate, Victory
            // Invoke("Victory", 5f);
            if (!finishedLevel)
            {
                shoutHandler.PlayAudio(true);

                //if currentLevel is positive it means we are not in level selection mode, so we can update it
                if (currentLevel.value >= 0)
                {
                    currentLevel.value++;
                }
                playerScore.SetValue(playerScore.value + 300);
                finishedLevel = true;
                Invoke("PropellUp", 1.5f);
                victory = Instantiate(victoryText).GetComponent<outroAnimationScript>();
                victory.slimesCollected=bonusGathered;
                victory.totalSlimes = CrossLevelInfo.maxSlimes;
                finalTime = CrossLevelInfo.time - handler.time.value;
                victory.time = finalTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the player does not control, it means the level is starting
        if (!hasControls)
        {
            if (emotions)
                emotions.NormalMood();
            //We enable controls and start timer
            hasControls = true;
            if(handler)
                handler.startRunning();
            //CameraShaker.GetInstance("MainCamera").ShakeOnce(Mathf.Min(Mathf.Abs(lastVelocity.y) / 7f, 5f), 3f, 0.1f, 0.1f);
            if (bumpSource != null)
            {
                bumpSource.pitch = Mathf.Pow(2, (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) / 100f));
                bumpSource.volume = (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) / 50f) * baseVolume;
                bumpSource.Play();
            }
            ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
            fallSparks.Emit(emitOverride, 10 + (int)Mathf.Min(Mathf.Abs(lastVelocity.y) / 5f, 20f));
        }
        /*Falldown particles*/
        if(Mathf.Abs(lastVelocity.y) > 1f)
        {

            //CameraShaker.GetInstance("MainCamera").ShakeOnce(Mathf.Min(Mathf.Abs(lastVelocity.y) / 7f, 5f), 3f, 0.1f, 0.1f);
            if (bumpSource != null)
            {
                bumpSource.pitch = Mathf.Pow(2, (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) / 100f));
                bumpSource.volume = (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) /50f) * baseVolume;
                bumpSource.Play();
            }

 
            if (Mathf.Abs(lastVelocity.y) > 5f)
            {
                if(emotions)
                    emotions.Shock();
                shoutHandler.PlayAudio(false);

                ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
                fallSparks.Emit(emitOverride, 10 + (int)Mathf.Min(Mathf.Abs(lastVelocity.y)/5f,20f));
                if (Gamepad.current != null)
                {
                    Gamepad.current.SetMotorSpeeds(1f, 1f);
                }


            }
        }
        
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }

    }


    /* ---------------------- DISABLED -------------------------*/
    private void checkGravity()
    {
        //We launch a ray downward to check if we are close to the ground
        RaycastHit hit;
        //Physics.gravity represents the direction to the ground
        if (Physics.Raycast(transform.position, Physics.gravity, out hit, 100f))
        {
            //If the collider hits
            if (hit.collider.tag == "Gravity")
            {
                /*We change the gravity*/
                float maxValue = -(Mathf.Abs(hit.normal.x) + Mathf.Abs(hit.normal.y) + Mathf.Abs(hit.normal.z)) / 9.8f;
                Physics.gravity = new Vector3(hit.normal.x * maxValue, hit.normal.y * maxValue, hit.normal.z * maxValue);
            }

        }

    }


}
