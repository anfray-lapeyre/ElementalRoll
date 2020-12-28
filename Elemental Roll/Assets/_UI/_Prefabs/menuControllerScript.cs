using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class menuControllerScript : Observer
{
    public PlayableDirector timeline;
    private passCinematicScript passCinematic;
    public CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera StartCamera;
    public CinemachineVirtualCamera OptionsCamera;
    public CinemachineVirtualCamera Transition_OptionsCamera;
    public CinemachineVirtualCamera PlayerSelectionCamera;
    public CinemachineVirtualCamera PlayerInSelectionCamera;
    public CinemachineVirtualCamera hasStartedCamera;
    public CinemachineVirtualCamera inOptionsCamera;

    public playerSelectionScript playerSelection;


    private int isActive = 0;
    private const int START = 0;
    private const int OPTIONS = 1;
    private const int PLAYERSELECTION = 2;
    private const int PLAYERINSELECTION = 3;
    private bool inOption = false;

    public GameObject OptionsMenu;
    public GameObject StartMenu;
    public GameObject StartText;
    public GameObject OptionText;
    public GameObject PlayerText;

    public GameObject SaveSelectionScreen;


    private GameObject bufferObject;

    public UIFader firstTimefade;

    private AudioSource audioSource;
    public AudioClip soundSwoosh;
    public AudioClip soundZoom;

    public bool inTransition = false;


    public void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<InputHandler>().addObserver(this);

    }


    override public void OnNotify(GameObject entity, object notifiedEvent)
    {
        switch (notifiedEvent.GetType().ToString())
        {
            case "MoveCommand":
                if (((MoveCommand)notifiedEvent).isNotJoystick()) {
                    OnDirection(((MoveCommand)notifiedEvent).getMove());
                }
                break;
            case "SpellCommand":
                OnConfirm(((SpellCommand)notifiedEvent).isPressed());
                break;
            case "RestartCommand":
                //OnRestart(((RestartCommand)notifiedEvent).isPressed());
                break;
            case "PauseCommand":
                //OnPause(((RestartCommand)notifiedEvent).isPressed());
                break;
            case "EagleViewCommand":
                //OnPause(((EagleViewCommand)notifiedEvent).isPressed());
                break;
            case "TopViewCommand":
                //OnPause(((TopViewCommand)notifiedEvent).isPressed());
                break;
            default:
                break;
        }
    }

    public void EnableInput()
    {
        bufferObject = Instantiate(StartText);
        GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<InputHandler>().addObserver(this);
        
    }

    public void OnConfirm(bool input)
    {
        if (input)
        {
            if (!cinemachineBrain.IsBlending && !inOption)
            {
                audioSource.clip = soundZoom;
                audioSource.Play();
                switch (isActive)
                {
                    case OPTIONS:
                        LoadOptionTab();
                        break;
                    case PLAYERSELECTION:
                        ChoosePlayer();
                        break;
                    case PLAYERINSELECTION:
                        ConfirmPlayer();
                        break;
                    default: //case START
                        LoadLevelSelectionTab();
                        break;
                }
            }
        }
    }

    //0 -> Start
    //1 -> Options
    //2 -> PlayerSelection

    public void OnDirection(Vector2 input)
    {
        if (!inTransition)
        {
            if (!cinemachineBrain.IsBlending && !inOption)
            {
                switch (isActive)
                {
                    case OPTIONS:
                        if (input.x > 0.1f)
                        {
                            audioSource.clip = soundSwoosh;
                            audioSource.Play();
                            GoToStart();
                        }
                        else if (input.x < -0.1f)
                        {
                            audioSource.clip = soundSwoosh;
                            audioSource.Play();
                            GoToPlayers();
                        }
                        else if (input.y > 0.1f)
                        {
                            audioSource.clip = soundZoom;
                            audioSource.Play();
                            LoadOptionTab();
                        }
                        break;
                    case PLAYERSELECTION:
                        if (input.x > 0.1f)
                        {
                            audioSource.clip = soundSwoosh;
                            audioSource.Play();
                            GoToOptions();
                        }
                        else if (input.x < -0.1f)
                        {
                            audioSource.clip = soundSwoosh;
                            audioSource.Play();
                            GoToStart();
                        }
                        else if (input.y > 0.1f)
                        {
                            audioSource.clip = soundZoom;
                            audioSource.Play();
                            ChoosePlayer();
                        }
                        break;
                    case PLAYERINSELECTION:
                        if (input.x > 0.1f)
                        {
                            audioSource.clip = soundSwoosh;
                            audioSource.Play();
                            PlayerSelectionGoRight();
                        }
                        else if (input.x < -0.1f)
                        {
                            audioSource.clip = soundSwoosh;
                            audioSource.Play();
                            PlayerSelectionGoLeft();
                        }
                        else if (input.y > 0.1f)
                        {
                            audioSource.clip = soundZoom;
                            audioSource.Play();
                            ConfirmPlayer();
                        }
                        else if (input.y < -0.1f)
                        {
                            audioSource.clip = soundZoom;
                            audioSource.Play();
                            OutOfPlayerSelectionPanel();
                        }
                        break;
                    default: //case START
                        if (input.x > 0.1f)
                        {
                            audioSource.clip = soundSwoosh;
                            audioSource.Play();
                            GoToPlayers();
                        }
                        else if (input.x < -0.1f)
                        {
                            audioSource.clip = soundSwoosh;
                            audioSource.Play();
                            GoToOptions();
                        }
                        else if (input.y > 0.1f)
                        {
                            audioSource.clip = soundZoom;
                            audioSource.Play();
                            LoadLevelSelectionTab();
                        }
                        break;
                }
            }
            inTransition = true;
            InvokeRealTime("notInTransition", 0.2f);
        }
    }

    public void notInTransition()
    {
        inTransition = false;
    }

    public void checkSave()
    {
        if(ActualSave.actualSave == null)
        {
            if (subject)
            {
                //If no actual save is stored, it means the game is starting
                spawnSave();
                inOption = true;
                //timeline.Pause();
                passCinematic = timeline.gameObject.GetComponent<passCinematicScript>();
                passCinematic.disable();
            }
        }
        else
        {
            UpdateProgress();
        }
        //Debug.Log(ActualSave.actualSave + " " + ActualSave.saveSlot);
    }

    private void spawnSave()
    {
        subject.removeObserver(this);

        GameObject tmp = Instantiate(SaveSelectionScreen, this.transform);
        //tmp.GetComponent<chooseSaveScript>().stateMachine.mustWait = true;
    }

    public void LaunchCinematic()
    {
        SceneManager.LoadScene("CutsceneCabin1");
    }

    public void OutOfSave(bool isNew=false)
    {

        if (isNew)
        {
                firstTimefade.FadeIn(1f);
                InvokeRealTime("LaunchCinematic", 2f);
               
        }
        else
        {
            GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<InputHandler>().addObserver(this);

            inOption = false;
            passCinematic.doEnable();
            //GoToStart();
            UpdateProgress();
        }
        

    }

    private void LoadLevelSelectionTab()
    {
        subject.removeObserver(this);
        ResetPriority();
        hasStartedCamera.Priority = 20;
        inOption = true;
        InvokeRealTime("InstantiateStartPanel", 0.5f);
    }

    private void InstantiateStartPanel()
    {
        Instantiate(StartMenu, this.transform);
    }

    private void LoadOptionTab()
    {
        if (!inOption)
        {
            subject.removeObserver(this);
            inOption = true;
            ResetPriority();
            inOptionsCamera.Priority = 20;
            InvokeRealTime("InstantiateOptionPanel", 0.5f);
        }
    }

    private void InstantiateOptionPanel()
    {
        Instantiate(OptionsMenu, this.transform);
    }

    private void ChoosePlayer()
    {
        if (!inOption)
        {
            ResetPriority();
            PlayerInSelectionCamera.Priority = 20;
            InvokeRealTime("InstantiatePlayerSelectionPanel", 0.5f);
            isActive = PLAYERINSELECTION;
        }
    }

    private void InstantiatePlayerSelectionPanel()
    {
        playerSelection.EnterSelection();
    }

    private void OutOfPlayerSelectionPanel()
    {

        ResetPriority();
        PlayerSelectionCamera.Priority = 20;
        isActive = PLAYERSELECTION;
        InvokeRealTime("spawnChoosePlayerText", 0.5f);
        playerSelection.QuitSelection();
    }

    private void GoToStart()
    {
        if (isActive != START)
        {
            ResetPriority();
            StartCamera.Priority = 20;
            InvokeRealTime("spawnStartText", 0.5f);
            isActive = START;
        }
    }

    private void spawnStartText()
    {
        bufferObject = Instantiate(StartText);
    }

    private void GoToOptions()
    {
        if(isActive != OPTIONS)
        {
            ResetPriority();
            Transition_OptionsCamera.Priority = 20;
            InvokeRealTime("TransitionOption",0.2f);
            isActive = OPTIONS;
        }
    }

    private void TransitionOption()
    {
        ResetPriority();
        OptionsCamera.Priority = 20;
        InvokeRealTime("spawnOptionText", 0.5f);
    }

    private void spawnOptionText()
    {
        bufferObject = Instantiate(OptionText);
    }

    private void GoToPlayers()
    {
        if (isActive != PLAYERSELECTION)
        {
            ResetPriority();
            PlayerSelectionCamera.Priority = 20;
            isActive = PLAYERSELECTION;
            InvokeRealTime("spawnChoosePlayerText", 0.5f);
        }
    }

    private void spawnChoosePlayerText()
    {
        bufferObject = Instantiate(PlayerText);
    }

    private void ResetPriority()
    {
        if (bufferObject)
        {
            TextFaderAtSpawnScript textFader = bufferObject.GetComponent<TextFaderAtSpawnScript>();
            if(textFader && !textFader.isFading())
                textFader.Kill();
            else if (textFader)
            {
                Destroy(textFader.gameObject, 0.2f);
            }
        }
        StartCamera.Priority = 1;
        OptionsCamera.Priority = 1;
        inOptionsCamera.Priority = 1;
        PlayerSelectionCamera.Priority = 1;
        Transition_OptionsCamera.Priority = 1;
        hasStartedCamera.Priority = 1;
        PlayerInSelectionCamera.Priority = 1;
    }

    public void OptionClose()
    {
        InvokeRealTime("outOfOption", 1f);
        ResetPriority();
        OptionsCamera.Priority = 20;
        InvokeRealTime("spawnOptionText", 0f);
    }

    public void outOfOption()
    {
        GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<InputHandler>().addObserver(this);

        inOption = false;
    }

    public void StartMenuClose()
    {
        InvokeRealTime("outOfOption", 1f);
        ResetPriority();
        StartCamera.Priority = 20;
        InvokeRealTime("spawnStartText", 0.5f);
    }

    private void UpdateProgress()
    {
        playerSelection.UpdateProgress();
    }

    private void PlayerSelectionGoLeft()
    {
        playerSelection.goLeft();
    }

    private void PlayerSelectionGoRight()
    {
        playerSelection.goRight();
    }

    private void ConfirmPlayer()
    {
        playerSelection.confirmChoice();
        InvokeRealTime("OutOfPlayerSelectionPanel", 0.3f);
    }

}
