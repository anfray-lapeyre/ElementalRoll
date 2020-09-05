using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Character
{
    public string name { get; set; }
    public float pitch { get; set; }

    public Character(string _name = "Byle", float _pitch = 1f)
    {
        this.name = _name;
        this.pitch=_pitch;
    }

    public Character Fire()
    {
        return new Character("Byle", 1.41f);
    }

    public Character Ice()
    {
        return new Character("Tracy", 1f);
    }

    public Character Earth()
    {
        return new Character("Rocky", 0.6f);
    }

    public Character Death()
    {
        return new Character("Tim", 1f);
    }

    public Character Wizard()
    {
        return new Character("Wizard", 0.65f);
    }
}
