using UnityEngine;
using System.Collections.Generic;

public class PuzzleNode : MonoBehaviour
{
    // Локальные направления выходов 
    public List<Vector3> exits = new List<Vector3>()
    {
        Vector3.forward,
        Vector3.left
    };

    private int _rotationStep = 0;

    public void RotateNode()
    {
        transform.localRotation *= Quaternion.AngleAxis(90f, Vector3.up);
        _rotationStep = (_rotationStep + 1) % 4;
    }

    // Получить глобальные направления выходов с учётом текущего поворота
    public List<Vector3> GetGlobalExits()
    {
        List<Vector3> globalExits = new List<Vector3>();
        foreach (var exit in exits)
        {
            Vector3 globalDir = transform.rotation * exit;
            globalExits.Add(globalDir.normalized);
            Debug.Log(globalDir.normalized);
        }
        return globalExits;
    }

    // Для теста — поворот по клику мыши
    private void OnMouseDown()
    {
        RotateNode();
        Debug.Log(_rotationStep);
        //GetGlobalExits();
    }
}
