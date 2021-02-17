using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SpellCommand : Command
{
    protected bool pressed = false;
    override public void execute(object value) {

        InputValue input = value as InputValue;

        pressed = (input.Get<float>() > 0.05f);
        //Debug.Log("SPELL : " + pressed);
    }

    public bool isPressed()
    {
        return pressed;
    }
}
