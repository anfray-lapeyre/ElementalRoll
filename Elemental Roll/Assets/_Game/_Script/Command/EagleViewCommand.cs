using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EagleViewCommand : Command
{
    private bool pressed = false;

    override public void execute(object value) {
        InputValue input = value as InputValue;
        pressed = input.isPressed;
        //Debug.Log("Eagle View : " + pressed);
    }

    public bool isPressed()
    {
        return pressed;
    }
}
