using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class difficultySettingsScript : MonoBehaviour
{

    public GameObject LevelLoader;
    public UIStateMachine stateMachine;
    public GameObject ConfirmChoiceWindow;

    public IntVariable LivesDifficulty;
    public IntVariable TimeDifficulty;
    public IntVariable currentLevel;


    public UIToggleGroup TimeGroup;
    public UIToggleGroup LivesGroup;

    public UIToggle Time0;
    public UIToggle Time1;
    public UIToggle Time2;

    public UIToggle Lives0;
    public UIToggle Lives1;
    public UIToggle Lives2;

    int iteration = 0;

    public UIButton confirmButton;

    public UIButton backButton;
    private Color disabledColor = new Color(0.1f, 0.1f, 0.1F);
    private TMPro.TextMeshProUGUI text;

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
        text = confirmButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.color = disabledColor;


    }

    private void Start()
    {
        if (!initialDifficultyChoice)
        {
            chosenLife = LivesDifficulty.value;
            chosenTime = TimeDifficulty.value;
            TimeGroup.SetAllTogglesOff();
            LivesGroup.SetAllTogglesOff();
            switch (LivesDifficulty.value)
            {
                case 1:
                    Lives1.Check();
                    break;
                case 2:
                    Lives2.Check();

                    break;
                default:
                    Lives0.Check();
                    break;
            }
            switch (TimeDifficulty.value)
            {
                case 1:
                    Time1.Check();
                    break;
                case 2:
                    Time2.Check();
                    break;
                default:
                    Time0.Check();
                    break;
            }
        }
        else
        {
            LivesDifficulty.value = -2;
            TimeDifficulty.value = -2;
            Debug.Log("Initial difficulty setting");
        }
    }

    public void isInitialDifficultyChoice()
    {
        chosenLife = -1;
        chosenTime = -1;
        text.color = disabledColor;
        initialDifficultyChoice = true;
        confirmButton.activate( false);
        //text.color = Color.white;
        TimeGroup.SetAllTogglesOff();
        LivesGroup.SetAllTogglesOff();
        Debug.Log("BIM, Initial difficulty choice");
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
                    confirmButton.activate(true);
                    text.color = Color.white;
                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.activate(false);
                    text.color = disabledColor;


                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenTime = 0;
                if (chosenLife >= 0)
                {
                    confirmButton.activate(true);
                    text.color = Color.white;
                    Debug.Log("Confirm should activate");
                }
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
                    confirmButton.activate(true);
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.activate(false);
                    text.color = disabledColor;

                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenTime = 1;
                if (chosenLife >= 0)
                {
                    confirmButton.activate(true);
                    text.color = Color.white;
                }
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
                    confirmButton.activate(true);
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.activate(false);
                    text.color = disabledColor;

                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenTime = 2;
                if (chosenLife >= 0)
                {
                    confirmButton.activate(true);
                    text.color = Color.white;
                }
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
                    confirmButton.activate(true);
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.activate(false);
                    text.color = disabledColor;

                }

            }
        }
        else
        {
            if (isToggled)
            {
                chosenLife = 0;
                if (chosenTime >= 0)
                {
                    confirmButton.activate(true);
                    text.color = Color.white;
                }
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
                    confirmButton.activate(true);
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.activate(false);
                    text.color = disabledColor;

                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenLife = 1;
                if (chosenTime >= 0)
                {
                    confirmButton.activate(true);
                    text.color = Color.white;
                }
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
                    confirmButton.activate(true);
                    text.color = Color.white;

                }
                else if (chosenTime == TimeDifficulty.value && chosenLife == LivesDifficulty.value)
                {
                    confirmButton.activate(false);
                    text.color = disabledColor;

                }
            }
        }
        else
        {
            if (isToggled)
            {
                chosenLife = 2;
                if (chosenTime >= 0)
                {
                    confirmButton.activate(true);
                    text.color = Color.white;
                }
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

            confirmPanel.GetComponentInChildren<UIStateMachine>().mustWait=true;
            stateMachine.subject.removeObserver(stateMachine);
            stateMachine.mustWait = true;
            //eventSystem.enabled = false;
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
            GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<InputHandler>().addObserver(stateMachine);
            //eventSystem.enabled = true;
        }
      
    }

    public void hasCancelledSettings()
    {
        GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<InputHandler>().addObserver(stateMachine);

        //eventSystem.enabled = true;
    }



    public void getBack()
    {
        if (initialDifficultyChoice)
        {
            this.GetComponentInParent<chooseSaveScript>().hasCancelledSettings();
            this.GetComponentInParent<chooseSaveScript>().stateMachine.mustWait = true;
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
