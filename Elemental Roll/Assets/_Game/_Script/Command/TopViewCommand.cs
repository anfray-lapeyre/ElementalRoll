using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TopViewCommand : Command
{
    private bool pressed = false;
    override public void execute(object value) {
        InputValue input = value as InputValue;
        pressed = input.isPressed;
        //Debug.Log("TOPVIEW : "+pressed);
    }

    public bool isPressed()
    {
        return pressed;
    }
}
