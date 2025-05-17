using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPrefabsSO", menuName = "SpawnSystem/EnemyPrefabsSO")]
public class EnemyPrefabsSO : ScriptableObject
{
    public List<EnemyPrefabData> enemyPrefabDatas;

    public Dictionary<string, GameObject> ToDictionary()
    {
        return enemyPrefabDatas.ToDictionary(x => x.Name, y => y.EnemyPrefab, StringComparer.OrdinalIgnoreCase);
    }
}
[Serializable]
public class EnemyPrefabData
{
    public string Name;
    public GameObject EnemyPrefab;
}
