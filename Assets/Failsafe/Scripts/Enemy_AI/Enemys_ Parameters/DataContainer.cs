using UnityEngine;
using NorskaLib.Spreadsheets;
using System;
using System.Collections.Generic;
using static DataContainer;
public class DataContainer
{
    [Serializable]
    public class SpreadsheetContent
    {
        [SpreadsheetPage("Enemy")]
        public List<EnemyData> enemyDatas; // Массив данных врагов
    }

    [CreateAssetMenu(fileName = "SpreadsheetContainer", menuName = "SpreadsheetContainer")]
    public class SpreadSheetContainer : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] SpreadsheetContent content;
        [SerializeField] public SpreadsheetContent Content => content;
    }
}
