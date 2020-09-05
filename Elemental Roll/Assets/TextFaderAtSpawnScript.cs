using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFaderAtSpawnScript : MonoBehaviour
{
    private TMP_Text text;
    private bool inFading = false;

    public bool isFading()
    {
        return inFading;
    }

    private void stopFading()
    {
        inFading = false;
    }
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        fade(0f,1f, 0.5f);
    }

    private void fade(float fromAlphaValue=0f, float toAlphaValue=1f, float duration=0.5f)
    {
        var color = text.color;
        var fadeoutcolor = color;
        color.a = toAlphaValue;
        fadeoutcolor.a = fromAlphaValue;
        LeanTween.value(gameObject, updateValueExampleCallback, fadeoutcolor, color, duration).setEase(LeanTweenType.easeInOutCubic);
        inFading = true;
        Invoke("stopFading", duration);
    }



    private void updateValueExampleCallback(Color val)
    {
        text.color = val;
    }

    public void Kill()
    {
        Invoke("doKill", 0.1f);
    }

    private void doKill()
    {
        fade(1f,0f, 0.5f);
        Destroy(this.gameObject, 0.6f);
    }
}
