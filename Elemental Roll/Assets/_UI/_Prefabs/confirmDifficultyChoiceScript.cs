using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class confirmDifficultyChoiceScript : MonoBehaviour
{

    public TMP_Text normalText;
    public TMP_Text hardcoreText;
    public TMP_Text saveText;


    public void Confirm()
    {
        if (this.GetComponentInParent<difficultySettingsScript>() != null)
            this.GetComponentInParent<difficultySettingsScript>().hasConfirmedSettings();
        else if (this.GetComponentInParent<StartMenuScript>() != null)
            this.GetComponentInParent<StartMenuScript>().hasConfirmedSettings();
        else if (this.GetComponentInParent<chooseSaveScript>() != null)
            this.GetComponentInParent<chooseSaveScript>().hasConfirmedSettings();
        Destroy(this.gameObject);

    }


    public void Cancel()
    {
        Debug.Log("Cancel");
        if (this.GetComponentInParent<difficultySettingsScript>() != null)
            this.GetComponentInParent<difficultySettingsScript>().hasCancelledSettings();
        else if (this.GetComponentInParent<StartMenuScript>() != null)
            this.GetComponentInParent<StartMenuScript>().hasCancelledSettings();
        else if(this.GetComponentInParent<chooseSaveScript>() !=null)
            this.GetComponentInParent<chooseSaveScript>().hasCancelledSettings();

        Destroy(this.gameObject);
    }

    public void setStartText(int normal = 1)
    {
        if (normal == 1)
        {
            normalText.enabled = true;
            hardcoreText.enabled = false;
            saveText.enabled= false;
        }
        else if(normal == 0)
        {
            normalText.enabled = false;
            hardcoreText.enabled = true;
            saveText.enabled = false;
        }
        else
        {
            normalText.enabled = false;
            hardcoreText.enabled = false;
            saveText.enabled = true;
        }
    }
}
