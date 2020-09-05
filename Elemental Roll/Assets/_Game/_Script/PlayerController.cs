using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorHelper;
using static LevelLoader;
using UnityEngine.InputSystem;
using TMPro;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
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

    private void Start()
    {
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


    }

    public void OnMove(InputValue value)
    {
        if (!inOptions)
        {
            moveHorizontal = value.Get<Vector2>().x;
            moveVertical = value.Get<Vector2>().y;
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

    public void OnRestart(InputValue value)
    {
        if (!inOptions)
        {
            if (hasControls)
                Restart();
        }
    }

    public void Restart()
    {
        if (!finishedLevel)
        {
            _levelLoader.LoadNextLevel(Mathf.Abs(currentLevel.value), true);//In case the value is negative while restarting, it means we are in level selection mode, we can put any positive value here
        }
            
    }

    public void OnPause(InputValue value)
    {
        if (!inOptions)
        {
            Instantiate(PauseMenu, this.transform);
            inOptions = true;
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

        //We rotate it in accordance to the camera, for the force to be applied in a logical manner
        movement = RotateY(movement, Mathf.Deg2Rad * playerCamera.transform.rotation.eulerAngles.y);

        //If the player is trying to go in a different direction than its actual movement, we intensify it to slow down easily
        movement = new Vector3(Mathf.Sign(player.velocity.x) == Mathf.Sign(movement.x) ? movement.x : movement.x * invertSpeedModifier, movement.y, Mathf.Sign(player.velocity.z) == Mathf.Sign(movement.z) ? movement.z : movement.z * invertSpeedModifier);

        //If the player is going fast enough, we amplify the speed in order to give a sensation of higher "horse power"
        float speedAmplifier = (Mathf.Abs(player.velocity.x) + Mathf.Abs(player.velocity.y) > 10f) ? Mathf.Min((Mathf.Abs(player.velocity.x) + Mathf.Abs(player.velocity.y))/10f,5f) : 1f;

        //We add it to the player's rigidbody as a Force
        player.AddForce(movement * speedModifier * speedAmplifier);

    }

    private void FixedUpdate()
    {
        if (!finishedLevel)
        {
            //We handle the player's movement
            if(hasControls)
                handleMovement();
            //We update the speed base on the sum of the player's velocity vector
            playerSpeed.SetValue((Mathf.Abs(player.velocity.x) + Mathf.Abs(player.velocity.y) + Mathf.Abs(player.velocity.z)) * 3f);
            playerRotationSpeed.SetValue(Mathf.Abs(player.angularVelocity.x) + Mathf.Abs(player.angularVelocity.y) + Mathf.Abs(player.angularVelocity.z));
            //checkGravity();
            lastVelocity = player.velocity;
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

        bool isEmittingSpeedSparks = false;
        var emission = speedSparks.emission;
        if (playerSpeed.value >= 40f)
        {
            CameraShaker.GetInstance("MainCamera").ShakeOnce(Mathf.Min((playerSpeed.value-40f)/100f,0.3f), 4f, 0.1f, 0.1f);
            isEmittingSpeedSparks = true;
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0.2f, 0.2f);
            }

            if (playerSpeed.value > 80f)
            {
                emotions.Roll();
            }
            else
            {
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

    }



    public void enableVictory()
    {
        if (finishedLevel)
        {
            
            SaveProgress();
            _levelLoader.ShowLoader();
            _levelLoader.LoadNextLevel(Mathf.Abs(currentLevel.value));//In case the value is negative while restarting, it means we are in level selection mode, we can put any positive value here
        }
    }
    
    private void SaveProgress()
    {
        if (currentLevel.value < 0)
        {
            int level = -currentLevel.value - 1;
            if (ActualSave.actualSave.levels[level].collectedSlime < bonusGathered)
                ActualSave.actualSave.levels[level].collectedSlime = bonusGathered;

        }
        else
        {

            ActualSave.actualSave.levels[currentLevel.value-1].beaten = true;
            if (ActualSave.actualSave.levels[currentLevel.value-1].collectedSlime < bonusGathered)
                ActualSave.actualSave.levels[currentLevel.value-1].collectedSlime = bonusGathered;
            if (LevelLoader)
            {
                LevelLoader.GetComponent<LevelLoader>().handleDifficultySaveData(currentLevel.value-1);

            }
            else
            {
                //In case there's an error and Level Loader is not spawned, we'll assume it's our fault and give max completion
                Debug.LogError("There has been an error, LevelLoader is not instantiated.");
                ActualSave.actualSave.levels[currentLevel.value].beatenInDifficultLife=true;
                ActualSave.actualSave.levels[currentLevel.value].beatenInEasyLife = true;
                ActualSave.actualSave.levels[currentLevel.value].beatenInNormalLife = true;
                ActualSave.actualSave.levels[currentLevel.value].beatinInDifficultTime = true;
                ActualSave.actualSave.levels[currentLevel.value].beatinInEasyTime = true;
                ActualSave.actualSave.levels[currentLevel.value].beatinInNormalTime = true;
            }
        }

        SaveSystem.SaveGame(ActualSave.actualSave, ActualSave.saveSlot);
    }

    private void PropellUp()
    {
        mustPropellUp = true;
    }

    private void IncrementBonusCount()
    {
        bonusGathered++;
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
            //We destroy the object and add points in the score
            Destroy(other.gameObject,0.0f);
            playerScore.SetValue(playerScore.value + 100);
            Invoke("BonusAnimation", 0.3f);
            Invoke("IncrementBonusCount", 0.5f);
            emotions.Bonus();
        }else if(other.tag == "Respawn")
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
            emotions.PropellUp();
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
                //if currentLevel is positive it means we are not in level selection mode, so we can update it
                if (currentLevel.value >= 0)
                {
                    currentLevel.value++;
                }
                playerScore.SetValue(playerScore.value + 300);
                finishedLevel = true;
                Invoke("PropellUp", 1.5f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the player does not control, it means the level is starting
        if (!hasControls)
        {
            //We enable controls and start timer
            hasControls = true;
            handler.startRunning();
        }
        /*Falldown particles*/
        if(Mathf.Abs(lastVelocity.y) > 1f)
        {

            CameraShaker.GetInstance("MainCamera").ShakeOnce(Mathf.Min(Mathf.Abs(lastVelocity.y) / 7f, 5f), 3f, 0.1f, 0.1f);
            if (bumpSource != null)
            {
                bumpSource.pitch = Mathf.Pow(2, (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) / 100f));
                bumpSource.volume = (Mathf.Clamp(Mathf.Abs(lastVelocity.y), 0, 100) /50f) * baseVolume;
                bumpSource.Play();
            }

 
            if (Mathf.Abs(lastVelocity.y) > 5f)
            {
                emotions.Shock();
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
