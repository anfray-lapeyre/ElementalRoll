using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class translateTextScript : MonoBehaviour
{

    public int ID;
    public bool isUGUI = true;
    // Start is called before the first frame update
    void Start()
    {
        RefreshDialog();
    }

    public void RefreshDialog()
    {
        if (ActualLanguage.actualLanguage != null)
        { 
            int index = 0;
       
            string loadedJsonFile = Resources.Load<TextAsset>(ActualLanguage.actualLanguage.chosenLanguage.name).text;
            DialogueContainer dialoguesInJson = JsonUtility.FromJson<DialogueContainer>(loadedJsonFile);
            for (int i = 0; i < dialoguesInJson.dialogues.Length; i++)
            {
                if (dialoguesInJson.dialogues[i].id == ID)
                    index = i;
            }

            if(isUGUI)
                this.GetComponent<TMPro.TextMeshProUGUI>().text = dialoguesInJson.dialogues[index].lines[0];
            else
                this.GetComponent<TMPro.TextMeshPro>().text = dialoguesInJson.dialogues[index].lines[0];

        }
    }

}
