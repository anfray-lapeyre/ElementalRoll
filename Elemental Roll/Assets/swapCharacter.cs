using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapCharacter : Observer
{

    private InputHandler inputSubject;

    //All the components necessary to the player's good function
    public GameObject playerCamera;
    public ParticleSystem fallSparksFire;
    public ParticleSystem fallSparksIce;
    public ParticleSystem fallSparksEarth;
    public ParticleSystem fallSparksDeath;
    public ParticleSystem speedSparks;
    public ParticleSystem grindSparks;
    public Cinemachine.CinemachineBrain cinemachineBrain;
    public Cinemachine.CinemachineVirtualCamera TopView;
    public UIParticleSystem BubbleParticle;

    private Vector3 velocity;
    private Vector3 angularVelocity;


    public GameObject PlayerFire;
    public GameObject PlayerIce;
    public GameObject PlayerEarth;
    public GameObject PlayerDeath;


    public PlayerCapsuleFollow playerFollower;

    public int actualPlayer;
    public GameObject actualPlayerInstance;

    public GameObject playerIconInstance;

    public Material[] playerIcons;

    private int playerNb = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputSubject = this.GetComponentInParent<InputHandler>();
        inputSubject.addObserver(this);
        playerNb = actualPlayerInstance.GetComponent<PlayerController>().playerNb;
        //actualPlayerInstance.GetComponent<PlayerController>().updatePlayerScreenHandling(1, 1);
    }

    //Handle controller system notifications from our persistant subject
    override public void OnNotify(GameObject entity, object notifiedEvent)
    {
        switch (notifiedEvent.GetType().ToString())
        {


            case "SwapCharacterCommand":
                SwapPlayer(((SwapCharacterCommand)notifiedEvent).getMove());
                break;
            default:
                break;
        }
    }

    public void SwapPlayer(Vector2 value)
    {
        int wanted = 0;
        //Left = fire
        //Up = Ice
        //Right = Earth
        //Down = Death
        wanted = (value.x < -0.1f) ? 0 : (value.y > 0.1f)? 1 : (value.x > 0.1f) ? 2 : (value.y < -0.1f) ? 3 : -1;
        if (actualPlayer != wanted && wanted>=0)
        {

            playerNb = actualPlayerInstance.GetComponent<PlayerController>().playerNb;
            int playerCount = actualPlayerInstance.GetComponent<PlayerController>().playerCount;
            //EZCameraShake.CameraShaker.RemoveInstance("FaceCamera" + playerNb);
            //EZCameraShake.CameraShaker.RemoveInstance("FaceCamera");
            Vector3 position = actualPlayerInstance.transform.position;
            Rigidbody rigid = actualPlayerInstance.GetComponent<Rigidbody>();
            velocity = rigid.velocity;
            angularVelocity = rigid.angularVelocity;
            List<PointInTime> history = actualPlayerInstance.GetComponent<TimeBody>().GetPointsInTime();
            ActualSave.actualSave.stats[playerNb].activePlayer = wanted;

            switch (wanted)
            {
                case 0:
                    Destroy(actualPlayerInstance);
                    actualPlayerInstance = Instantiate(PlayerFire, this.transform);
                    SetSettingsForPlayer(actualPlayerInstance.GetComponent<PlayerController>(),wanted);
                    break;
                case 1:
                    Destroy(actualPlayerInstance);
                    actualPlayerInstance = Instantiate(PlayerIce, this.transform);
                    SetSettingsForPlayer(actualPlayerInstance.GetComponent<PlayerController>(), wanted);
                    break;
                case 2:
                    Destroy(actualPlayerInstance);
                    actualPlayerInstance = Instantiate(PlayerEarth, this.transform);
                    SetSettingsForPlayer(actualPlayerInstance.GetComponent<PlayerController>(), wanted);
                    break;
                case 3:
                    Destroy(actualPlayerInstance);
                    actualPlayerInstance = Instantiate(PlayerDeath, this.transform);
                    SetSettingsForPlayer(actualPlayerInstance.GetComponent<PlayerController>(), wanted);
                    break;
            }
            actualPlayer = wanted;
            //playerFollower.player = 
            actualPlayerInstance.transform.position = position;

            playerIconInstance.GetComponent<stayOnTopScript>().objectToFollow = actualPlayerInstance.transform;
            actualPlayerInstance.GetComponent<TimeBody>().SetPointsInTime(history);
            playerFollower.player = actualPlayerInstance;
            playerFollower.UpdateRigid();
            actualPlayerInstance.GetComponent<PlayerController>().updatePlayerScreenHandling(playerNb, playerCount);

        }
    }

    //Will set all the necessary informations for the playerController script, that is linked to each character
    //This will enable HotSwapping between 2 characters
    public void SetSettingsForPlayer(PlayerController target, int wanted)
    {
        target.hasControls = true;
        target.hasPlayerTouched = true;
        target.playerCamera = playerCamera;
        target.normalView = playerCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        target.speedSparks = speedSparks;
        target.grindSparks = grindSparks;
        target.cinemachineBrain = cinemachineBrain;
        target.topView = TopView;
        target.bonusAnimation = BubbleParticle;

        switch (wanted)
        {
            case 0:
                target.fallSparks = fallSparksFire;

                break;
            case 1:
                target.fallSparks = fallSparksIce;

                break;
            case 2:
                target.fallSparks = fallSparksEarth;

                break;
            case 3:
                target.fallSparks = fallSparksDeath;

                break;

        }
        //target.hasControls = true;

        target.GetComponent<Rigidbody>().velocity = velocity;
        target.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
    }

    public void StoreVelocity(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public int getPlayerNb()
    {
        return playerNb;
    }


}
