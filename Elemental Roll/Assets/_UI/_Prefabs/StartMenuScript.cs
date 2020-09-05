using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartMenuScript : MonoBehaviour
{
    public IntVariable currentLevel;
    public GameObject UIFade;
    public bool isClosing = false;
    public GameObject LevelLoader; // Base
    private LevelLoader _levelLoader; //Instantiated child script
    public IntVariable LivesLeft;
    public IntVariable difficultyLife;
    public IntVariable difficultyChrono;

    public GameObject ConfirmChoiceWindow;
    public EventSystem eventSystem;

    public GameObject chooseSave;
    public UIFader firstTimefade;

    private void Awake()
    {
        currentLevel.value = ActualSave.actualSave.NextLevel();
        difficultyLife.value = ActualSave.actualSave.chosenLivesDifficulty;
        difficultyChrono.value = ActualSave.actualSave.chosenTimeDifficulty;
        this.GetComponent<UIFader>().FadeIn();
        GameObject levelLoader = Instantiate(LevelLoader);
        _levelLoader = levelLoader.GetComponent<LevelLoader>();
        difficultyLife.value = ActualSave.actualSave.chosenLivesDifficulty;
        difficultyChrono.value = ActualSave.actualSave.chosenTimeDifficulty;
    }

    public void ContinueGame()
    {
        //UIFade.GetComponent<UIFader>().FadeIn();
        _levelLoader.ShowLoader();
        this.GetComponent<UIFader>().FadeOut();
        _levelLoader.LoadNextLevel(currentLevel.value, false);
    }

    public void NewGame()
    {
        //Instantiate(UIFade);
        //UIFade.GetComponent<UIFader>().FadeIn();

        eventSystem.enabled = false;

        GameObject confirmPanel = Instantiate(ConfirmChoiceWindow, this.transform);
        confirmPanel.GetComponent<confirmDifficultyChoiceScript>().setStartText(2);

    }

    public void hasConfirmedSettings()
    {
        eventSystem.enabled = true;

        ActualSave.actualSave = new SaveFileInfo();
        SaveSystem.SaveGame(ActualSave.actualSave, ActualSave.saveSlot);
        LivesLeft.value = 5;
        _levelLoader.ShowLoader();
        this.GetComponent<UIFader>().FadeOut();
        _levelLoader.LoadNextLevel(0, false);
    }

    public void hasCancelledSettings()
    {
        eventSystem.enabled = true;
    }

    public void SpawnLevelSelection()
    {
        //ActualSave.actualSave.fillWithBeaten(6);
        _levelLoader.LoadNextLevel(-1, false);
    }

    public void LoadGame()
    {
        GameObject g = Instantiate(chooseSave, this.transform.parent);
        eventSystem.enabled = false;
        g.GetComponent<chooseSaveScript>().SetTarget(this);
    }

    public void OutOfSave(bool isNew = false)
    {
        if (isNew)
        {
            firstTimefade.FadeIn(2f);
            Invoke("LaunchCinematic", 2f);
        }
        else
        {
            this.GetComponentInParent<menuControllerScript>().StartMenuClose();
            this.GetComponent<UIFader>().FadeOut();
            _levelLoader.LoadNextLevel(-2); //We reload with the new selected save
        }
    }

    public void getBack()
    {
        if (!isClosing)
        {
            isClosing = true;
            this.GetComponentInParent<menuControllerScript>().StartMenuClose();
            this.GetComponent<UIFader>().FadeOut();
            Destroy(this.gameObject, 1f);

        }
 
    }
}
