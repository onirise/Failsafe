using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData//Все игровые данные для слхронения
{
    public RunData runData;
    // Другие параметры для сохронения в игре

    public GameData() // Инициализация
    {
        runData = new RunData();
    }
}

[System.Serializable]
public class RunData
{
    public PlayerStateData playerState;
    public List<SubjectsData> Subjects;
    public FloorGenerationData floorGeneration;
    public List<Enemy_Data> enemies;
    public List<QuestData> quests;
    //Параметры на время рана
    public RunData() // Инициализация
    {
        playerState = new PlayerStateData();
    }
}

[System.Serializable]
public class PlayerStateData
{
    public int health;
    public Vector3 position;
    // Другие параметры состояния игрока
}

[System.Serializable]
public class SubjectsData
{
    public string SubjectName;
    public Vector3 position;
    // Другие параметры предмета
}

[System.Serializable]
public class FloorGenerationData
{
    // Параметры генерации этажа
}

[System.Serializable]
public class Enemy_Data
{
    public string enemyName;
    public Vector3 position;
    public int health;
}
public class QuestData
{
    //Прогресс квеста
}
