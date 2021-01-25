using UnityEngine;
using UnityEngine.Timeline;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;


public class spawnPlayerScript : Observer
{
    public GameObject firePrefab;
    public GameObject icePrefab;
    public GameObject earthPrefab;
    public GameObject deathPrefab;
    public GameObject titleLevelPrefab;
    private GameObject instantiated;
    public TimelineAsset timeline;
    public CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera virtualCamera;
    private float desiredAngle;
    private GameObject player;
    private Transform playerCamera;
    public FloatVariable timer;
    public bool isRestarting = false;
    private GameObject levelTitle;
    public Transform socle;
    public bool isPlaying = false;
    private GameObject persistantHandler;
    // Start is called before the first frame update
    private void Awake()
    {
        
        persistantHandler = GameObject.FindGameObjectsWithTag("PersistentObject")[0];
        persistantHandler.GetComponent<InputHandler>().addObserver(this);
        desiredAngle = socle.localRotation.eulerAngles.y;
        switch (ActualSave.actualSave.chosenPlayer)
        {
            
            case 1:
                instantiated= Instantiate(icePrefab, transform.position + Vector3.up*15f, Quaternion.identity);
                break;
            case 2:
                instantiated=Instantiate(earthPrefab, transform.position + Vector3.up*15f, Quaternion.identity);
                break;
            case 3:
                instantiated=Instantiate(deathPrefab, transform.position + Vector3.up*15f, Quaternion.identity);
                break;
            default:
                instantiated=Instantiate(firePrefab, transform.position+Vector3.up*15f, Quaternion.identity);
                break;
        }
        desiredAngle = socle.rotation.eulerAngles.y;
        playerCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.parent;
        //playerCamera.Rotate(Vector3.up, desiredAngle);
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        player.SetActive(false);
        player.GetComponent<Rigidbody>().isKinematic = true;
        if (CrossLevelInfo.mustPassIntro)
        {
            PlayableDirector director = cinemachineBrain.gameObject.GetComponent<PlayableDirector>();
            if (!director.playableGraph.IsValid())
            {
                director.Play();
            }
            director.playableGraph.GetRootPlayable(0).SetSpeed(100);
            isPlaying = true;
            Invoke("DisableYourself", 0.1f);
        }

        SetTime();
        levelTitle = Instantiate(titleLevelPrefab);
    }

    override public void OnNotify(GameObject entity, object notifiedEvent)
    {
        switch (notifiedEvent.GetType().ToString())
        {
            case "MoveCommand":
                //OnMove(((MoveCommand)notifiedEvent).getMove());
                break;
            case "SpellCommand":
                OnSpecialAction(((SpellCommand)notifiedEvent).isPressed());
                break;
            case "RestartCommand":
                OnRestart(((RestartCommand)notifiedEvent).isPressed());
                break;
            case "PauseCommand":
                //OnPause(((PauseCommand)notifiedEvent).isPressed());
                break;
            case "EagleViewCommand":
               // OnPause(((EagleViewCommand)notifiedEvent).isPressed());
                break;
            case "TopViewCommand":
                //OnTopView(((TopViewCommand)notifiedEvent).isPressed());
                break;
            default:
                break;
        }
    }

    public void ActivatePlayer()
    {
        desiredAngle = socle.rotation.eulerAngles.y;

        playerCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.parent;
        //playerCamera.Rotate(Vector3.up, desiredAngle - 260.4645f);
        player.transform.parent.parent.Rotate(Vector3.up, desiredAngle);
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.SetActive(true);
        player.GetComponent<Rigidbody>().AddForce(-Vector3.up * 30f);
        virtualCamera.LookAt = player.transform;
        virtualCamera.Follow = playerCamera;
    }

    public void activateUI()
    {
        playerCamera.gameObject.GetComponent<UIFader>().FadeIn();
    }

    public void playerIsNotPlaying()
    {
        isPlaying = false;
    }

    public void deactivateUI()
    {
        playerCamera.gameObject.GetComponent<UIFader>().FadeOut();
    }

    public void disableCinemaBrain()
    {
        //cinemachineBrain.gameObject.SetActive(false);
        cinemachineBrain.gameObject.GetComponent<Camera>().enabled=false;
        
    }

    public void LoadNextLevel()
    {
        player.GetComponent<PlayerController>().enableVictory();
        
    }

    public void Replay()
    {
        TimeBody replay = player.GetComponent<TimeBody>();
        if (replay)
        {
            replay.StartRewind();
        }
        else
        {
            LoadNextLevel();
        }
    }

    public void SetTime()
    {
        if(CrossLevelInfo.time > 0 )
            timer.value = CrossLevelInfo.time+0.0f;
        else
        {
            timer.value = 60f;
        }
        if (levelTitle)
            Destroy(levelTitle, 0f);
    }


    private void FixedUpdate()
    {
        if (timer.value <= 0 && !isRestarting)
        {
            player.GetComponent<PlayerController>().Restart();
            isRestarting = true;
        }
    }

    public void OnRestart(bool input)
    {
        if (!isPlaying && input)
        {

            LoadNextLevel();
            PlayableDirector director = cinemachineBrain.gameObject.GetComponent<PlayableDirector>();
            if (!director.playableGraph.IsValid())
            {
                director.Play();
            }
            director.playableGraph.GetRootPlayable(0).SetSpeed(100);
            isPlaying = true;
            Invoke("DisableYourself", 0.1f);
        }
    }


    public void OnSpecialAction(bool input)
    {
        if(!isPlaying)
            OnRestart(input);
    }


    public void DisableYourself()
    {
        this.enabled = false;
    }
}
