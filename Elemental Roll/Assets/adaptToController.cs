using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class adaptToController : MonoBehaviour
{
    public Sprite PS4;
    public Sprite XB1;
    public Sprite PC;
    public Sprite Switch;
    public PlayerInput input;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<Image>(out image))
        {
            
            switch (input.devices[0].ToString())
            {
                case "Keyboard:/Keyboard":
                    image.sprite = PC;
                    break;
                case "Mouse:/Mouse":
                    image.sprite = PC;
                    break;
                case "XInputControllerWindows:/XInputControllerWindows":
                    image.sprite = XB1;
                    break;
                case "DualShock4GamepadHID:/DualShock4GamepadHID":
                    image.sprite = PS4;
                    break;
                case "Gamepad:/Gamepad":
                    image.sprite = PS4;
                    break;
                default:
                    image.sprite = PS4;
                    break;

            }
        }
    }

  
}
