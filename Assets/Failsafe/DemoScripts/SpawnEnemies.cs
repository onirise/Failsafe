using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DMDungeonGenerator;
using Unity.AI.Navigation;
using System.Linq;

public class SpawnEnemiesCallback : MonoBehaviour
{
    // Start is called before the first frame update

    public DMDungeonGenerator.DungeonGenerator generator;

    public GameObject PlayerPrefab;
    public GameObject spawnedPlayer;
    public GameObject EnemyPrefab;
    public NavMeshSurface NavMeshSurface;
    public List<GameObject> enemies = new List<GameObject>();

    /// <summary>
    /// Шанс для каждой комнаты, что в ней заспавнится враг
    /// </summary>
    public float enemySpawnChance = 0.8f;

    void Awake()
    {
        if (generator != null)
        {
            Debug.Log("Registered post generation callback");
            generator.OnComplete += GeneratorComplete;
        }
    }

    public void GeneratorComplete(DMDungeonGenerator.DungeonGenerator generator)
    {
        //cleanup
        //Destroy the player if one already exists from the last generation
        if (spawnedPlayer != null) GameObject.DestroyImmediate(spawnedPlayer);
        for (int i = 0; i < enemies.Count; i++) GameObject.DestroyImmediate(enemies[i].gameObject);
        enemies = new List<GameObject>();
        //cleanup done

        GenerateNavMesh();

        //spawn the player in the first room somewhere
        Vector3 spawnRoomPos = generator.DungeonGraph[0].data.gameObject.transform.position;
        spawnedPlayer = GameObject.Instantiate(PlayerPrefab, spawnRoomPos, Quaternion.identity);

        SpawnEnemies();
    }

    public void GenerateNavMesh()
    {
        var doors = new List<GameObject>();
        foreach (Transform child in generator.transform)
        {
            if (child.TryGetComponent<GameplayDoor>(out _))
            {
                doors.Add(child.gameObject);
            }
        }
        // Нужно сперва деактивировать двери, иначе навмеш не соединиться между комнатами
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
        NavMeshSurface.BuildNavMesh();
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }
    }

    /// <summary>
    /// Генерация врагов в комнате
    /// </summary>
    public void SpawnEnemies()
    {
        for (int i = 0; i < generator.DungeonGraph.Count; i++)
        {
            GraphNode room = generator.DungeonGraph[i];
            if (generator.rand.Next(100) / 100f > enemySpawnChance) continue;

            Vector3 enemyOffset = new Vector3(Random.Range(-0.1f, 0.1f), 0f, Random.Range(-0.1f, 0.1f));
            Vector3 enemyPos = room.data.transform.position;

            GameObject spawnedEnemy = GameObject.Instantiate(EnemyPrefab, enemyPos + enemyOffset, Quaternion.identity);
            enemies.Add(spawnedEnemy);

            spawnedEnemy.GetComponentInChildren<VisualSensor>().Target = spawnedPlayer.transform;
        }
    }
}
