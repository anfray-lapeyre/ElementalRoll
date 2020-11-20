using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using TMPro;


public class buttonShowTextScript : MonoBehaviour
{
    public TMP_Text levelName;
    public TMP_Text levelNbText;
    private int textAnimation;
    public levelBubbleHandlerScript parent;

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        fade(Color.clear, Color.white);
        textAnimation = LeanTween.scale(levelNbText.gameObject, Vector3.one * 1.2f, 1f).setEaseInOutQuad().setLoopPingPong().id;
        if (parent != null)
        {
            parent.askForHandlePreview();
        }
    }

    //Do this when the selectable UI object is selected.
    public void OnDeselect(BaseEventData eventData)
    {
        fade(Color.white, Color.clear);
        LeanTween.pause(textAnimation);
    }

    void fade(Color initialColor, Color goalColor, float time=0.5f)
    {
        LeanTween.value(gameObject, updateValueExampleCallback, initialColor, goalColor, time);
    }

    void updateValueExampleCallback(Color val)
    {
        if (levelName != null)
        {
            levelName.color = val;
        }
    }

    public void OnDisable()
    {
        fade(Color.white, Color.clear);
        LeanTween.pause(textAnimation);
    }

}

