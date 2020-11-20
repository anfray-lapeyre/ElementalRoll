using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class passCinematicScript : Observer
{
    private bool isEnabled = true;
    private GameObject persistantHandler;


    private void Awake()
    {
        persistantHandler = GameObject.FindGameObjectsWithTag("PersistentObject")[0];
        persistantHandler.GetComponent<InputHandler>().addObserver(this);
    }


    override public void OnNotify(GameObject entity, object notifiedEvent)
    {
        switch (notifiedEvent.GetType().ToString())
        {
            case "MoveCommand":
                //OnDirection(((MoveCommand)notifiedEvent).getMove());
                break;
            case "SpellCommand":
                OnSpecialAction(((SpellCommand)notifiedEvent).isPressed());
                break;
            case "RestartCommand":
                OnRestart(((RestartCommand)notifiedEvent).isPressed());
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

    public void OnRestart(bool input)
    {
        if(input)
            passCinematic();
    }

    public void OnSpecialAction(bool input)
    {
        if(input)
            passCinematic();
    }

    private void passCinematic()
    {
        if (isEnabled)
        {
            PlayableDirector director = this.gameObject.GetComponent<PlayableDirector>();
            director.playableGraph.GetRootPlayable(0).SetSpeed(10000);
        }

    }

    public void closeTimeline()
    {
        Destroy(this.gameObject);
    }

    public void disable()
    {
        isEnabled = false;
    }

    public void doEnable()
    {
        isEnabled = true;
    }
}
