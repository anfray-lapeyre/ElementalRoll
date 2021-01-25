using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class chooseLanguageScript : MonoBehaviour
{
    public Image flagImage;

    public UIDropDown dropDown;

    private StartMenuScript target;

    private GameObject persistantHandler;

    private LanguageContainer allLanguages;

    private int language;

    private void Awake()
    {
        
        persistantHandler = GameObject.FindGameObjectsWithTag("PersistentObject")[0];
        //persistantHandler.GetComponent<InputHandler>().addObserver(this);
        this.gameObject.GetComponent<UIFader>().FadeIn(0.3f);
        Invoke("StopTime", 0.4f);
        //RefreshBubbles();

        LanguageFileInfo languageSave = SaveSystem.LoadLanguage();
        if(languageSave == null)
        {
            languageSave = new LanguageFileInfo();
        }
        Debug.Log("language chargé : "+ languageSave.chosenLanguage.name);

        //We load all the possible languages
        string loadedJsonFile = Resources.Load<TextAsset>("languages").text;
        allLanguages = JsonUtility.FromJson<LanguageContainer>(loadedJsonFile);
        //When we get to the one with te right code, we keep the index to be able to access the info easily
        for(int i = 0; i< allLanguages.languages.Length; i++)
        {
            Debug.Log(allLanguages.languages[i].code);
            if(allLanguages.languages[i].code == languageSave.chosenLanguage.name)
            {
                Debug.Log("This is the right language");
                language = i;
                //We store in an environment variable the adress of the right dialogues
                languageSave.chosenLanguage.name = allLanguages.languages[i].adress;

                ActualLanguage.actualLanguage = languageSave.Clone();
            }
        }

        //We then fill our dropdown with every language
        dropDown.options.Clear();
        for (int i = 0; i < allLanguages.languages.Length; i++)
        {
            dropDown.options.Add(allLanguages.languages[i].name);
        }


        HandleSelect();
    }

    public void SetTarget(StartMenuScript _target)
    {
        target = _target;
    }

   

    public void HandleSelect()
    {
        language = dropDown.value;
        flagImage.sprite = Resources.Load<Sprite>("Flags/"+allLanguages.languages[language].image);
        ActualLanguage.actualLanguage.chosenLanguage.name = allLanguages.languages[language].adress;
        RefreshAllText();
    }

    private void RefreshAllText()
    {
        translateTextScript[] foundTextObjects = FindObjectsOfType(typeof(translateTextScript)) as translateTextScript[];
        for (int i = 0; i < foundTextObjects.Length; i++)
        {
            foundTextObjects[i].RefreshDialog();
        }
    }

    public void StopTime()
    {
        Time.timeScale = 0f;

    }

    public void StartTime()
    {
        Time.timeScale = 1f;

    }

    public void hasConfirmedSettings()
    {
        Debug.Log("Confirmed !");
        //We save language
        SaveSystem.SaveLanguage(allLanguages.languages[language].adress);
        //And then destroy the window
        Destroy(this.gameObject);
    }


        private void OnDestroy()
    {
        StartTime();
        if (target)
        {
            target.OutOfLanguage();
        }
        else if (this.transform.parent.GetComponent<menuControllerScript>())
        {
            this.transform.parent.GetComponent<menuControllerScript>().OutOfLanguage();
        }
         
    }

}
