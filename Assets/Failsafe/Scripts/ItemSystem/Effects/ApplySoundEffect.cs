using UnityEngine;

public class ApplySoundEffect : BaseEffect
{
    // to-do Поменять на нормальный звук
    public AudioClip Sound;
    public override void Apply()
    {
        //to-do Вызов звука
        Debug.Log($"Был вызван звук {Sound}");
    }
}