using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class handleMusicScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioClip[] musicsCabin;
    public AudioClip[] musicsSunset;
    public AudioClip[] musicsForest;
    public AudioClip[] musicsRiver;
    public AudioClip[] musicsCavern;
    public AudioClip[] musicsHell;
    public AudioClip[] ambientForDialogue;
    public int currentMusicAtmosphere;
    private AudioSource source;
    private float tmpVolume=0f;
    private float tmpTime=0f;

    private bool volumeMustChange = false;
    private float targetVolume = 0f;
    private float targetTime = 0f;
    private float fromVolume = 0f;
    private float stepVolume = 0f;
    private Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];

        SetSound();
        SetMusic(0);
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

    public void SetMusic()
    { 
        Debug.Log("Invoked set music");
        SetMusic(-1);
    }

    public void SetMusic(int _musicValue)
    {
        int musicValue = _musicValue;
        switch (currentMusicAtmosphere)
        {
            case -1:
                musicValue = (musicValue < 0) ? Random.Range(0, ambientForDialogue.Length-1) : musicValue;
                source.clip = ambientForDialogue[musicValue];
                InvokeRealTime("SetMusic", ambientForDialogue[musicValue].length);
                break;
            case 1:
                musicValue = (musicValue<0)?Random.Range(0, musicsSunset.Length-1):musicValue;
                source.clip = musicsSunset[musicValue];
                InvokeRealTime("SetMusic", musicsSunset[musicValue].length);
                break;
            case 2:
                musicValue = (musicValue < 0) ? Random.Range(0, musicsForest.Length-1) : musicValue;
                source.clip = musicsForest[musicValue];
                InvokeRealTime("SetMusic", musicsForest[musicValue].length);
                break;
            case 3:
                musicValue = (musicValue < 0) ? Random.Range(0, musicsRiver.Length-1) : musicValue;
                source.clip = musicsRiver[musicValue];
                InvokeRealTime("SetMusic", musicsRiver[musicValue].length);
                break;
            case 4:
                musicValue = (musicValue < 0) ? Random.Range(0, musicsCavern.Length-1) : musicValue;
                source.clip = musicsCavern[musicValue];
                InvokeRealTime("SetMusic", musicsCavern[musicValue].length);
                break;
            case 5:
                musicValue = (musicValue < 0) ? Random.Range(0, musicsHell.Length-1) : musicValue;
                source.clip = musicsHell[musicValue];
                InvokeRealTime("SetMusic", musicsHell[musicValue].length);
                break;
            default:
                musicValue = (musicValue < 0) ? Random.Range(0, musicsCabin.Length-1) : musicValue;
                source.clip = musicsCabin[musicValue];
                InvokeRealTime("SetMusic", musicsCabin[musicValue].length);
                break;
        }
        source.Play();
        
    }

    public void changeMusic(int value, float time)
    {
        StopCoroutine(coroutine);

        //Transition by lowering volume
        tmpVolume = source.volume;
        setVolume(0, time/2f);
        tmpTime = time;
        currentMusicAtmosphere = value;

        InvokeRealTime("changeMusicafterWait", time/2f + 0.1f);
    }

    void changeMusicafterWait()
    {
        //Then changing music
        SetMusic();

        //And setting volume up
        setVolume(tmpVolume, tmpTime / 2f);
    }

    public void setVolume(float value, float time)
    {
        //Changes volumes to set value, in given time

        volumeMustChange = true;
        targetTime = time;
        targetVolume = value;
        fromVolume = source.volume;
        stepVolume = Mathf.Sign(targetVolume - fromVolume)/targetTime;
        //LeanTween.value(this.gameObject, source.volume, value, time).setOnUpdate((float val)=> { source.volume = val; });

    }


    void InvokeRealTime(string functionName, float delay)
    {
        coroutine = StartCoroutine(InvokeRealTimeHelper(functionName, delay));
    }

    private IEnumerator InvokeRealTimeHelper(string functionName, float delay)
    {
        float timeElapsed = 0f;
        while (timeElapsed < delay)
        {
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        SendMessage(functionName);
    }

    private void FixedUpdate()
    {
        if (volumeMustChange)
        {
            source.volume += stepVolume * Time.fixedUnscaledDeltaTime;

            if (Mathf.Abs(source.volume - targetVolume) <=0.01f )
            {
                volumeMustChange = false;
            }
        }
    }

}
