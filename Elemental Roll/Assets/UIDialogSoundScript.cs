using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialogSoundScript : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void Play()
    {
        audioSource.Play();
        print("SOUND");
    }
}
