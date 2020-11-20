﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class pauseMenuScript : MonoBehaviour
{
    public GameObject optionsButton;
    public GameObject difficultyButton;
    public GameObject OptionsMenu;
    public GameObject DifficultyMenu;
    public IntVariable currentLevel;
    public Button quitGameButton;
    public UIStateMachine stateMachine;

    private GameObject persistantHandler;


    bool inOption = true;
    public GameObject LevelLoader; // Base

    // Start is called before the first frame update
    void Awake()
    {
        persistantHandler = GameObject.FindGameObjectsWithTag("PersistentObject")[0];
        LeanTween.alphaCanvas(this.GetComponent<CanvasGroup>(), 1f, 0.2f);
        Invoke("stopTime", 0.2f);
        if(currentLevel.value < 0)
        {
            difficultyButton.GetComponent<UIButton>().isActive = false;
            difficultyButton.GetComponentInChildren<TMP_Text>().color = new Color(0.1f, 0.1f, 0.1f);
        
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
            stateMachine.subject.removeObserver(stateMachine);

            inOption = true;
        }
    }

    public void Difficulty()
    {
        if (!inOption)
        {
            Instantiate(DifficultyMenu, this.transform);
            stateMachine.subject.removeObserver(stateMachine);

            inOption = true;
        }
    }

    public void OptionClose()
    {
        inOption = false;
        persistantHandler.GetComponent<InputHandler>().addObserver(stateMachine);
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
