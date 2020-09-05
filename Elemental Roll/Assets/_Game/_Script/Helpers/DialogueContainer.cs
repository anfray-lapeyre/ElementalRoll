
using UnityEngine;

[System.Serializable]

public class DialogueContainer 
{
    public DialogueJSONFormat[] dialogues;

    public static DialogueContainer CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<DialogueContainer>(jsonString);
    }
}
