using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveGame(SaveFileInfo levelsSave, int currentSaveFile = 0)
    {
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player"+currentSaveFile+".slime";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, levelsSave);
        stream.Close();
    }

    public static void SaveGame(int chosenPlayer=0)
    {
        if(ActualSave.actualSave.levels.Length <= 0)
        {
            ActualSave.actualSave = new SaveFileInfo();
        }
        //In this case we use the ActualSave data
        SaveGame(ActualSave.actualSave);
    }

    public static void EraseGame(int currentSaveFile = 0)
    {
        string path = Application.persistentDataPath + "/player" + currentSaveFile + ".slime";
        File.Delete(path);
    }


    public static SaveFileInfo LoadGame(int currentSaveFile=0)
    {
        string path = Application.persistentDataPath + "/player" + currentSaveFile + ".slime";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveFileInfo save = formatter.Deserialize(stream) as SaveFileInfo;
            stream.Close();
            return save;
        }
        else
        {
            //Debug.LogError("Save File not found in : " + path);
            return null;
        }
    }
}
