using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class UISoundScript : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip soundClick;
    public AudioClip soundMoveDown;
    public AudioClip soundMoveUp;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void OnDirection(InputValue value)
    {
        if (value.Get<Vector2>().y > 0)
        {
            audioSource.clip = soundMoveUp;
            audioSource.Play();
        }
        else if (value.Get<Vector2>().y < 0)
        {
            audioSource.clip = soundMoveDown;
            audioSource.Play();
        }else if (value.Get<Vector2>().x > 0)
        {
            audioSource.clip = soundMoveUp;
            audioSource.Play();
        }
        else if (value.Get<Vector2>().x < 0)
        {
            audioSource.clip = soundMoveDown;
            audioSource.Play();
        }
    }

    public void OnConfirm(InputValue value)
    {
        audioSource.clip = soundClick;
        audioSource.Play();
    }

    public void OnReturn(InputValue value)
    {
        audioSource.clip = soundClick;
        audioSource.Play();
    }
}
