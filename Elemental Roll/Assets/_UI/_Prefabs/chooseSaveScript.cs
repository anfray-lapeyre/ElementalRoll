using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class chooseSaveScript : MonoBehaviour
{
    public GameObject firstSave;
    public GameObject secondSave;
    public GameObject thirdSave;

    public Image firstImg;
    public Image secondImg;
    public Image thirdImg;

    public TMP_Text firstText;
    public TMP_Text secondText;
    public TMP_Text thirdText;

    public TMP_Text firstPerc;
    public TMP_Text secondPerc;
    public TMP_Text thirdPerc;

    public GameObject firstDelete;
    public GameObject secondDelete;
    public GameObject thirdDelete;

    public EventSystem eventSystem;
    public GameObject ConfirmChoiceWindow;
    private SaveFileInfo[] saves;

    public GameObject chooseDifficulty;
    private bool choosesDifficulty = false;

    private StartMenuScript target;

    private bool isNew = false;

    private void Awake()
    {
        this.gameObject.GetComponent<UIFader>().FadeIn(0.5f);
        Invoke("StopTime", 0.6f);
        RefreshBubbles();


    }

    public void SetTarget(StartMenuScript _target)
    {
        target = _target;
    }

    private void RefreshBubbles()
    {
        saves = new SaveFileInfo[3];
        for (int i = 0; i < 3; i++)
        {
            saves[i] = SaveSystem.LoadGame(i);
            if (saves[i] == null)
            {
                switch (i)
                {
                    case 1:
                        secondImg.enabled = false;
                        secondText.text = "?";
                        secondText.color = Color.gray;
                        secondPerc.text = "??%";
                        secondPerc.color = Color.gray;
                        Destroy(secondDelete);
                        break;
                    case 2:
                        thirdImg.enabled = false;
                        thirdText.text = "?";
                        thirdText.color = Color.gray;
                        thirdPerc.text = "??%";
                        thirdPerc.color = Color.gray;
                        Destroy(thirdDelete);

                        break;
                    default:
                        firstImg.enabled = false;
                        firstText.text = "?";
                        firstText.color = Color.gray;
                        firstPerc.text = "??%";
                        firstPerc.color = Color.gray;
                        Destroy(firstDelete);

                        break;
                }
            }
            else
            {
                switch (i)
                {
                    case 1:
                        secondPerc.text = saves[i].getPercentage() + "%";
                        break;
                    case 2:
                        thirdPerc.text = saves[i].getPercentage() + "%";
                        break;
                    default:
                        firstPerc.text = saves[i].getPercentage() + "%";
                        break;
                }
            }
        }
    }

    public void SelectFirst()
    {
        ActualSave.saveSlot = 0;
        HandleSelect();
    }

    public void SelectSecond()
    {
        ActualSave.saveSlot = 1;
        HandleSelect();
    }

    public void SelectThird()
    {
        ActualSave.saveSlot = 2;
        HandleSelect();
    }

    private void HandleSelect()
    {
        if (saves[ActualSave.saveSlot] == null)
        {
            eventSystem.enabled = false;
            ActualSave.actualSave = new SaveFileInfo();

            GameObject difficultyChoice = Instantiate(chooseDifficulty, this.transform);
            difficultyChoice.GetComponent<difficultySettingsScript>().isInitialDifficultyChoice();
            choosesDifficulty = true;
            
        }
        else
        {
            ActualSave.actualSave = saves[ActualSave.saveSlot];
            StartTime();
            this.gameObject.GetComponent<UIFader>().FadeOut(0.2f);
            Destroy(this.gameObject, 0.3f);
        }
        
    }

    public void deleteFirst()
    {
        ActualSave.saveSlot = 0;
        handleDelete();
    }

    public void deleteSecond()
    {
        ActualSave.saveSlot = 1;
        handleDelete();
    }

    public void deleteThird()
    {
        ActualSave.saveSlot = 2;
        handleDelete();
    }

    private void handleDelete()
    {
        if(saves[ActualSave.saveSlot] != null)
        {
            eventSystem.enabled = false;

            GameObject confirmPanel = Instantiate(ConfirmChoiceWindow, this.transform);
            confirmPanel.GetComponent<confirmDifficultyChoiceScript>().setStartText(2);
            
        }

    }

   


    public void hasConfirmedSettings()
    {
        eventSystem.enabled = true;
        //The player chose an empty slot
        if (choosesDifficulty)
        {
            SaveSystem.SaveGame(ActualSave.actualSave, ActualSave.saveSlot);
            StartTime();
            this.gameObject.GetComponent<UIFader>().FadeOut(0.2f);
            Destroy(this.gameObject, 0.3f);
            isNew = true;
        }//The player wanted to delete a save file
        else
        {
            SaveSystem.EraseGame(ActualSave.saveSlot);
            switch (ActualSave.saveSlot)
            {
                case 1:
                    eventSystem.SetSelectedGameObject(secondSave.GetComponentInChildren<buttonShowTextScript>().gameObject);
                    break;
                case 2:
                    eventSystem.SetSelectedGameObject(thirdSave.GetComponentInChildren<buttonShowTextScript>().gameObject);
                    break;
                default:
                    eventSystem.SetSelectedGameObject(firstSave.GetComponentInChildren<buttonShowTextScript>().gameObject);
                    break;
            }
            RefreshBubbles();
        }
        

    }

    public void hasCancelledSettings()
    {
        eventSystem.enabled = true;
    }

    public void StopTime()
    {
        Time.timeScale = 0f;

    }

    public void StartTime()
    {
        Time.timeScale = 1f;

    }


    private void OnDestroy()
    {

        if (target)
        {
            target.OutOfSave();
        }
        else if (this.transform.parent.GetComponent<menuControllerScript>())
        {
            this.transform.parent.GetComponent<menuControllerScript>().OutOfSave(isNew);
        }
         
    }

}
