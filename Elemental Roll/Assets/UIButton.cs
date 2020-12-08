using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class UIButton : MonoBehaviour
{

    public bool isActive = true;
    public const int SELECTED = 1;
    public const int UNSELECTED = 0;
    public const int GOLEFT = 2;
    public const int GORIGHT = 3;
    public const int GOUP = 4;
    public const int GODOWN = 5;
    public int actualState = 0;

    public UIButton leftButton;
    public UIButton rightButton;
    public UIButton upButton;
    public UIButton downButton;

    public Color baseImageColor;


    public EventTrigger.TriggerEvent customCallback;

    public EventTrigger.TriggerEvent whenSelected;

    public EventTrigger.TriggerEvent whenDeselected;

    virtual public void ExecuteFunction()
    {
        if (isActive)
        {
            BaseEventData eventData = new BaseEventData(EventSystem.current);


            //colorTransition(pressedColor);
            customCallback.Invoke(eventData);
            InvokeRealTime("GoBackToSelectedColor", 0.11f);
        }
    }

    protected void GoBackToSelectedColor()
    {
        switch (actualState)
        {
            case SELECTED:
                colorTransition(selectedColor);
                break;
            default:
                colorTransition(normalColor);
                break;
        }
    }

    public Image targetGraphic;

    public Color normalColor=Color.white;
    public Color pressedColor = Color.white;
    public Color selectedColor = Color.white;
    public Color disabledColor = Color.white;

    public float transitionSpeed = 0.1f;

    virtual protected void Awake()
    {
        if (targetGraphic != null)
        {
            baseImageColor = targetGraphic.color;
            if (isActive)
                targetGraphic.color = normalColor * baseImageColor;
            else
            {
                targetGraphic.color = disabledColor * baseImageColor;
            }
        }
    }


    /*
     * Return value : 
     * null = problem, the caller should cancel any action
     * UIButton = returns the last object target by this function
     * 
     */

    virtual public UIButton changeState(int newState)
    {


        switch (actualState)
            {
                case SELECTED:
                    return changeWhenSelected(newState);
                case UNSELECTED:
                    return changeWhenUnselected(newState);
                default:
                    return null;
            }
    }

    virtual protected UIButton changeWhenSelected(int newState)
    {
        if (!isActive)
        { //If the button is inactive and selected, problem
            return null;
        }
        switch (newState)
        {
            case SELECTED: //It is already selected, so there should be no problem, just OK
                return this;
            case UNSELECTED: //Not a normal case, but if a script asks a button to unselected, we should be OK
                //LeanTween. (targetGraphic, normalColor, transitionSpeed);
                colorTransition(normalColor);
                actualState = newState;
                launchUnselected();
                return this;
            case GOLEFT:
                return moveToNext(leftButton, GOLEFT);
            case GORIGHT:
                return moveToNext(rightButton, GORIGHT);
            case GOUP:
                return moveToNext(upButton, GOUP);
            case GODOWN:
                return moveToNext(downButton, GODOWN);
        }
        return null;
    }

    protected virtual void launchUnselected()
    {
        if (whenDeselected != null)
        {
            BaseEventData eventData = new BaseEventData(EventSystem.current);


            //We invoke the possible function if it is selected
            whenDeselected.Invoke(eventData);
        }
    }

    virtual protected UIButton moveToNext(UIButton nextButton, int wantedState)
    {
        if(nextButton == null)
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
            launchUnselected();
            return nextSelected;
        }
        return this;
    }

    protected UIButton changeWhenUnselected(int newState)
    {
        switch (newState)
        {
            case SELECTED:
                launchSelected();

                colorTransition(selectedColor);
                actualState = newState;
                return this;
            case UNSELECTED://It is already unselected, so there should be no problem, just OK
                return this;
            case GOLEFT:
                return moveToNextWhenUnselected(leftButton, GOLEFT);
            case GORIGHT:
                return moveToNextWhenUnselected(rightButton, GORIGHT);
            case GOUP:
                return moveToNextWhenUnselected(upButton, GOUP);
            case GODOWN:
                return moveToNextWhenUnselected(downButton, GODOWN);
        }
        return null;
    }


    protected virtual void launchSelected()
    {
        if (whenSelected != null)
        {
            BaseEventData eventData = new BaseEventData(EventSystem.current);


            //We invoke the possible function if it is selected
            whenSelected.Invoke(eventData);
        }
    }

    protected UIButton moveToNextWhenUnselected(UIButton nextButton, int wantedState)
    {
        if (!isActive) //If we are inactive, we skip
        {
            return nextButton.changeState(wantedState);
        }
        else//If we are active and unselected, we select ourselves
        {
            colorTransition(selectedColor);
            actualState = SELECTED;
            launchSelected();
            return this;
        }
    }


    protected virtual void colorTransition(Color toColor)
    {
        if (targetGraphic != null)
        {
            if (Time.timeScale >= 0.1f)
            {
                LeanTween.value(targetGraphic.gameObject, targetGraphic.color, toColor, transitionSpeed).setOnUpdate((Color _value) =>
                {
                    targetGraphic.color = _value*baseImageColor;
                });
            }
            else
            {
                targetGraphic.color = toColor*baseImageColor;
            }
        }
    }


    public void activate(bool val)
    {
        isActive = val;
        colorTransition((val) ? normalColor : disabledColor);
    }



    public void InvokeRealTime(string functionName, float delay)
    {
        StartCoroutine(InvokeRealTimeHelper(functionName, delay));
    }

    private IEnumerator InvokeRealTimeHelper(string functionName, float delay)
    {
        float timeElapsed = 0f;
        while (timeElapsed < delay)
        {
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        SendMessage(functionName);
    }
}
