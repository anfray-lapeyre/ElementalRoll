using System;
using UnityEngine;

[System.Serializable]
public class LanguageFileInfo
{
    public LanguageSaveFormat chosenLanguage;


    //New save
    public LanguageFileInfo(string _chosenLanguage)
    {
        chosenLanguage = new LanguageSaveFormat();
        chosenLanguage.name = _chosenLanguage;
    }

    //Default new save
    public LanguageFileInfo()
    {
        chosenLanguage = new LanguageSaveFormat();
        chosenLanguage.name = "en_US";
    }


    //Existing save
    public LanguageFileInfo(LanguageSaveFormat _chosenLanguage)
    {

        chosenLanguage = _chosenLanguage;
    }

    public void Verbose(int count)
    {
        Debug.Log("Language save is set to : " + chosenLanguage.name);
    }

}