using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerProgressBarScript : MonoBehaviour
{
    public int powerNb=0;
    public swapCharacter playerData;
    private Image img;
    private float currentFill = 1f;
    private bool hasReachedTop=true;
    private Color startColor;
    private Color currentColor;
    private Color baseColor;
    private int playerNb;
    private int characterNb;
    
    // Start is called before the first frame update
    void Start()
    {
        img = this.GetComponent<Image>();
        currentFill = 1f;
        startColor = img.color;
        currentColor = startColor;
        baseColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        playerNb = playerData.getPlayerNb();
        characterNb = ActualSave.actualSave.stats[playerNb].activePlayer;
        GetCurrentFill();

        if (ActualSave.actualSave.stats[playerNb].powerTime[characterNb][powerNb] >= ActualSave.actualSave.stats[playerNb].maxPowerTime[characterNb][powerNb] && hasReachedTop == false)
        {
            hasReachedTop = true;
            this.transform.LeanScale(Vector3.one * 1.1f, 0.1f).setLoopPingPong(1).setEaseInOutExpo();

        }else if(ActualSave.actualSave.stats[playerNb].powerTime[characterNb][powerNb] < ActualSave.actualSave.stats[playerNb].maxPowerTime[characterNb][powerNb])
        {
                hasReachedTop = false;
        }
    }

    void GetCurrentFill()
    {

        float fill = ActualSave.actualSave.stats[playerNb].powerTime[characterNb][powerNb] / ActualSave.actualSave.stats[playerNb].maxPowerTime[characterNb][powerNb];

        currentFill = Vector3.Lerp(new Vector3(currentFill, 0, 0), new Vector3(fill, 0, 0), Time.deltaTime*5f).x;
        if (fill < 0.1f)
        {
            currentColor = baseColor;
        }
        else
        {
            currentColor.r = startColor.r*fill + baseColor.r*(1-fill);
            currentColor.g = startColor.g * fill + baseColor.g * (1 - fill);
            currentColor.b = startColor.b * fill + baseColor.b * (1 - fill);
        }
        img.fillAmount = currentFill;
        img.color = currentColor;
    }
}
