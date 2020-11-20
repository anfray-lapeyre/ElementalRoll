using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : Subject
{
    //Liste des commandes -> 6 commandes
    //Restart -> CommandB
    Command buttonB;
    //UseSpell -> CommandA
    Command buttonA;
    //TopView -> CommandX
    Command buttonX;
    //EagleView->CommandY
    Command buttonY;
    //Move -> CommandLeftStick
    Command leftStick;
    //Pause -> CommandStart
    Command buttonStart;


    override protected void Awake()
    {
        base.Awake();

        buttonStart = new PauseCommand();
        leftStick = new MoveCommand();
        buttonA = new SpellCommand();
        buttonB = new RestartCommand();
        buttonX = new TopViewCommand();
        buttonY = new EagleViewCommand();
    }


    public void OnRestart(InputValue value) // CommandB
    {
            buttonB.execute(value);
            Notify(buttonB);
        
    }

    public void OnSpecialAction(InputValue value) //CommandA
    {
            buttonA.execute(value);
            Notify(buttonA);
        
    }

    public void OnMove(InputValue value) //CommandLeftStick
    {
            leftStick.execute(value);
            Notify(leftStick);
        
    }

    public void OnPause(InputValue value) //CommandStart
    {
            buttonStart.execute(value);
            Notify(buttonStart);
        
    }

    public void OnTopView(InputValue value) //CommandX
    {
            buttonX.execute(value);
            Notify(buttonX);
        
    }

    public void OnEagleView(InputValue value) //CommandY
    {
            buttonY.execute(value);
            Notify(buttonY);
        
    }

    public void OnLook(InputValue value) //Si vers le bas : CommandX
    {
        //On  n'y touche pas pour l'instant, trop risuqé de faire des erreurs et compliquer
    }

}
