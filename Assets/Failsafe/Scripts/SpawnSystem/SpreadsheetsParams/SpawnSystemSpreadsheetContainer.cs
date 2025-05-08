using System;
using System.Collections.Generic;
using NorskaLib.Spreadsheets;
using UnityEngine;

namespace SpawnSystem
{
    [Serializable]
    public class SpreadshetContent
    {
        [SpreadsheetPage("enemySpawnDatas")]
        public List<EnemySpawnData> enemySpawnDatas;
    }

    [CreateAssetMenu(fileName = "SpreadsheetContainer", menuName = "SpawnSystem/SpawnSystemSpreadsheetContainer")]
    public class SpawnSystemSpreadsheetContainer : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] SpreadshetContent content;
        public SpreadshetContent Content => content;

        [ContextMenu("DebugData")]
        public void DebugData()
        {
            foreach (var enemy in Content.enemySpawnDatas)
            {
                Debug.Log($"{enemy.Name}; {enemy.Weight}; {enemy.Random}; {enemy.Timer}; {enemy.Level}; {enemy.EnemyName};");
            }
        }
    }
}
