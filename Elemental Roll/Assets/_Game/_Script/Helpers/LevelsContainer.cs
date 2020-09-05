
using UnityEngine;

[System.Serializable]
public class LevelsContainer
{
    public LevelFormat[] levels;

    public static LevelsContainer CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<LevelsContainer>(jsonString);
    }
}

