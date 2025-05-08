using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatas", menuName = "Data/SoundDatas", order = 2)]
public class SoundDatas : ScriptableObject
{
    public SoundData[] soundDatas;
}
public enum SoundType { Footstep, Impact, Distract, Explosion }

[System.Serializable]
public class SoundData
{
    public string soundName; // Название звука
    public SoundType soundType; // Тип звука
    public float maxRadius; // Максимальный радиус звука
    public float duration; // Длительность звука

  
}


