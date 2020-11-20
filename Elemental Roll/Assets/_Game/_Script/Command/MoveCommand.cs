﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MoveCommand : Command
{
    private Vector2 movements;
    override public void execute(object value) {
        InputValue input = value as InputValue;
        movements.x = input.Get<Vector2>().x;
        movements.y = input.Get<Vector2>().y;
        //Debug.Log("MOVE : " + movements);
    }

    public Vector2 getMove()
    {
        return movements;
    }
}
