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

    public LanguageFileInfo Clone()
    {
        LanguageFileInfo clone = new LanguageFileInfo();
        clone.chosenLanguage = new LanguageSaveFormat();
        if(chosenLanguage != null)
            clone.chosenLanguage.name = chosenLanguage.name;
        return clone;
    }

    public void Verbose()
    {
        Debug.Log("Language save is set to : " + chosenLanguage.name);
    }

}