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

    private float timeBetweenTwoMove;
    private float maxTimeBetweenTwoMove = 0.5f;
    private float minTimeBetweenTwoMove = 0.05f;
    private bool hasStoppedMoving = false;

    override protected void Awake()
    {
        base.Awake();

        buttonStart = this.gameObject.AddComponent<PauseCommand>();
        leftStick = this.gameObject.AddComponent<MoveCommand>(); 
        buttonA = this.gameObject.AddComponent<SpellCommand>(); 
        buttonB = this.gameObject.AddComponent<RestartCommand>(); 
        buttonX = this.gameObject.AddComponent<TopViewCommand>(); 
        buttonY = this.gameObject.AddComponent<EagleViewCommand>(); 
        timeBetweenTwoMove = maxTimeBetweenTwoMove;
        Debug.Log("Bien s'passer");
    }


    public void OnRestart(InputValue value) // CommandB
    {
        Debug.Log("Restart");
            buttonB.execute(value);
            Notify(buttonB);
        
    }

    public void OnSpecialAction(InputValue value) //CommandA
    {
        Debug.Log("SpecialAction");

        buttonA.execute(value);
            Notify(buttonA);
        
    }

    public void OnMove(InputValue value) //CommandLeftStick
    {
        (leftStick as MoveCommand).setJoystick(false);
        leftStick.execute(value);
        Debug.Log("Move + : "+value.Get<Vector2>());
        Notify(leftStick);
            if ((leftStick as MoveCommand).isMoving()) {
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

    public void OnMoveJoystick(InputValue value)
    {
        (leftStick as MoveCommand).setJoystick(true);

        leftStick.execute(value);
        Debug.Log("Move + : " + value.Get<Vector2>());
        Notify(leftStick);
        if ((leftStick as MoveCommand).isMoving())
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
        (leftStick as MoveCommand).executeHorizontal(value);
        Notify(leftStick);
        if ((leftStick as MoveCommand).isMoving())
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
        (leftStick as MoveCommand).executeVertical(value);
        Notify(leftStick);
        if ((leftStick as MoveCommand).isMoving())
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
        if((leftStick as MoveCommand).isMoving() && !hasStoppedMoving)
        {
            Notify(leftStick);
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
        Debug.Log("Pause");

        buttonStart.execute(value);
            Notify(buttonStart);
        
    }

    public void OnTopView(InputValue value) //CommandX
    {
        Debug.Log("TopView");

        buttonX.execute(value);
            Notify(buttonX);
        
    }

    public void OnEagleView(InputValue value) //CommandY
    {
        Debug.Log("EagleView");

        buttonY.execute(value);
            Notify(buttonY);
        
    }

    public void OnLook(InputValue value) //Si vers le bas : CommandX
    {
        //On  n'y touche pas pour l'instant, trop risuqé de faire des erreurs et compliquer
    }




}
