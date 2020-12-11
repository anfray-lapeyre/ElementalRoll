using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MoveCommand : Command
{
    private Vector2 movements;
    private bool isMenuJoystick = false;
    public bool isMenuController = false;
    

    override public void execute(object value) {
        InputValue input = value as InputValue;
        movements.x = input.Get<Vector2>().x;
        movements.y = input.Get<Vector2>().y;
        if (isMenuJoystick)
        {
            movements.x = (Mathf.Abs(movements.x) >= 0.5f) ? 1 * Mathf.Sign(movements.x) : 0;
            movements.y = (Mathf.Abs(movements.y) >= 0.5f) ? 1 * Mathf.Sign(movements.y) : 0;
        }

       
       // Debug.Log(input.Get<Vector2>());
        
    }

    public void setJoystickMenu(bool value)
    {
        isMenuJoystick = value;
    }

    public Vector2 getMove()
    {
        return movements;
    }

    public bool isMoving()
    {
        return Mathf.Abs(movements.x) >= 0.05f || Mathf.Abs(movements.y) >= 0.05f;
    }

    public bool isNotJoystick()
    {
        return !isMenuJoystick;
    }

    public void executeVertical(InputValue value)
    {
        Debug.Log("MoveVertical : " + value.Get<float>());

        //value.Get<float>()
        movements.y = value.Get<float>();
    }

    public void executeHorizontal(InputValue value)
    {
        Debug.Log("MoveHorizontal : " + value.Get<float>());

        movements.x = value.Get<float>();

    }
}
