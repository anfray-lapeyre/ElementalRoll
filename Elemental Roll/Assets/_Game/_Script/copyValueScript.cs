using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class copyValueScript : MonoBehaviour
{
    public TextMeshProUGUI Text;
    private TextMeshProUGUI thisText;
    public bool AlwaysUpdate;

    // Start is called before the first frame update
    void Start()
    {
        thisText = this.GetComponent<TextMeshProUGUI>();
        if(thisText)
            thisText.text = Text.text;
    }

    // Update is called once per frame
    void Update()
    {
        if(AlwaysUpdate && thisText)
        {
            thisText.text = Text.text;
        }
    }
}
