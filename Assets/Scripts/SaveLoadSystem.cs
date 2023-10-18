using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour 
{
    public static void SaveFile(int score)
    {
        string destination = Application.persistentDataPath + "/highscore.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, score);
        file.Close();
        
        Debug.Log(score + " is saved");
    }

    public static string[] LoadFile()
    {
        string destination = Application.persistentDataPath + "/highscore.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found. Destination: " + destination);
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        string[] loadedData = (string[]) bf.Deserialize(file);
        file.Close();
        
        return loadedData;
    }

}