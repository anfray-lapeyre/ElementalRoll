using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SettingsControllerScript : MonoBehaviour
{

    public AudioMixer audioMixer;
    private Resolution[] resolutions;
    public Color highlightColor = Color.black;
    public Toggle fullScreenToggle;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Image fillBackground;

    public Slider VolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SoundVolumeSlider;

    public Button backButton;

    private bool inSetting = false;

    public Image[] parents;

    private const int FULLSCREEN = 0, RESOLUTION = 1, QUALITY = 2, VOLUME = 3, MUSIC = 4, SOUND = 5, BACK = 6;

    private UIFader fader;

    private void Awake()
    {
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


        fullScreenToggle.isOn = Screen.fullScreen;

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
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("GlobalVolume", volume);
    }

    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void setSoundVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
    }

    public void setQuality(int qualityIndex)
    {
        if (QualitySettings.GetQualityLevel() != qualityIndex)
            QualitySettings.SetQualityLevel(qualityIndex);
    }


    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }


    public void SetResolution(int resolutionIndex)
    {
        if (resolutions[resolutionIndex].width != Screen.currentResolution.width || resolutions[resolutionIndex].height != Screen.currentResolution.height)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);


        }
    }

    public void OnReturn(InputValue input)
    {
        if (inSetting)
        {
            inSetting = false;
        }
        else
        {
            getBack();
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

    private void OnDestroy()
    {
        if (this.GetComponentInParent<menuControllerScript>())
        {
            this.GetComponentInParent<menuControllerScript>().OptionClose();
        }else if (this.GetComponentInParent<pauseMenuScript>())
        {
            this.GetComponentInParent<pauseMenuScript>().OptionClose();
        }

    }

}
