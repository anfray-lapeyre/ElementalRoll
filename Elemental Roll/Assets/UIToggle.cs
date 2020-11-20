using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIToggle : UIButton
{

    public Sprite uncheckedImage;
    public Sprite checkedImage;
    public Color uncheckedColor;
    public Color checkedColor;
    public bool isChecked=false;
    public UIToggleGroup toggleGroup;

    override protected void Awake()
    {
        toggleGroup.PopulateToggles(this);
        //Debug.Break();
        if (targetGraphic != null)
        {
            //baseImageColor = targetGraphic.color;
            if (isActive)
                targetGraphic.color = normalColor *baseImageColor* ((isChecked) ? checkedColor : uncheckedColor);
            else
            {
                targetGraphic.color = disabledColor * baseImageColor * ((isChecked) ? checkedColor : uncheckedColor);
            }
            if (isChecked)
            {
                Check();
            }
            else
            {
                targetGraphic.sprite = uncheckedImage;
            }
        }



    }
    protected new UIButton changeWhenUnselected(int newState)
    {

        if (!isActive)
        { //If the button is inactive and selected, problem
            return null;
        }

        switch (newState)
        {
            case SELECTED:
                if (toggleGroup != null)
                {
                    toggleGroup.select(this);
                }
                colorTransition(selectedColor * ((isChecked) ? checkedColor : uncheckedColor));
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
  

        override public UIButton changeState(int newState)
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

    protected new UIButton changeWhenSelected(int newState)
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
                colorTransition(normalColor * ((isChecked) ? checkedColor : uncheckedColor));
                actualState = newState;
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


    override protected UIButton moveToNext(UIButton nextButton, int wantedState)
    {
        if (nextButton == null)
        {
            if (actualState != SELECTED)
            {
                actualState = SELECTED;
                colorTransition(selectedColor * ((isChecked)? checkedColor : uncheckedColor));
            }
            return this;
        }
        UIButton nextSelected = nextButton.changeState(wantedState);
        if (nextSelected != null)
        {
            //A button has been selected, so we deselect ourselves and return the selected button
            colorTransition(normalColor * ((isChecked) ? checkedColor : uncheckedColor));
            actualState = UNSELECTED;
            return nextSelected;
        }
        return this;
    }


    override public void ExecuteFunction()
    {
        if (isActive)
        {
            if (!isChecked)
            {
                Check();
            }
        }

    }

    public void Check()
    {
        targetGraphic.sprite = checkedImage;

        isChecked = true;
        colorTransition(selectedColor * checkedColor);

        toggleGroup.select(this);
        BaseEventData eventData = new BaseEventData(EventSystem.current);

        customCallback.Invoke(eventData);
        Debug.Log("Checked");
    }

    public void Uncheck()
    {
        Debug.Log("Unchecked");

        targetGraphic.sprite = uncheckedImage;
        colorTransition(normalColor * uncheckedColor);
        isChecked = false;
    }

}
