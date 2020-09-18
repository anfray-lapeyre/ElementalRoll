using UnityEngine;
using UnityEngine.Timeline;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;


public class spawnPlayerScript : MonoBehaviour
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
    // Start is called before the first frame update
    private void Awake()
    {
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
        }

        SetTime();
        levelTitle = Instantiate(titleLevelPrefab);
    }


    public void ActivatePlayer()
    {
        desiredAngle = socle.rotation.eulerAngles.y;
        print(socle.rotation.eulerAngles.y);
        print(socle.localRotation.eulerAngles.y);
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

    public void OnRestart(InputValue input)
    {
        LoadNextLevel();
        PlayableDirector director = cinemachineBrain.gameObject.GetComponent<PlayableDirector>();
        if (!director.playableGraph.IsValid())
        {
            director.Play();
        }
        director.playableGraph.GetRootPlayable(0).SetSpeed(100);
        
    }
}
