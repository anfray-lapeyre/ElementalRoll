using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlider : UIButton
{
    private RectTransform handle;
    public bool TopDown=true;
    public int value=0; // Step where the slider starts
    public float size = 0.1f;
    public int nbSteps = 20; //Nb of steps to go from one side to another, no limit

    public RectTransform scrollContainer;
    public RectTransform contentToScroll;
    private float startPosition;

    public bool isSliderFromLevelSelection = false;

    // Start is called before the first frame update
    void Start()
    {
        handle = targetGraphic.gameObject.GetComponent<RectTransform>();
        handle.sizeDelta = new Vector2(handle.sizeDelta.x,this.GetComponent<RectTransform>().sizeDelta.y * size);
        handle.anchoredPosition = new Vector2(handle.anchoredPosition.x,-handle.sizeDelta.y / 2);
        contentToScroll.anchoredPosition = new Vector2(contentToScroll.anchoredPosition.x, -contentToScroll.sizeDelta.y / 2);
    }

    public void Refresh()
    {
        handle = targetGraphic.gameObject.GetComponent<RectTransform>();
        handle.sizeDelta = new Vector2(handle.sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y * size);
        handle.anchoredPosition = new Vector2(handle.anchoredPosition.x, Mathf.Max(-this.GetComponent<RectTransform>().sizeDelta.y + handle.sizeDelta.y / 2,Mathf.Min(-handle.sizeDelta.y / 2 , -handle.sizeDelta.y / 2 - ( this.GetComponent<RectTransform>().sizeDelta.y / (nbSteps+3))*value)));
        contentToScroll.anchoredPosition = new Vector2(contentToScroll.anchoredPosition.x, Mathf.Max(-contentToScroll.sizeDelta.y / 2f, Mathf.Min(contentToScroll.sizeDelta.y / 2 - scrollContainer.sizeDelta.y, -contentToScroll.sizeDelta.y / 2 + ((contentToScroll.sizeDelta.y - scrollContainer.sizeDelta.y) / (nbSteps - 1))*value)));
    }

    override protected UIButton moveToNext(UIButton nextButton, int wantedState)
    {
        if (TopDown)
        {
            switch (wantedState)
            {
                case GOUP:
                    value = Mathf.Max(0, value - 1);
                    Refresh();
                    break;
                case GODOWN:
                    value = Mathf.Min(nbSteps-1, value + 1);

                    Refresh();

                    break;
                case GOLEFT:
                    if (isSliderFromLevelSelection)
                    {
                        InvokeRealTime("leaveSelected", 0.01f);
                        leftButton = transform.parent.GetComponent<LevelSelectionScript>().getButtonForSlider();
                        colorTransition(normalColor);
            actualState = UNSELECTED;
                        return transform.parent.GetComponent<LevelSelectionScript>().getButtonForSlider();


                    }
                    break;
            }
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
            Debug.Log(actualState);
            colorTransition(normalColor);
            actualState = UNSELECTED;
            return nextSelected;
        }
        return this;
    }


    public void leaveSelected()
    {
        transform.parent.GetComponent<UIStateMachine>().OnMove(Vector2.left);
    }
}
