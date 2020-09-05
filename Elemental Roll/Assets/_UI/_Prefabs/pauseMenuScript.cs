using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class pauseMenuScript : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject optionsButton;
    public GameObject difficultyButton;
    public GameObject OptionsMenu;
    public GameObject DifficultyMenu;
    public IntVariable currentLevel;
    public Button quitGameButton;

    bool inOption = true;
    public GameObject LevelLoader; // Base

    // Start is called before the first frame update
    void Awake()
    {

        LeanTween.alphaCanvas(this.GetComponent<CanvasGroup>(), 1f, 0.2f);
        Invoke("stopTime", 0.2f);
        if(currentLevel.value < 0)
        {
            difficultyButton.GetComponent<Button>().interactable = false;
            difficultyButton.GetComponentInChildren<TMP_Text>().color = new Color(0.1f, 0.1f, 0.1f);
            Navigation optionsNav = optionsButton.GetComponent<Button>().navigation;
            optionsNav.selectOnDown = quitGameButton; ;

            Navigation getBackNav = quitGameButton.navigation;
            getBackNav.selectOnUp = optionsButton.GetComponent<Button>();

            optionsButton.GetComponent<Button>().navigation = optionsNav;
            quitGameButton.navigation = getBackNav;
        }
    }

    public void stopTime()
    {
        Time.timeScale = 0.0f;
        inOption = false;

    }

    public void Options()
    {
        if (!inOption)
        {
            Instantiate(OptionsMenu, this.transform);
            eventSystem.enabled = false;
            inOption = true;
        }
    }

    public void Difficulty()
    {
        if (!inOption)
        {
            Instantiate(DifficultyMenu, this.transform);
            eventSystem.enabled = false;
            inOption = true;
        }
    }

    public void OptionClose()
    {
        inOption = false;
        eventSystem.enabled = true;
    }

    public void MainMenu()
    {
        if (!inOption)
        {
            LevelLoader _levelLoader = Instantiate(LevelLoader).GetComponent<LevelLoader>(); //Instantiated child script
            inOption = true;
            Time.timeScale = 1f;

            _levelLoader.LoadNextLevel(-2, false); //Load Menu
            _levelLoader.ShowLoader();
        }
        
    }

    public void Resume()
    {
        if (!inOption)
        {

            Time.timeScale = 1f;
            LeanTween.alphaCanvas(this.GetComponent<CanvasGroup>(), 0f, 0.2f);
            Destroy(this.gameObject, 0.2f);
        }
    }

    public void OnDestroy()
    {
        if(this.GetComponentInParent<PlayerController>())
            this.GetComponentInParent<PlayerController>().QuitOptions();
    }
}
