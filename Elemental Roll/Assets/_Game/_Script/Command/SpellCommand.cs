using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SpellCommand : Command
{
    private bool pressed = false;
    override public void execute(object value) {
        InputValue input = value as InputValue;
        pressed = input.isPressed;
        //Debug.Log("SPELL : " + pressed);
    }

    public bool isPressed()
    {
        return pressed;
    }
}
