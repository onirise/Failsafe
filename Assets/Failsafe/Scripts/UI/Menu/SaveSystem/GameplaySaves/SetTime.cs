using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class SetTime : MonoBehaviour
{
    public LocalizeStringEvent localizeStringEvent;
    
    public float time;
    void OnSetTime()
    {
        
        localizeStringEvent.StringReference.Arguments = new object[] { time };
        localizeStringEvent.RefreshString();
    }

    void Update()
    {
        time += Time.deltaTime;

        OnSetTime();
        
    }
}
