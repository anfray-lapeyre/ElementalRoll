using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class setMaxBonusNumberScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        TextMeshProUGUI thisText = this.GetComponent<TextMeshProUGUI>();
        if (thisText)
        {
            
            int count = CrossLevelInfo.maxSlimes;
            thisText.text = "/ " + count;
        }
    }

    


}
