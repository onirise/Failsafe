using UnityEngine;

public class SaveLoadGame : MonoBehaviour
{
    private SaveLoadManager saveLoadManager;
    private GameData currentGameData;

    [SerializeField] private Player player;//Не бейте бога ради, это временно

    private void Awake()
    {
        saveLoadManager = GetComponent<SaveLoadManager>();
        currentGameData = new GameData(); // Инициализация новых данных
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            SavePlayerState();

        if (Input.GetKeyDown(KeyCode.F9))
            LoadGame();
    }
    private void SaveGame()
    {
        //сохроняем текущее состояние игры

        saveLoadManager.SaveGame(currentGameData);
    }

    public void SavePlayerState()
    {
        // Сохронения состояния игрока
        currentGameData.runData.playerState.health = player.health;
        currentGameData.runData.playerState.position = player.gameObject.transform.position;
        currentGameData.runData.playerState.rotation = player.gameObject.transform.rotation;

        Debug.Log($"Saving Health: {currentGameData.runData.playerState.health}");
        Debug.Log($"Saving Position: {player.gameObject.transform.position}, {player.gameObject.transform.rotation}");

        SaveGame();
    }
    public void LoadGame()
    {
        currentGameData = saveLoadManager.LoadGame();
        if (currentGameData != null)
        {
            UpdateGameState(currentGameData);
        }
    }
    private void UpdateGameState(GameData gameData)
    {
        // Обновление данных о текущем забеге

        // Обновление состояния игрока
        player.health = gameData.runData.playerState.health;

        Debug.Log($"Loaded Health: {gameData.runData.playerState.health}");
        Debug.Log($"Loaded Position: {gameData.runData.playerState.position}");
        Debug.Log($"Current Position: {player.gameObject.transform.position}");




        // Обновление текущего этажа

        // Обновление информации о противниках

        // Обновление квестов

        // Дополнительные обновления, если необходимо
        // Например, обновление UI, состояния игры и т.д.
    }
}
