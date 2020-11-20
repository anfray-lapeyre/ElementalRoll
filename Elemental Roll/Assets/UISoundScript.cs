using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundScript : Observer
{
    private AudioSource audioSource;
    public AudioClip soundClick;
    public AudioClip soundMoveDown;
    public AudioClip soundMoveUp;

    private void Awake()
    {

        GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<InputHandler>().addObserver(this);
        audioSource = this.GetComponent<AudioSource>();
    }

    override public void OnNotify(GameObject entity, object notifiedEvent)
    {
        switch (notifiedEvent.GetType().ToString())
        {
            case "MoveCommand":
                OnDirection(((MoveCommand)notifiedEvent).getMove());
                break;
            case "SpellCommand":
                OnConfirm(((SpellCommand)notifiedEvent).isPressed());
                break;
            case "RestartCommand":
                OnReturn(((RestartCommand)notifiedEvent).isPressed());
                break;
            case "PauseCommand":
                OnReturn(((PauseCommand)notifiedEvent).isPressed());
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

    public void OnDirection(Vector2 value)
    {
        if (value.y > 0)
        {
            audioSource.clip = soundMoveUp;
            audioSource.Play();
        }
        else if (value.y < 0)
        {
            audioSource.clip = soundMoveDown;
            audioSource.Play();
        }else if (value.x > 0)
        {
            audioSource.clip = soundMoveUp;
            audioSource.Play();
        }
        else if (value.x < 0)
        {
            audioSource.clip = soundMoveDown;
            audioSource.Play();
        }
    }

    public void OnConfirm(bool value)
    {
        if (value)
        {
            audioSource.clip = soundClick;
            audioSource.Play();
        }
    }

    public void OnReturn(bool value)
    {
        if (value)
        {
            audioSource.clip = soundClick;
            audioSource.Play();
        }
    }
}
