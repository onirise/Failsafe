#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStateConfig))]
public class EnemyStateConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EnemyStateConfig config = (EnemyStateConfig)target;

        if (GUILayout.Button("Обновить список состояний"))
        {
            config.RefreshStates();
            EditorUtility.SetDirty(config);
            Debug.Log("Список доступных состояний обновлён.");
        }
    }
}
#endif