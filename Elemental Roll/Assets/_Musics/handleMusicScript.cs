using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class handleMusicScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioClip[] musics;
    public int currentMusic;

    // Start is called before the first frame update
    void Start()
    {
        SetSound();
        SetMusic();
    }

    void SetSound()
    {
        GameObject[] sources = GameObject.FindGameObjectsWithTag("SoundFX");
        foreach(GameObject source in sources)
        {
            AudioSource soundFX = source.GetComponent<AudioSource>();
            soundFX.outputAudioMixerGroup=audioMixer.FindMatchingGroups("Sounds")[0];
        }

    }

    void SetMusic()
    {
        AudioSource source = GameObject.FindGameObjectsWithTag("Music")[0].GetComponent<AudioSource>();
        source.outputAudioMixerGroup=audioMixer.FindMatchingGroups("Music")[0];
        source.loop = true;
        source.clip = musics[currentMusic];
        source.Play();
    }


}
