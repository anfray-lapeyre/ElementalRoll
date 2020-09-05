using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class difficultySettingsScript : MonoBehaviour
{

    public GameObject LevelLoader;
    public EventSystem eventSystem;
    public GameObject ConfirmChoiceWindow;

    public IntVariable LivesDifficulty;
    public IntVariable TimeDifficulty;
    public IntVariable currentLevel;


    public ToggleGroup TimeGroup;
    public ToggleGroup LivesGroup;

    public Toggle Time0;
    public Toggle Time1;
    public Toggle Time2;

    public Toggle Lives0;
    public Toggle Lives1;
    public Toggle Lives2;

    int iteration = 0;

    public Button confirmButton;

    public Button backButton;
    private Color disabledColor = new Color(0.1f, 0.1f, 0.1F);
    private TMPro.TMP_Text text;

    private int chosenLife;
    private int chosenTime;
    private bool initialDifficultyChoice = false;

    private void Update()
    {
        if (iteration < 2)
        {
            iteration++;
        }
    }

    private void Awake()
    {
        text = confirmButton.GetComponentsInChildren<TMPro.TMP_Text>()[0];
        text.color = disabledColor;
        chosenLife = LivesDifficulty.value;
        chosenTime = TimeDifficulty.value;
        TimeGroup.SetAllTogglesOff();
        LivesGroup.SetAllTogglesOff();
        switch (LivesDifficulty.value)
        {
            case 1:
                Lives1.isOn = true;
                break;
            case 2:
                Lives2.isOn = true;
                break;
            default:
                Lives0.isOn = true;
                break;
        }
        switch (TimeDifficulty.value)
        {
            case 1:
                Time1.isOn = true;
                break;
            case 2:
                Time2.isOn = true;
                break;
            default:
                Time0.isOn = true;
                break;
        }

        TimeGroup.allowSwitchOff = false;
        LivesGroup.allowSwitchOff = false;
    }

    public void isInitialDifficultyChoice()
    {
        initialDifficultyChoice = true;
        confirmButton.interactable = true;
        text.color = Color.white;
    }

    public void setTimeDifficultyUnlimited( bool isToggled)
    {
        if (!initialDifficultyChoice)
        {
            if (iteration >= 2 && isToggled)
            {
                chosenTime = 0;
                if (chosenTime != TimeDifficulty.value)
                {
                    confirmButton.interactable = true;
                    text.color = Color.white;
                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.interactable = false;
                    text.color = disabledColor;


                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenTime = 0;
            }
        }

    }
    public void setTimeDifficultyNormal( bool isToggled)
    {
        if (!initialDifficultyChoice)
        {
            if (iteration >= 2 && isToggled)
            {

                chosenTime = 1;
                if (chosenTime != TimeDifficulty.value)
                {
                    confirmButton.interactable = true;
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.interactable = false;
                    text.color = disabledColor;

                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenTime = 1;
            }
        }
    }
    public void setTimeDifficultyChrono( bool isToggled)
    {
        if (!initialDifficultyChoice)
        {
            if (iteration >= 2 && isToggled)
            {

                chosenTime = 2;
                if (chosenTime != TimeDifficulty.value)
                {
                    confirmButton.interactable = true;
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.interactable = false;
                    text.color = disabledColor;

                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenTime = 2;
            }
        }
    }

    public void setLivesDifficultyUnlimited(bool isToggled)
    {
        if (!initialDifficultyChoice)
        {
            if (iteration >= 2 && isToggled)
            {

                chosenLife = 0;
                if (chosenLife != LivesDifficulty.value)
                {
                    confirmButton.interactable = true;
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.interactable = false;
                    text.color = disabledColor;

                }

            }
        }
        else
        {
            if (isToggled)
            {
                chosenLife = 0;
            }
        }
    }
    public void setLivesDifficultyPerLevel(bool isToggled)
    {
        if (!initialDifficultyChoice)
        {
            if (iteration >= 2 && isToggled)
            {

                chosenLife = 1;
                if (chosenLife != LivesDifficulty.value)
                {
                    confirmButton.interactable = true;
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.interactable = false;
                    text.color = disabledColor;

                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenLife = 1;
            }
        }
    }
    public void setLivesDifficultyPerTry(bool isToggled)
    {
        if (!initialDifficultyChoice)
        {
            if (iteration >= 2 && isToggled)
            {

                chosenLife = 2;
                if (chosenLife != LivesDifficulty.value)
                {
                    confirmButton.interactable = true;
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.interactable = false;
                    text.color = disabledColor;

                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenLife = 2;
            }
        }
    }

    public void Confirm()
    {
        if (initialDifficultyChoice)
        {
            LivesDifficulty.value = chosenLife;
            TimeDifficulty.value = chosenTime;
            currentLevel.value = 0;
            ActualSave.actualSave.chosenLivesDifficulty = chosenLife;
            ActualSave.actualSave.chosenTimeDifficulty = chosenTime;
            this.GetComponentInParent<chooseSaveScript>().hasConfirmedSettings();
            Destroy(this.gameObject);
        }
        else
        {
            GameObject confirmPanel = Instantiate(ConfirmChoiceWindow, this.transform);

            confirmPanel.GetComponent<confirmDifficultyChoiceScript>().setStartText(!(chosenLife == 2 && chosenLife != LivesDifficulty.value) ? 1 : 0);

            eventSystem.enabled = false;
        }
    }

    public void hasConfirmedSettings()
    {
        

        LevelLoader _levelLoader = Instantiate(LevelLoader).GetComponent<LevelLoader>(); //Instantiated child script
        bool isDifferent = chosenLife != LivesDifficulty.value || chosenTime != TimeDifficulty.value;
        bool mustRestartFromZero = chosenLife != LivesDifficulty.value && chosenLife == 2;
        if (mustRestartFromZero)
        {
            LivesDifficulty.value = chosenLife;
            TimeDifficulty.value = chosenTime;
            _levelLoader.ShowLoader();
            Time.timeScale = 1f;
            currentLevel.value = 0;

            _levelLoader.LoadNextLevel(0, false); //First Level

        }
        else if (isDifferent)
        {
            //This check avoids exploiting changing time difficulty to regain lives
            if (chosenLife != LivesDifficulty.value)
            {
                LivesDifficulty.value = chosenLife;
            }
            TimeDifficulty.value = chosenTime;
            _levelLoader.ShowLoader();
            Time.timeScale = 1f;

            _levelLoader.LoadNextLevel(currentLevel.value, false); //Reload Current Level
        }
        else
        {
            eventSystem.enabled = true;
        }
      
    }

    public void hasCancelledSettings()
    {
        eventSystem.enabled = true;
    }



    public void getBack()
    {
        if (initialDifficultyChoice)
        {
            this.GetComponentInParent<chooseSaveScript>().hasCancelledSettings();
        }
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (this.GetComponentInParent<chooseSaveScript>() == null)
        {

            if (this.GetComponentInParent<menuControllerScript>())
            {
                this.GetComponentInParent<menuControllerScript>().OptionClose();
            }
            else if (this.GetComponentInParent<pauseMenuScript>())
            {
                this.GetComponentInParent<pauseMenuScript>().OptionClose();
            }
        }

    }

}
