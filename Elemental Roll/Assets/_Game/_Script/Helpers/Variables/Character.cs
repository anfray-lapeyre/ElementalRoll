using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Character
{
    public string name { get; set; }
    public float pitch { get; set; }
    public float baseVolume { get; set; }
    public float maxSpeed { get; set; }
    public float speedModifier { get; set; }
    public float invertSpeedModifier { get; set; }
    public float jumpForce { get; set; }
    public float powerTime { get; set; }
    public float secondPowerTime { get; set; }

    public Character(string _name, float _pitch, float _baseVolume, float _speedModifier, float _invertSpeedModifier, float _jumpForce, float _powerTime, float _secondPowerTime)
    {
        this.name = _name;
        this.pitch = _pitch;
        this.baseVolume = _baseVolume;
        this.speedModifier = _speedModifier;
        this.invertSpeedModifier = _invertSpeedModifier;
        this.jumpForce = _jumpForce;
        this.powerTime = _powerTime;
        this.secondPowerTime = _secondPowerTime;
    }

    public Character(string _name = "Byle", float _pitch = 1f)
    {
        this.name = _name;
        this.pitch=_pitch;
        this.baseVolume = 1f;
        this.speedModifier = 3f;
        this.invertSpeedModifier = 2f;
        this.jumpForce = 550f;
        this.powerTime = 5f;
        this.secondPowerTime = 5f;
    }

    public Character(string _name)
    {
        switch (_name)
        {
            case "A Wizard Passing By":
                this.name = "A Wizard Passing By";
                this.pitch = 0.65f;
                this.baseVolume = 1f;
                this.speedModifier = 3f;
                this.invertSpeedModifier = 2f;
                this.jumpForce = 550f;
                this.powerTime = 15f;
                break;
            case "Tracy":
                this.name = "Tracy";
                this.pitch = 1f;
                this.baseVolume = 1f;
                this.speedModifier = 3.1f;
                this.invertSpeedModifier = 2.6f;
                this.jumpForce = 600f;
                this.powerTime = 10f; //Bubble float
                this.secondPowerTime = 15f; //Shrink
                break;
            case "Rocky":
                this.name = "Rocky";
                this.pitch = 0.6f;
                this.baseVolume = 1f;
                this.speedModifier = 2.9f;
                this.invertSpeedModifier = 1f;
                this.jumpForce = 400f;
                this.powerTime = 8f; //Stop then Bottom Dash
                this.secondPowerTime = 15f; //Grappling
                break;
            case "Tim":
                this.name = "Tim";
                this.pitch = 1f;
                this.baseVolume = 1f;
                this.speedModifier = 3f;
                this.invertSpeedModifier = 2.3f;
                this.jumpForce = 450f;
                this.powerTime = 10f;//AOE Freeze
                this.secondPowerTime = 15f; //Rewind
                break;
            default:
                this.name = "Byle";
                this.pitch = 1.41f;
                this.baseVolume = 1f;
                this.speedModifier = 3f;
                this.invertSpeedModifier = 2f;
                this.jumpForce = 550f;
                this.powerTime = 6f; //Sprint
                this.secondPowerTime = 5f; //Jump
                break;

        }
    }

    public Character Fire()
    {
        return new Character("Byle");
    }

    public Character Ice()
    {
        return new Character("Tracy");
    }

    public Character Earth()
    {
        return new Character("Rocky");
    }

    public Character Death()
    {
        return new Character("Tim");
    }

    public Character Wizard()
    {
        return new Character("A Wizard Passing By");
    }


    public static Character getCharacterInfo(int power)
    {
        switch (power)
        {
            case 1:
                return new Character("Tracy");
            case 2:
                return new Character("Rocky");
            case 3:
                return new Character("Tim");
            default:
                return new Character("Byle");
        }
    }
}
