using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private readonly string saveStrName = "_dsd2sdfdf3fIOSD";

    public SaveData saveData { get; private set; }

    public void Save()
    {
        var str = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(saveStrName,str);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(saveStrName))
        {
            var str = PlayerPrefs.GetString(saveStrName);
            saveData = JsonUtility.FromJson<SaveData>(str);
        }
        else
        {
            saveData = new SaveData();
            Save();
        }
    }
}

[System.Serializable]
public class SaveData
{
    public int OpenedLevel = 0;
    public float LastProgress = 0f;
    public int CountStars = 0;
    public int LeftoverFood = 0;    
}