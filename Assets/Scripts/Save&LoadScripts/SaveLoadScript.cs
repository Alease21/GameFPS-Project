using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadScript
{
    public static void Save(string fileName, GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName + ".cam";
        FileStream fs = new FileStream(path, FileMode.Create);
        bf.Serialize(fs, gameData);
        fs.Close();
    }

    public static GameData Load(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".cam";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            GameData gameData = bf.Deserialize(fs) as GameData;
            fs.Close();
            return gameData;
        }
        return null;
    }
}
