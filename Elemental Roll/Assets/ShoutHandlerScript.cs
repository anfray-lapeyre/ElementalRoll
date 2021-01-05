using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutHandlerScript : MonoBehaviour
{
    public int character = 0; //0 : Byle, 1: Tracy, 2:Rocky, 3:Tim
    public AudioClip[] negativeShouts;
    public AudioClip[] positiveShouts;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.loop = false;
        Character dialogueCharacter = new Character();
        switch (character)
        {
            case 1:
                dialogueCharacter = dialogueCharacter.Ice();
                break;
            case 2:
                dialogueCharacter = dialogueCharacter.Earth();
                break;
            case 3:
                dialogueCharacter = dialogueCharacter.Death();
                break;
            default:
                dialogueCharacter = dialogueCharacter.Fire();
                break;
        }

        audioSource.pitch = dialogueCharacter.pitch;
    }

    public void PlayAudio(bool positive=true)
    {
        if(positive)
            PlayAudioPositive(Random.Range(0,positiveShouts.Length - 1 ));
        else
            PlayAudioNegative(Random.Range(0, negativeShouts.Length - 1));

    }

    public void PlayAudioPositive(int value)
    {
        if (value >= 0 && value< positiveShouts.Length)
        {
            audioSource.PlayOneShot(positiveShouts[value]);
        }
    }

    public void PlayAudioNegative(int value)
    {
        if (value >= 0 && value < negativeShouts.Length)
        {
            audioSource.PlayOneShot(negativeShouts[value]);
        }
    }
}
