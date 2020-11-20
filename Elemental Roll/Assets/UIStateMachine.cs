using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIStateMachine : Observer
{
    public UIButton firstSelected;
    private GameObject persistantHandler;

    public bool mustWait = false;

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

                OnMove(((MoveCommand)notifiedEvent).getMove());
                break;
            case "SpellCommand":
                    
                if (((SpellCommand)notifiedEvent).isPressed())// && !waitingforButtontoUnpress)
                    {
                    if(!mustWait)
                        firstSelected.ExecuteFunction();
                }
                else
                {
                    if (mustWait)
                        mustWait = false;
                }
                break;
            case "RestartCommand":
                //OnRestart(((RestartCommand)notifiedEvent).isPressed());
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
