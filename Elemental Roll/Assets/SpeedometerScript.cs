using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedometerScript : MonoBehaviour
{
    public int playerNb=0;
    public bool mustUpdate = true;
    private TMPro.TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        text.text = (int)ActualSave.actualSave.stats[playerNb].playerSpeed + "";
    }

    // Update is called once per frame
    void Update()
    {
        if (mustUpdate)
        {
            text.text = (int)ActualSave.actualSave.stats[playerNb].playerSpeed + "";

        }
    }
}
