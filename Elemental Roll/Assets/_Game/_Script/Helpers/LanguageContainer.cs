
using UnityEngine;

[System.Serializable]

public class LanguageContainer 
{
    public LanguageJSONFormat[] languages;

    public static LanguageContainer CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<LanguageContainer>(jsonString);
    }
}
