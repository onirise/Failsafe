using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public RunData runData;

    public GameData()
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
    public RunData()
    {
        playerState = new PlayerStateData();//Пока только cостояние игрока, временно
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
    // Другие параметры противника
}
public class QuestData
{
    // Параметры квестов
}
