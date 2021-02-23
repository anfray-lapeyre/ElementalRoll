using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorHelper;
using static AccelerationHelper;
using static LevelLoader;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using TMPro;
using EZCameraShake;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PlayerController : Observer
{
    private bool loading = false;

    public GameObject playerCamera;
    private Rigidbody player;
    public ParticleSystem fallSparks;
    public ParticleSystem speedSparks;
    public ParticleSystem grindSparks;
    [HideInInspector]
    public float moveVertical = 0f;
    [HideInInspector]
    public float moveHorizontal = 0f;
    private Vector3 respawnPosition;
    private AudioSource bumpSource;

    private Vector3 lastVelocity;

    private bool mustPropellUp = false;
    private bool finishedLevel = false;
    [HideInInspector]
    public bool hasControls = false;

    private TextMeshProUGUI bonusCount;
    public UIParticleSystem bonusAnimation;
    private int bonusGathered = 0;

    //public GameObject LevelLoader; //Base
    //private LevelLoader _levelLoader; //Instantiated child

    public GameObject PauseMenu;
    private bool inOptions=false;

    private EmotionHandlerScript emotions;

    public GameObject victoryText;

    public GameObject Boom;
    public GameObject propelBoom;
    private int invokingTime;

    private SphereCollider playerCollider;

    private Transform StartParent;

    [HideInInspector]
    public bool hasPlayerTouched = false;

    private InputHandler inputHandler;

    public Cinemachine.CinemachineBrain cinemachineBrain;

    public Cinemachine.CinemachineVirtualCamera normalView;
    public Cinemachine.CinemachineVirtualCamera topView;

    public Camera faceCamera;


    //This will contain an object that will enable our player to look somewhere in particular
    public Transform[] eyes;

    private ShoutHandlerScript shoutHandler;
    private bool isLookingDown = false;
    //Rewinding Time for Tim
    //Shrinking for Tracy
    private bool isPowerInUse = false;

    [HideInInspector]
    public int playerNb;
    [HideInInspector]
    public int playerCount;
    private int characterNb;
    public Vector3 defaultGravity = new Vector3(0, -9.87f, 0);

    /* ------------------------------------------------------------------------------------------- INITIALIZATION ------------------------------------------------------------------------------*/

    public void updatePlayerScreenHandling(int _playerNb, int _maxPlayers)
    {
        playerNb = _playerNb;
        playerCount = _maxPlayers;
       
        normalView.name = "MainCameraRPG" + playerNb;
        topView.name = "TopView" + playerNb;
        //CameraShaker.ChangeNameOfInstance(faceCamera.name, "FaceCamera"+playerNb);


        cinemachineBrain.gameObject.layer = 15 + playerNb;
        normalView.gameObject.layer = 15 + playerNb;
        topView.gameObject.layer = 15 + playerNb;
        cinemachineBrain.GetComponent<Camera>().cullingMask |= (int)Mathf.Pow(2, (15 + playerNb));

        Debug.Log("Player nb : " + playerNb + " & maxPlayers : " + playerCount + " screen Placement : " + (playerNb % 2) / 2f + " screenSize : ");
        this.transform.parent.parent.GetComponent<PlayerInstanceHandler>().UpdateUIPlacement(playerNb);

        //We need to set the right render texture on the face camera to not override the others 
        faceCamera.targetTexture = this.transform.parent.parent.GetComponent<PlayerInstanceHandler>().GetCurrentRenderTexture();
        if(emotions == null)
        {
            emotions = this.GetComponent<EmotionHandlerScript>();
        }
        emotions.playerNb = playerNb;
        //emotions.NormalMood();
        Debug.Log("PlayerController : " + playerNb + " & EmotionNb : " + emotions.playerNb);
    }



    private void Start()
    {
        //This is an emergency thing. It should not ever trigger except in testing environment
        if (ActualSave.actualSave == null)
        {
            ActualSave.actualSave = new SaveFileInfo();
        }

        inputHandler = this.transform.parent.parent.GetComponent<InputHandler>();
            //GameObject.FindGameObjectsWithTag("PersistentObject")[0];
        inputHandler.addObserver(this);
        StartParent = this.transform.parent;
        shoutHandler = playerCamera.GetComponentInChildren<ShoutHandlerScript>();
        playerCollider = this.GetComponent<SphereCollider>();
        emotions = this.GetComponent<EmotionHandlerScript>();
        // We get the player's rigidbody's component
        player = GetComponent<Rigidbody>();
        ActualSave.actualSave.stats[playerNb].playerRotationSpeed=0f;
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

        bonusCount = GameObject.FindGameObjectsWithTag("BonusCount")[0].GetComponent<TextMeshProUGUI>();

        characterNb = ActualSave.actualSave.stats[playerNb].activePlayer;
        /*
        GameObject levelLoader = Instantiate(LevelLoader);
        _levelLoader = levelLoader.GetComponent<LevelLoader>();*/


        //updatePlayerScreenHandling(playerNb, playerCount);
        defaultGravity = new Vector3(0, -9.87f, 0);

        if (characterNb == 2)// If earth
        {
            //We instantiate the Grappling Gun
            GameObject instantiatedEarthBoom = Instantiate(Boom, transform);
            GrapplingGunScript gun = instantiatedEarthBoom.GetComponent<GrapplingGunScript>();
            gun.gunTip = transform;
            gun.varCamera = grindSparks.transform.parent;
            gun.player = transform;
        }
    }






    /*---------------------------------------------------------------------------------------------- INPUTS ---------------------------------------------------------------------------*/



    //Handle controller system notifications from our persistant subject
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
            case "SecondSpecialActionCommand":
                OnSecondSpecialAction(((SecondSpecialActionCommand)notifiedEvent).isPressed());
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

    //----------------- Left stick, arrows or ZQSD --------------
    public void OnMove(Vector2 value)
    {

        if (!inOptions)
        {
            moveHorizontal = value.x;
            moveVertical = value.y;
        }
    }


   
    // ------------------  Start or Escape -----------
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
    //Enables the inputs to be taken into account again
    public void QuitOptions()
    {
        inOptions = false;
    }

   //-------------------- B Button or Return ---------------------
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

    public void Restart()
    {
        if (!finishedLevel && !loading)
        {
            //_levelLoader.LoadNextLevel(Mathf.Abs(ActualSave.actualSave.stats[playerNb].currentLevel), true);//In case the value is negative while restarting, it means we are in level selection mode, we can put any positive value here
            loading = true;
        }

    }


    //------------------- On Maj or Left/Right Trigger -----------------------
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

    //--------------------- On Space or A Button --------------------------------
    public void OnSpecialAction(bool value)
    {
        if (value && !inOptions)
        {
            if (hasControls)
            {
                if (ActualSave.actualSave.stats[playerNb].powerTime[characterNb][0] >= ActualSave.actualSave.stats[playerNb].maxPowerTime[characterNb][0])
                {
                    hasPlayerTouched = true;
                    switch (characterNb)
                    {
                        case 1: //Ice
                                //Ice platform generation
                            ActualSave.actualSave.stats[playerNb].powerTime[characterNb][0] = 0f;

                            //We make sure that when the power gauge reaches 0, the player cannot use the power anymore
                            shoutHandler.PlayAudio(true);
                            invokingTime = 15;

                            InvokeIce();


                            
                            break;
                        case 2:
                            //Earth
                            //No idea
                            shoutHandler.PlayAudio(true);
                            emotions.PowerUp();
                            ActualSave.actualSave.stats[playerNb].powerTime[characterNb][0] = 0f;

                            //player.AddForce(-player.velocity);

                            player.isKinematic = true;
                            GameObject instantiatedEarthBoom = Instantiate(propelBoom, fallSparks.transform);
                            Destroy(instantiatedEarthBoom, 2f);
                            Invoke("EarthMovesAgain", 1.5f);
                            break;
                        case 3:
                            //Death 
                            //Can rewing time thrice per level

                            //We remove a third of the power

                            
                            ActualSave.actualSave.stats[playerNb].powerTime[characterNb][0] = 0f;

                            emotions.PowerUp();
                            shoutHandler.PlayAudio(true);
                            //everything is handled inside after we place it
                            GameObject instantiatedDeathBoom = Instantiate(Boom);
                            instantiatedDeathBoom.transform.position = this.transform.position;
                            rumble(0.1f, 0.8f);
                           
                            break;
                        default: //Fire
                                 //Upward propulsion
                            ActualSave.actualSave.stats[playerNb].powerTime[characterNb][0] = 0f;

                            emotions.PowerUp();
                            shoutHandler.PlayAudio(true);
                            
                            
                            player.AddForce(grindSparks.transform.parent.forward * Character.getCharacterInfo(ActualSave.actualSave.stats[playerNb].activePlayer).jumpForce*0.5f);
                            rumble(0.1f, 0.8f);
                            GameObject instantiatedBoom = Instantiate(Boom);
                            instantiatedBoom.transform.position = this.transform.position;
                            Destroy(instantiatedBoom, 2f);
                            break;
                    }
                }
            }
        }
        else if(!value && !inOptions && isPowerInUse)
        {
            DeathStopRewind();
        }
    }


    //--------------------- On Space or A Button --------------------------------
    public void OnSecondSpecialAction(bool value)
    {
        if (value && !inOptions)
        {
            if (hasControls)
            {
                if (ActualSave.actualSave.stats[playerNb].powerTime[characterNb][1] >= ActualSave.actualSave.stats[playerNb].maxPowerTime[characterNb][1])
                {
                    hasPlayerTouched = true;
                    switch (characterNb)
                    {
                        case 1: //Ice
                                //Ice platform generation
                            ActualSave.actualSave.stats[playerNb].powerTime[characterNb][1] = 0f;

                            emotions.PowerUp();
                            shoutHandler.PlayAudio(true);
                            gameObject.LeanScale(Vector3.one * 0.5f, 0.2f).setEaseInElastic();
                            isPowerInUse = true;
                            rumble(0.1f, 0.8f);
                            Invoke("RegainSize", 5f);
                            break;
                        case 2:
                            //Earth
                            //No idea
                            shoutHandler.PlayAudio(true);
                            emotions.PowerUp();
                            ActualSave.actualSave.stats[playerNb].powerTime[characterNb][1] = 0f;

                            //player.AddForce(-player.velocity);
                            isPowerInUse = true;
                            GrapplingGunScript gun = this.GetComponentInChildren<GrapplingGunScript>();
                            gun.StartGrapple();

                            break;
                        case 3:
                            //Death 
                            //Can rewing time thrice per level

                            //We remove a third of the power

                            ActualSave.actualSave.stats[playerNb].powerTime[characterNb][1] = 0f;

                            //We make sure that when the power gauge reaches 0, the player cannot use the power anymore
                            shoutHandler.PlayAudio(true);

                            DeathRewind();
                            Invoke("DeathStopRewind", 3f);

                            break;
                        default: //Fire
                                 //Upward propulsion
                            ActualSave.actualSave.stats[playerNb].powerTime[characterNb][1] = 0f;

                            emotions.PowerUp();
                            shoutHandler.PlayAudio(true);
                            if (player.velocity.y < 0)
                            {
                                player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
                            }
                            player.AddForce(new Vector3(0f, 0.3f * Character.getCharacterInfo(ActualSave.actualSave.stats[playerNb].activePlayer).jumpForce, 0f));
                            rumble(0.1f, 0.8f);
                            GameObject instantiatedBoom = Instantiate(Boom);
                            instantiatedBoom.transform.position = this.transform.position;
                            Destroy(instantiatedBoom, 2f);
                            break;
                    }
                }
            }
        }
        else if (!value && !inOptions && isPowerInUse)
        {
            if(characterNb == 2) // Earth
            {
                isPowerInUse = false;
                GrapplingGunScript gun = this.GetComponentInChildren<GrapplingGunScript>();
                gun.StopGrapple();
                player.velocity = player.velocity / 4f;
            }
            else if (characterNb == 3) // Death
            {
                DeathStopRewind();
            }
        }
    }

    public void SetPowerInUse( bool p)
    {
        isPowerInUse = p;
    }

    public void RegainSize()
    {
        isPowerInUse = false;
        gameObject.LeanScale(Vector3.one, 0.2f).setEaseInElastic();
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



    //Earth power related
    public void EarthMovesAgain()
    {
        player.isKinematic = false;

        player.AddForce(new Vector3(0f, -1f * Character.getCharacterInfo(ActualSave.actualSave.stats[playerNb].activePlayer).jumpForce, 0f));
        GameObject instantiatedBoom = Instantiate(propelBoom);
        instantiatedBoom.transform.position = this.transform.position;
        Destroy(instantiatedBoom, 2f);
    }


    //Reinstaure Gravity
    public void RestoreGravity(){
        Physics.gravity = defaultGravity;
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
        fallSparks.Emit(emitOverride, 20);
        
    }

    //Death power related 1/2
    public void DeathRewind()
    {
        isPowerInUse = true;
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
            AudioSource source = GameObject.FindGameObjectWithTag("PersistentObject").GetComponent<AudioSource>();
            AudioSource soundSource = shoutHandler.GetComponent<AudioSource>();
            float characterPitch = new Character().Death().pitch;
            LeanTween.value(1f,-0.9f,0.3f).setOnUpdate((float val) =>
            {
               source.pitch = val;
                soundSource.pitch = val * characterPitch;
                
            });





        }
    }


    //Death power related 2/2

    public void DeathStopRewind()
    {
        if (isPowerInUse)
        {
            isPowerInUse = false;
            TimeBody[] replays = Resources.FindObjectsOfTypeAll(typeof(TimeBody)) as TimeBody[];
            for (int i = 0; i < replays.Length; i++)
            {
                replays[i].StopRewind();
            }
            //This makes it possible again for Tim to use its powers in the old version
            //powerTime = Mathf.Max(ActualSave.actualSave.stats[playerNb].powerGauge - 0.01f, 0.5f);
            Volume[] volume = Resources.FindObjectsOfTypeAll(typeof(Volume)) as Volume[];
            if (volume.Length > 0 && volume[0].profile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.saturation.overrideState = true;
                LeanTween.value(-70f, 0f, 0.3f).setOnUpdate((float val) =>
                {
                    colorAdjustments.saturation.value = val;
                });
                AudioSource source = GameObject.FindGameObjectWithTag("PersistentObject").GetComponent<AudioSource>();
                AudioSource soundSource = shoutHandler.GetComponent<AudioSource>();
                float characterPitch = new Character().Death().pitch;
                LeanTween.value(-0.9f, 1f, 0.3f).setOnUpdate((float val) =>
                {
                    source.pitch = val;
                    soundSource.pitch = val * characterPitch;

                });
            }
            //Debug.Log("disableEye : " + (int)ActualSave.actualSave.stats[playerNb].powerGauge+1);
            //emotions.disableEye((int)ActualSave.actualSave.stats[playerNb].powerGauge+1);

        }
    }



  /* ------------------------------------------------------------------------------------------ FIXED UPDATE --------------------------------------------------------------------------*/

    private void FixedUpdate()
    {
        
        if (!finishedLevel)
        {
            handlePlayerMovementAndPowerGauge();
        }
        else
        {
            handlePlayerFinalBehaviour();
        }

    }



    /* --------------------------------------------------------------------------------------  UPDATE ----------------------------------------------------------------------------------------------*/


    private void Update()
    {
        grindParticles();

        speedParticles();


        isCharacterAffraidOfCliff();

    }



    /*----------------------------------------------------------------------------------------- TRIGGER HANDLING --------------------------------------------------------------------------------*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bonus")
        {
            bonusGathered++;
            /*if (victory != null)
            {
                victory.slimesCollected = bonusGathered;
            }*/
            //We destroy the object and add points in the score
            Destroy(other.gameObject, 0.0f);
            Invoke("BonusAnimation", 0.3f);
            Invoke("IncrementBonusCount", 0.5f);
            if (emotions)
                emotions.Bonus();
            shoutHandler.PlayAudio(true);

        }
        else if (other.tag == "Respawn")
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
            if (emotions)
                emotions.PropellUp();
            shoutHandler.PlayAudio(false);

            player.AddForce(new Vector3(0f, 1f * Character.getCharacterInfo(ActualSave.actualSave.stats[playerNb].activePlayer).jumpForce, 0f));
            rumble(0.1f, 0.8f);
        }
        else if (other.tag == "Finish")
        {
            //If the player goes through the gate, Victory
            // Invoke("Victory", 5f);
            if (!finishedLevel)
            {
                shoutHandler.PlayAudio(true);
                finishedLevel = true;
                Invoke("PropellUp", 1.5f);

                /*This needs to be contianed elsewhere

                //if currentLevel is positive it means we are not in level selection mode, so we can update it
                if (ActualSave.actualSave.stats[playerNb].currentLevel >= 0)
                {
                    ActualSave.actualSave.stats[playerNb].currentLevel++;
                }
                
                victory = Instantiate(victoryText).GetComponent<outroAnimationScript>();
                victory.slimesCollected = bonusGathered;
                victory.totalSlimes = CrossLevelInfo.maxSlimes;
                finalTime = CrossLevelInfo.time - handler.time.value;
                victory.time = finalTime;
                */
            }
        }
    }




    /*---------------------------------------------------------------------------------------------------------- COLLISION HANDLING -----------------------------------------------------------------------------------------------------*/


    private void OnCollisionEnter(Collision collision)
    {
        //If the player does not control, it means the level is starting
        if (!hasControls)
        {
            if (emotions)
                emotions.NormalMood();
            //We enable controls and start timer
            hasControls = true;

            CameraShaker shaker = normalView.GetComponent<CameraShaker>();
            if(shaker != null)
                shaker.ShakeOnce(Mathf.Min(Mathf.Abs(lastVelocity.y) / 7f, 5f), 3f, 0.1f, 0.1f);
            if (bumpSource != null)
            {
                bumpSource.pitch = Mathf.Pow(2, (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) / 100f));
                bumpSource.volume = (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) / 50f) * Character.getCharacterInfo(ActualSave.actualSave.stats[playerNb].activePlayer).baseVolume;
                bumpSource.Play();
            }
            ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
            fallSparks.Emit(emitOverride, 10 + (int)Mathf.Min(Mathf.Abs(lastVelocity.y) / 5f, 20f));


          
        }
        /*Falldown particles*/
        if (Mathf.Abs(lastVelocity.y) > 1f)
        {
            CameraShaker shaker = normalView.GetComponent<CameraShaker>();

            if (shaker != null)
                shaker.ShakeOnce(Mathf.Min(Mathf.Abs(lastVelocity.y) / 7f, 5f), 3f, 0.1f, 0.1f);
            if (bumpSource != null)
            {
                bumpSource.pitch = Mathf.Pow(2, (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) / 100f));
                bumpSource.volume = (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) / 50f) * Character.getCharacterInfo(ActualSave.actualSave.stats[playerNb].activePlayer).baseVolume;
                bumpSource.Play();
            }


            if (Mathf.Abs(lastVelocity.y) > 5f)
            {
                if (emotions)
                    emotions.Shock();
                shoutHandler.PlayAudio(false);

                ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
                fallSparks.Emit(emitOverride, 10 + (int)Mathf.Min(Mathf.Abs(lastVelocity.y) / 5f, 20f));
                rumble(0.2f, 0.2f);

            }
        }

        HorizontalBreakableScript h = collision.collider.GetComponent<HorizontalBreakableScript>();
        if (h && new Vector2(lastVelocity.x,lastVelocity.z).magnitude >= 18f)
        {
            h.Break();
        }
        VerticalBreakableScript v = collision.collider.GetComponent<VerticalBreakableScript>();
        if (v && Mathf.Abs(lastVelocity.y)>=15f)
        {
            v.Break();
        }
        rumble(0f, 0f);

    }







    /****************************************************************************** MOVEMENTS *****************************************************************/

    private void handlePlayerMovementAndPowerGauge()
    {
        //We increment time for all the powers and cap theme to maxPower with just a little nudge on top
        //That way, we are sure that the float comparisons with maxPower on other parts of the code will work 100%
        if (!isPowerInUse)
        {
            float nudge = 0.0001f;
            for (int i = 0; i < 4; i++)
            {
                ActualSave.actualSave.stats[playerNb].powerTime[i][0] = Mathf.Min(ActualSave.actualSave.stats[playerNb].powerTime[i][0] + Time.fixedDeltaTime, ActualSave.actualSave.stats[playerNb].maxPowerTime[i][0] + nudge);
                ActualSave.actualSave.stats[playerNb].powerTime[i][1] = Mathf.Min(ActualSave.actualSave.stats[playerNb].powerTime[i][1] + Time.fixedDeltaTime, ActualSave.actualSave.stats[playerNb].maxPowerTime[i][1] + nudge);
            }
        }
        //We handle the player's movement
        if (hasControls)
        {
            handleMovement();
            //We update the speed base on the sum of the player's velocity vector
            if (hasPlayerTouched)
            {
                ActualSave.actualSave.stats[playerNb].playerSpeed = (player.velocity.magnitude * 2f);
                ActualSave.actualSave.stats[playerNb].playerRotationSpeed=(Mathf.Abs(player.angularVelocity.x) + Mathf.Abs(player.angularVelocity.y) + Mathf.Abs(player.angularVelocity.z));
                //checkGravity();
                lastVelocity = player.velocity;
            }
            else
            {
                player.velocity = player.velocity * 0.2f;
            }
        }
    }




    private void handleMovement()
    {


        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //This part is to immobilize the player at the start of a game before he touches the controller
        if (!hasPlayerTouched && movement.magnitude != 0f)
        {
            //Now the player can move
            hasPlayerTouched = true;
        }

        //movement 
        movement = applySpeedCurve(player.velocity, movement, cinemachineBrain.transform.eulerAngles.y, Character.getCharacterInfo(ActualSave.actualSave.stats[playerNb].activePlayer));
        movement += CheckIfOnPlatform();


        //We add it to the player's rigidbody as a Force
        player.AddForce(movement);

    }








    /************************************************************************************* END OF LEVEL BEHAVIOUR **************************************************************************/
    //For all save and loading behaviour, see PlayerControllerOld, this has nothing to do here

    //Following behaviours will only be active when the end of level behaviour container in PlayerControllerOld (loading, saving, ischaracterunlocked...) is integrated elsewhere
    //The player propells up after accessing a gate

    private void PropellUp()
    {
        mustPropellUp = true;
        emotions.PowerUp();
        if (player.velocity.y < 0)
        {
            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
        }
        player.AddForce(new Vector3(0f, 0.3f * Character.getCharacterInfo(ActualSave.actualSave.stats[playerNb].activePlayer).jumpForce, 0f));
        rumble(1f, 0.8f);

        GameObject instantiatedBoom = Instantiate(propelBoom);
        instantiatedBoom.transform.position = this.transform.position;
        Destroy(instantiatedBoom, 2f);
        Invoke("BoomAgain", 7f);
    }


    private void handlePlayerFinalBehaviour()
    {
        if (!mustPropellUp)
        {
            //We decrease the movement of the player
            player.velocity = new Vector3(player.velocity.x / (1 + 0.1f / player.velocity.magnitude), player.velocity.y / 1.2f, player.velocity.z / (1 + 0.1f / player.velocity.magnitude));
        }
        else
        {
            //Then Propell him up
            player.AddForce(Vector3.up * 15f);
            rumble(0.2f, 0.2f);


        }
    }

    public void BoomAgain()
    {
        GameObject instantiatedBoom = Instantiate(propelBoom);
        instantiatedBoom.transform.position = this.transform.position;
        Destroy(instantiatedBoom, 2f);
    }




    // --------------------- BONUS BEHAVIOUR ---------------------*/

    private void IncrementBonusCount()
    {
        bonusCount.text = bonusGathered + "";
    }

    private void BonusAnimation()
    {
        bonusAnimation.Play();
    }







    /* ---------------- CHECKS IF THE PLAYER IS ON A MOVING/ROTATING PLATFOM -------------------------------------*/
    //If so, it changes the referential to the right this / Returns the value to add to movement
    public Vector3 CheckIfOnPlatform()
    {
        //We will handle here the player's physical referential (if you're on a moving platform, you move with it)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerCollider.bounds.extents.y + 0.1f))
        {
            GameObject RaycastReturn = hit.collider.gameObject;
            //If it's a rotating platform, we make the player dependant of it
            if (RaycastReturn.GetComponent<simpleRotatingPlatformScript>())
            {
                //Make the object parent here ! 
                if (this.transform.parent != RaycastReturn.transform)
                    this.transform.SetParent(RaycastReturn.transform, true);
            }
            //If it's not, we extract the player
            else
            {
                if (this.transform.parent != StartParent)
                    this.transform.SetParent(StartParent, true);
            }

            //If a moving platform, we add its movement to the player
            if (RaycastReturn.GetComponent<Rigidbody>())
            {
                return RaycastReturn.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime; //simpleMovingPlatformScript handling
            }

        }
        else
        {
            //In case previous screenings weren't sufficient, if the player isn't anymore on a plaftorm, we extract it
            if (this.transform.parent != StartParent)
                this.transform.SetParent(StartParent, true);
        }
        return Vector3.zero;
    }


    private void grindParticles()
    {
        //Handling grind sparks. If the player is too fast and touching the ground
        RaycastHit hit;
        var grindEmission = grindSparks.emission;
        Vector3 direction = Physics.gravity;
        bool isEmittingGrindSparks = false;
        if (Physics.Raycast(transform.position, direction, out hit, 0.67f) && ActualSave.actualSave.stats[playerNb].playerSpeed > 50f)
        {
            isEmittingGrindSparks = true;
            grindEmission.rateOverTime = Mathf.Min(Mathf.Max((ActualSave.actualSave.stats[playerNb].playerSpeed - 50f), 0f) / 2f, 20f);

            rumble(1f, 1f);
          

        }

        grindEmission.enabled = isEmittingGrindSparks;
    }

    private void speedParticles()
    {
        //Handling speed particles
        bool isEmittingSpeedSparks = false;
        ParticleSystem.EmissionModule emission = speedSparks.emission;
        if (ActualSave.actualSave.stats[playerNb].playerSpeed >= 60f)
        {
            CameraShaker shaker = normalView.GetComponent<CameraShaker>();

            if (shaker != null)
                shaker.ShakeOnce(Mathf.Min((ActualSave.actualSave.stats[playerNb].playerSpeed - 40f) / 100f, 0.3f), 4f, 0.1f, 0.1f);
            isEmittingSpeedSparks = true;
            rumble(0.2f, 0.2f);

            if (ActualSave.actualSave.stats[playerNb].playerSpeed > 80f)
            {
                if (emotions)
                    emotions.Roll();

            }
            else
            {
                if (emotions)
                    emotions.StopRolling();
            }
        }
        else
        {
            rumble(0f, 0f);

        }
        emission.rateOverTime = Mathf.Min(Mathf.Max((ActualSave.actualSave.stats[playerNb].playerSpeed - 40f), 0f) / 5f, 15f);
        emission.enabled = isEmittingSpeedSparks;
    }

    //If a character is at the border of a cliff, trigger an afraid expression
    private void isCharacterAffraidOfCliff()
    {

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

    //Checks if a character is at the border of a cliff
    private bool isPlayerOnBorder()
    {
        RaycastHit borderhit;
        float playerSize = playerCollider.bounds.extents.x / 1.2f;

        bool result = !Physics.Raycast(transform.position + playerSize * fallSparks.transform.right, Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        result = result || !Physics.Raycast(transform.position + playerSize * -fallSparks.transform.right, Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        result = result || !Physics.Raycast(transform.position + playerSize * (-fallSparks.transform.right + fallSparks.transform.forward), Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        result = result || !Physics.Raycast(transform.position + playerSize * 0.8f * (fallSparks.transform.right + fallSparks.transform.forward), Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        result = result || !Physics.Raycast(transform.position + playerSize * fallSparks.transform.forward, Vector3.down, out borderhit, playerCollider.bounds.extents.y + 2f);
        //We make sure there that the player gets worried ONLY if it is on the ground.
        result = result && Physics.Raycast(transform.position, Physics.gravity, out borderhit, playerCollider.bounds.extents.y + 0.1f);
        return result;
    }







    private void rumble(float low, float high)
    {
        /*
        Gamepad currentController = null;
        foreach(Gamepad g in Gamepad.all)
        {
            if (low > 0.01f && high > 0.01f)


            foreach(InputDevice d in inputHandler.GetComponent<PlayerInput>().user.pairedDevices)
            {
                if(g.deviceId == d.deviceId)
                {
                    currentController = g;
                }
            }
        }
        //inputHandler.GetComponent<PlayerInput>().user.pairedDevices[0].deviceId;
        //Gamepad currentController = Gamepad.all.FirstOrDefault(g => inputHandler.GetComponent<PlayerInput>().devices.Any(d => d.deviceId == g.deviceId));
        if(currentController != null)
            currentController.SetMotorSpeeds(low, high);
            */
    }




    override protected void OnDestroy()
    {
        base.OnDestroy();
        if (defaultGravity != Physics.gravity)
        {
            Physics.gravity = defaultGravity;
        }
    }




    /* -------------------------------------------------------------------------------------------- DEPRECATED ---------------------------------------------------------------------------------------------*/
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

}
