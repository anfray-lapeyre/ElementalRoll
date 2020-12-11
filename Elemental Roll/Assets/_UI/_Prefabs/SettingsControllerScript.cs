using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SettingsControllerScript : Observer
{

    public AudioMixer audioMixer;
    private Resolution[] resolutions;
    public Color highlightColor = Color.black;
    public UIToggle fullScreenToggle;
    public UIDropDown resolutionDropdown;
    public UIDropDown qualityDropdown;
    public Image fillBackground;

    public UIValueSlider VolumeSlider;
    public UIValueSlider MusicVolumeSlider;
    public UIValueSlider SoundVolumeSlider;

    public UIButton backButton;

    private bool inSetting = false;

    public Image[] parents;

    private const int FULLSCREEN = 0, RESOLUTION = 1, QUALITY = 2, VOLUME = 3, MUSIC = 4, SOUND = 5, BACK = 6;

    private UIFader fader;

    private AudioSource audioSource;
    public AudioClip soundClick;
    public AudioClip soundMoveDown;
    public AudioClip soundMoveUp;
    public AudioClip soundSwoosh;

    public UIStateMachine stateMachine;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        if (Time.timeScale > 0.01f)
        {
            fader = this.GetComponent<UIFader>();
            fader.FadeIn();
            fillBackground.color = Color.clear;

        }
        else
        {
            this.GetComponent<CanvasGroup>().alpha = 1f;
        }

        if (Screen.fullScreen)
        {
            fullScreenToggle.Check();
        }
        else
        {
            fullScreenToggle.Uncheck();
        }

        //SOUND SLIDERS INITIALIZATION
        float value;
        audioMixer.GetFloat("GlobalVolume", out value);
        VolumeSlider.value = value;
        VolumeSlider.Refresh();

        audioMixer.GetFloat("MusicVolume", out value);
        MusicVolumeSlider.value = value;
        MusicVolumeSlider.Refresh();

        audioMixer.GetFloat("SoundVolume", out value);
        SoundVolumeSlider.value = value;
        SoundVolumeSlider.Refresh();

        qualityDropdown.value = QualitySettings.GetQualityLevel();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<InputHandler>().addObserver(this);

    }

    override public void OnNotify(GameObject entity, object notifiedEvent)
    {
        switch (notifiedEvent.GetType().ToString())
        {
            case "MoveCommand":
                OnDirection(((MoveCommand)notifiedEvent).getMove());
                break;
            case "SpellCommand":
                //OnConfirm(((SpellCommand)notifiedEvent).isPressed());
                break;
            case "RestartCommand":
                //OnReturn(((RestartCommand)notifiedEvent).isPressed());
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

    public void setVolume()
    {

        audioSource.clip = soundClick;
        audioSource.Play();
        
        audioMixer.SetFloat("GlobalVolume", VolumeSlider.value);
    }

    public void setMusicVolume()
    {

        audioSource.clip = soundClick;
        audioSource.Play();
        audioMixer.SetFloat("MusicVolume", MusicVolumeSlider.value);
    }

    public void setSoundVolume()
    {
        audioSource.clip = soundClick;
        audioSource.Play();
        audioMixer.SetFloat("SoundVolume", SoundVolumeSlider.value);
    }

    public void setQuality()
    {
        audioSource.clip = soundClick;
        audioSource.Play();
        if (QualitySettings.GetQualityLevel() != qualityDropdown.value)
            QualitySettings.SetQualityLevel(qualityDropdown.value);
        //stateMachine.mustWait = true;
    }


    public void SetFullScreen(bool isFullScreen)
    {
        if (audioSource != null)
        {
            audioSource.clip = soundClick;
            audioSource.Play();
        }
        Screen.fullScreen = isFullScreen;
        //stateMachine.mustWait = true;
    }


    public void SetResolution()
    {
        if (resolutions[resolutionDropdown.value].width != Screen.currentResolution.width || resolutions[resolutionDropdown.value].height != Screen.currentResolution.height)
        {
            audioSource.clip = soundClick;
            audioSource.Play();
            Resolution resolution = resolutions[resolutionDropdown.value];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        }
        //stateMachine.mustWait = true;
    }

    public void OnReturn(bool input)
    {
        if (input && audioSource.enabled)
        {
            if (inSetting)
            {

                audioSource.clip = soundClick;
                audioSource.Play();
                inSetting = false;
            }
            else
            {
                audioSource.clip = soundSwoosh;
                audioSource.Play();
                getBack();
            }
        }
    }

    public void getBack()
    {
        if (Time.timeScale >= 0.1f)
        {
            fader.FadeOut();
            Destroy(this.gameObject, 0.5f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    override protected void OnDestroy()
    {
        base.OnDestroy();
        if (this.GetComponentInParent<menuControllerScript>())
        {
            this.GetComponentInParent<menuControllerScript>().OptionClose();
        }else if (this.GetComponentInParent<pauseMenuScript>())
        {
            this.GetComponentInParent<pauseMenuScript>().OptionClose();
        }

    }

    public void OnDirection(Vector2 value)
    {
        if (value.y > 0)
        {
            audioSource.clip = soundMoveUp;
            audioSource.Play();
        }else if (value.y < 0)
        {
            audioSource.clip = soundMoveDown;
            audioSource.Play();
        }
    }


}
