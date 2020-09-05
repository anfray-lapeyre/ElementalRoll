
using UnityEngine;

[System.Serializable]
public class LevelsSaveContainer
{
    public LevelSaveFormat[] levels;

    public static LevelsSaveContainer CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<LevelsSaveContainer>(jsonString);
    }
}

