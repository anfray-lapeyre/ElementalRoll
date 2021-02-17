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
    //SecondSpecialAction->CommandY
    Command buttonY;
    //Move -> CommandLeftStick
    Command leftStick;
    //Move -> CommandLeftStick for Menu
    Command leftStickMenu;
    //Pause -> CommandStart
    Command buttonStart;

    //1/2/3/4 or d-pad -> Swap character
    Command swapPlayer;

    private float timeBetweenTwoMove;
    private float maxTimeBetweenTwoMove = 0.5f;
    private float minTimeBetweenTwoMove = 0.05f;
    private bool hasStoppedMoving = false;

    override protected void Awake()
    {
        base.Awake();

        buttonStart = this.gameObject.AddComponent<PauseCommand>();
        leftStick = this.gameObject.AddComponent<MoveCommand>(); 
        leftStickMenu = this.gameObject.AddComponent<MoveCommand>();
        (leftStickMenu as MoveCommand).isMenuController = true;
        buttonA = this.gameObject.AddComponent<SpellCommand>(); 
        buttonB = this.gameObject.AddComponent<RestartCommand>(); 
        buttonX = this.gameObject.AddComponent<TopViewCommand>(); 
        buttonY = this.gameObject.AddComponent<SecondSpecialActionCommand>();
        swapPlayer = this.gameObject.AddComponent<SwapCharacterCommand>();
        timeBetweenTwoMove = maxTimeBetweenTwoMove;

    }


    public void OnRestart(InputValue value) // CommandB
    {
        //Debug.Log("Restart");
        buttonB.execute(value);
            Notify(buttonB);
    }

    public void OnSpecialAction(InputValue value) //CommandA
    {
       // Debug.Log("SpecialAction");
        buttonA.execute(value);
            Notify(buttonA);
    }

    public void OnMove(InputValue value) //CommandLeftStick
    {
        leftStick.execute(value);
       // Debug.Log("Move + : "+value.Get<Vector2>());
        Notify(leftStick);

    }

    public void OnMoveJoystick(InputValue value)
    {
        (leftStickMenu as MoveCommand).setJoystickMenu(true);

        leftStickMenu.execute(value);
        Notify(leftStickMenu);
        if ((leftStickMenu as MoveCommand).isMoving())
        {
            InvokeRealTime("MoveAgain", timeBetweenTwoMove);
            hasStoppedMoving = false;
        }
        else
        {
            this.StopAllCoroutines();
            timeBetweenTwoMove = maxTimeBetweenTwoMove;

            hasStoppedMoving = true;
        }
    }

    public void OnMoveHorizontal(InputValue value)
    {
        (leftStickMenu as MoveCommand).setJoystickMenu(false);


        (leftStick as MoveCommand).executeHorizontal(value);
        Notify(leftStick);


        (leftStickMenu as MoveCommand).executeHorizontal(value);
        Notify(leftStickMenu);
        if ((leftStickMenu as MoveCommand).isMoving())
        {
            InvokeRealTime("MoveAgain", timeBetweenTwoMove);
            hasStoppedMoving = false;
        }
        else
        {
            this.StopAllCoroutines();
            timeBetweenTwoMove = maxTimeBetweenTwoMove;

            hasStoppedMoving = true;
        }
    }

    public void OnMoveVertical(InputValue value)
    {
        (leftStickMenu as MoveCommand).setJoystickMenu(false);

        (leftStick as MoveCommand).executeVertical(value);
        Notify(leftStick);

        (leftStickMenu as MoveCommand).executeVertical(value);
        Notify(leftStickMenu);
        if ((leftStickMenu as MoveCommand).isMoving())
        {
            InvokeRealTime("MoveAgain", timeBetweenTwoMove);
            hasStoppedMoving = false;
        }
        else
        {
            this.StopAllCoroutines();
            timeBetweenTwoMove = maxTimeBetweenTwoMove;

            hasStoppedMoving = true;
        }
    }

    public void MoveAgain()
    {
        if((leftStickMenu as MoveCommand).isMoving() && !hasStoppedMoving)
        {
            Notify(leftStickMenu);
            timeBetweenTwoMove = Mathf.Max(timeBetweenTwoMove / 2f , minTimeBetweenTwoMove);
            InvokeRealTime( "MoveAgain", timeBetweenTwoMove);
            
        }
        else
        {
            this.StopAllCoroutines();
            timeBetweenTwoMove = maxTimeBetweenTwoMove;
            hasStoppedMoving = true;
        }
    }

    public void OnPause(InputValue value) //CommandStart
    {
        //Debug.Log("Pause");

        buttonStart.execute(value);
            Notify(buttonStart);
        
    }

    public void OnTopView(InputValue value) //CommandX
    {
        //Debug.Log("TopView : " +value.Get<float>());

        buttonX.execute(value);
            Notify(buttonX);
        
    }
    public void OnTopViewTrigger(InputValue value) //CommandX
    {
        //Debug.Log("TopView : " + value.Get<float>());

        buttonX.execute(value);
        Notify(buttonX);

    }
    public void OnSecondSpecialAction(InputValue value) //CommandY
    {
        //Debug.Log("SecondSpecialAction");

        buttonY.execute(value);
            Notify(buttonY);
        
    }

    public void OnLook(InputValue value) //Si vers le bas : CommandX
    {
        //On  n'y touche pas pour l'instant, trop risuqé de faire des erreurs et compliquer
    }

    public void OnSwapPlayer(InputValue value)
    {
        swapPlayer.execute(value);
        Notify(swapPlayer);
    }

}
