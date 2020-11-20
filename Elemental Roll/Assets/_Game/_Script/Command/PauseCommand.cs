﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PauseCommand : Command
{
    private bool pressed = false;
    override public void execute(object value) {
        InputValue input = value as InputValue;
        pressed = input.isPressed;
        //Debug.Log("PAUSE : " + pressed);
    }

    public bool isPressed()
    {
        return pressed;
    }
}
