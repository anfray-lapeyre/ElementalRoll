using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerProgressBarScript : MonoBehaviour
{
    public FloatVariable maxPowerTime;
    public FloatVariable powerTime;
    private Image img;
    private float currentFill = 0f;
    private bool hasReachedTop=true;
    private Color startColor;
    private Color currentColor;
    private Color baseColor;
    
    // Start is called before the first frame update
    void Start()
    {
        img = this.GetComponent<Image>();
        currentFill = powerTime.value;
        startColor = img.color;
        currentColor = startColor;
        baseColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
        if(powerTime.value>= maxPowerTime.value && hasReachedTop == false)
        {
            hasReachedTop = true;
            this.transform.LeanScale(Vector3.one * 1.1f, 0.1f).setLoopPingPong(1).setEaseInOutExpo();

        }else if(powerTime.value < maxPowerTime.value)
        {
                hasReachedTop = false;
        }
    }

    void GetCurrentFill()
    {
        float fill = powerTime.value / maxPowerTime.value;
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
