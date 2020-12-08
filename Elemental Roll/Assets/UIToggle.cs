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
    public Color normalCheckedColor;
    public Color selectedCheckedColor;
    public Color pressedCheckedColor;
    public Color checkedColor;
    public bool isChecked=false;
    public UIToggleGroup toggleGroup;

    override protected void Awake()
    {
        if(toggleGroup)toggleGroup.PopulateToggles(this);
        //Debug.Break();
        if (targetGraphic != null)
        {
            //baseImageColor = targetGraphic.color;
            if (isActive)
                targetGraphic.color = ((isChecked) ? normalCheckedColor * checkedColor : normalColor * baseImageColor * uncheckedColor);
            else
            {
                targetGraphic.color = disabledColor *((isChecked) ? checkedColor : baseImageColor * uncheckedColor);
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
                colorTransition( ((isChecked) ? selectedCheckedColor * checkedColor : selectedColor * uncheckedColor));
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

    protected override UIButton changeWhenSelected(int newState)
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
                colorTransition(((isChecked) ? normalCheckedColor * checkedColor : normalColor * uncheckedColor));
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

     override protected void launchSelected()
    {

    }

    override protected UIButton moveToNext(UIButton nextButton, int wantedState)
    {
        if (nextButton == null)
        {
            if (actualState != SELECTED)
            {
                actualState = SELECTED;
                colorTransition(((isChecked)? selectedCheckedColor * checkedColor : selectedColor * uncheckedColor));
            }
            return this;
        }
        UIButton nextSelected = nextButton.changeState(wantedState);
        if (nextSelected != null)
        {
            //A button has been selected, so we deselect ourselves and return the selected button
            colorTransition(((isChecked) ? normalCheckedColor * checkedColor : normalColor * uncheckedColor));
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
            else
            {
                Uncheck();
            }
        }

    }

    public void Check()
    {
        targetGraphic.sprite = checkedImage;

        isChecked = true;
        colorTransition(selectedCheckedColor * checkedColor);

        if (toggleGroup) toggleGroup.select(this);
        BaseEventData eventData = new BaseEventData(EventSystem.current);

        whenSelected.Invoke(eventData);
    }

    public void Uncheck()
    {

        targetGraphic.sprite = uncheckedImage;
        colorTransition(((actualState == SELECTED)? selectedColor : normalColor) * uncheckedColor);
        isChecked = false;
        if (toggleGroup) toggleGroup.select(null);
        BaseEventData eventData = new BaseEventData(EventSystem.current);

        whenDeselected.Invoke(eventData);
    }


    override protected void colorTransition(Color toColor)
    {

        if (targetGraphic != null)
        {
            if (Time.timeScale >= 0.1f)
            {
                LeanTween.value(targetGraphic.gameObject, targetGraphic.color, toColor, transitionSpeed).setOnUpdate((Color _value) =>
                {
                        targetGraphic.color = _value * baseImageColor;

                });
            }
            else
            {
                targetGraphic.color = toColor * baseImageColor;

            }
        }
    }

}
