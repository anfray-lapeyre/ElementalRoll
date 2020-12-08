using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIValueSlider : UIButton
{
    private RectTransform handle;
    public RectTransform fill;

    public float minValue = 0;
    public float maxValue = 100;

    public float value =0;

    private int nbSteps = 10;


    void Start()
    {
        handle = targetGraphic.gameObject.GetComponent<RectTransform>();
        float valueAsPercentage = (maxValue - value) / (maxValue - minValue);
        fill.sizeDelta = new Vector2((this.GetComponent<RectTransform>().sizeDelta.x - handle.sizeDelta.x )* (1f-valueAsPercentage) ,fill.sizeDelta.y);
        fill.anchoredPosition = new Vector2(((this.GetComponent<RectTransform>().sizeDelta.x - handle.sizeDelta.x) * (1f-valueAsPercentage))/2f, fill.anchoredPosition.y);
        handle.anchoredPosition = new Vector2(((this.GetComponent<RectTransform>().sizeDelta.x - handle.sizeDelta.x) * (1f-valueAsPercentage)) , handle.anchoredPosition.y);
        leftButton = null;
        rightButton = null;
    }



    override protected UIButton moveToNext(UIButton nextButton, int wantedState)
    {
            switch (wantedState)
            {
                case GOLEFT:
                    value = Mathf.Max(minValue, value - (maxValue-minValue)/((float)nbSteps));
                    BaseEventData eventData = new BaseEventData(EventSystem.current);
                    customCallback.Invoke(eventData);
                    Refresh();
                    break;
                case GORIGHT:
                    value = Mathf.Min(maxValue, value + (maxValue - minValue) / ((float)nbSteps));
                    BaseEventData eventData2 = new BaseEventData(EventSystem.current);
                    customCallback.Invoke(eventData2);
                    Refresh();

                    break;

            }



        if (nextButton == null)
        {
            if (actualState != SELECTED)
            {
                actualState = SELECTED;
                colorTransition(selectedColor);
            }
            return this;
        }

        UIButton nextSelected = nextButton.changeState(wantedState);

        if (nextSelected != null)
        {
            //A button has been selected, so we deselect ourselves and return the selected button
            colorTransition(normalColor);
            actualState = UNSELECTED;
            return nextSelected;
        }
        return this;
    }


    public void Refresh()
    {
        handle = targetGraphic.gameObject.GetComponent<RectTransform>();
        float valueAsPercentage = (maxValue - value) / (maxValue - minValue);

        fill.sizeDelta = new Vector2((this.GetComponent<RectTransform>().sizeDelta.x - handle.sizeDelta.x) * (1f - valueAsPercentage), fill.sizeDelta.y);
        fill.anchoredPosition = new Vector2(((this.GetComponent<RectTransform>().sizeDelta.x - handle.sizeDelta.x) * (1f - valueAsPercentage)) / 2f, fill.anchoredPosition.y);
        handle.anchoredPosition = new Vector2(((this.GetComponent<RectTransform>().sizeDelta.x - handle.sizeDelta.x) * (1f - valueAsPercentage)), handle.anchoredPosition.y);
    }

}
