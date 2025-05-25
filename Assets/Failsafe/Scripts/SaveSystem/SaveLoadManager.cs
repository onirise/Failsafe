using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;

    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "gameData.json");
    }

    public void SaveGame(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved!");
    }

    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded!");
            return gameData;
        }
        else
        {
            Debug.LogError("Save file not found!");
            return null;
        }
    }
}
