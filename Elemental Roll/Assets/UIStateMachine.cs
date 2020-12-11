using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIStateMachine : Observer
{
    public UIButton firstSelected;
    private GameObject persistantHandler;

    public UIButton onReturn;

    private void Awake()
    {
        
        persistantHandler = GameObject.FindGameObjectsWithTag("PersistentObject")[0];
        persistantHandler.GetComponent<InputHandler>().addObserver(this);

        firstSelected.changeState(UIButton.SELECTED);

    }



    public void testButtonAction(BaseEventData data)
    {
        Debug.Log("Event received from : " + data.selectedObject.name);
    }

    public void changeActiveObject(UIButton newActiveObject)
    {
        if (newActiveObject != null)
        {
            UIButton value = newActiveObject.changeState(UIButton.SELECTED);
            if (value != null)
            {
                firstSelected.changeState(UIButton.UNSELECTED);
                firstSelected = newActiveObject;
            }

        }
    }

    override public void OnNotify(GameObject entity, object notifiedEvent)
    {
        switch (notifiedEvent.GetType().ToString())
        {
            case "MoveCommand":
                if (((MoveCommand)notifiedEvent).isMenuController)
                {
                    OnMove(((MoveCommand)notifiedEvent).getMove());
                }
                break;
            case "SpellCommand":
                Debug.Log("Spell is pressed : " + ((SpellCommand)notifiedEvent).isPressed());
                    if (((SpellCommand)notifiedEvent).isPressed())// && !waitingforButtontoUnpress)
                    {
                        Debug.Log("Spell pressed");
                            firstSelected.ExecuteFunction();
                    }
                break;
            case "RestartCommand":
                if (onReturn && ((RestartCommand)notifiedEvent).isPressed())
                {
                    switch (onReturn.GetType().ToString()) {
                        case "UIButton":
                            onReturn.ExecuteFunction();
                            break;
                        case "UIDropDown":
                            (onReturn as UIDropDown).getBack();
                            break;
                    }
                }
                break;
            case "PauseCommand":
                //OnPause(((RestartCommand)notifiedEvent).isPressed());
                break;
            case "EagleViewCommand":
                //OnPause(((EagleViewCommand)notifiedEvent).isPressed());
                break;
            case "TopViewCommand":
                //OnPause(((TopViewCommand)notifiedEvent).isPressed());
                break;
            default:
                break;
        }
    }

    public void OnMove(Vector2 value)
    {
        if (value.y > 0)
        {
            moveInDirection(UIButton.GOUP);
        }else if (value.y < 0)
        {
            moveInDirection(UIButton.GODOWN);
        }else if (value.x > 0)
        {
            moveInDirection(UIButton.GORIGHT);
        }
        else if (value.x < 0)
        {
            moveInDirection(UIButton.GOLEFT);
        }
    }

    private void moveInDirection(int direction)
    {
        UIButton button = firstSelected.changeState(direction);
        if (button != null)
        {
            firstSelected = button;
        }
    }


}
