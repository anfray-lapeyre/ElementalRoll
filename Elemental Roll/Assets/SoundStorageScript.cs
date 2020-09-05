using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundStorageScript : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, AudioClip>> values;
    public AudioClip[] Fireaudios;
    public AudioClip[] Iceaudios;
    public AudioClip[] Earthaudios;
    public AudioClip[] Deathaudios;
    public AudioClip[] Wizardaudios;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
       
    }

    private void InitializeDictionary()
    {
        values = new Dictionary<string, Dictionary<string, AudioClip>>();
        values.Add("Byle", new Dictionary<string, AudioClip>());
        foreach (AudioClip sound in Fireaudios)
        {
            values["Byle"].Add(sound.name, sound);
        }
        values.Add("Tracy", new Dictionary<string, AudioClip>());
        foreach (AudioClip sound in Iceaudios)
        {
            values["Tracy"].Add(sound.name, sound);
        }
        values.Add("Rocky", new Dictionary<string, AudioClip>());
        foreach (AudioClip sound in Earthaudios)
        {
            values["Rocky"].Add(sound.name, sound);
        }
        values.Add("Tim", new Dictionary<string, AudioClip>());
        foreach (AudioClip sound in Deathaudios)
        {
            values["Tim"].Add(sound.name, sound);
        }
        values.Add("Wizard", new Dictionary<string, AudioClip>());
        foreach (AudioClip sound in Wizardaudios)
        {
            values["Wizard"].Add(sound.name, sound);
        }
    }

    public Dictionary<string, Dictionary<string, AudioClip>> getAudio()
    {
        if (values != null)
            return values;
        InitializeDictionary();
        return values;
    }
}
