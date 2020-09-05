using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialogue 
{
    public Character interlocutor { get; set; }
    public string[] lines { get; set; }
    public int id;

    public Dialogue(Character _interlocutor, string[] _lines, int _id)
    {
        interlocutor = _interlocutor;
        lines = _lines.Clone() as string[];
        id = _id;
    }
}
