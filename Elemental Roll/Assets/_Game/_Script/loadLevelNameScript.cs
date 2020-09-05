using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class loadLevelNameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        TextMeshProUGUI thisText = this.GetComponent<TextMeshProUGUI>();
        if (thisText)
        {
            if (CrossLevelInfo.LevelName != null)
                thisText.text = CrossLevelInfo.LevelName;
            else
                thisText.text = "Little Melody";
        }
    }

}
