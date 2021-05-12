using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData
{
    public float bestTime;
    public bool[] isUnlocked;
}

public class GameData : MonoBehaviour 
{
    public static GameData gameData;
    public SaveData saveData;

    public int currentLevel;
    public Vector3 respawnPosition;

	// Use this for initialization
	void Awake () 
    {
	    if(gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
	}
	void Start()
    {
        Load();
    }

    public void UnlockNextLevel()
    {
        if(currentLevel < saveData.isUnlocked.Length)
        {
            saveData.isUnlocked[currentLevel] = true;
            Save();
            currentLevel++;
        }
    }
	public void Save()
    {
        string path = Application.persistentDataPath + "/levels.data";
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = File.Open(path, FileMode.Create);

        SaveData data = new SaveData();
        data = saveData;

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/levels.data";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);

            saveData = binaryFormatter.Deserialize(stream) as SaveData;
            stream.Close();
        }
    }

    void OnDisable()
    {
        //Save();
    }
}
