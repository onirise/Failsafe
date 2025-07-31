using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class SwitcherScript : MonoBehaviour
{
    [SerializeField]TMP_Text OutputDataText;
    public List<string> options;
    public UnityEvent<Int32> OnValueChange;

    int currentOption = 0;

    public void AddOptions(List<string> options)
    {
        this.options.AddRange(options);
        OutputDataText.text = options[currentOption];
    }


    
    public void SetCurrentOption(int option)
    {
        currentOption = option;
        OutputDataText.text = options[currentOption];
    }

    public void OnLeftClick()
    {
        
        
        currentOption--;
        if (currentOption < 0)
        {
            currentOption = options.Count - 1;
        }
        OutputDataText.text = options[currentOption];
        OnValueChange.Invoke(currentOption);
    }
    
    public void OnRightClick()
    {
        currentOption++;
        if (currentOption > options.Count - 1)
        {
            currentOption = 0;
        }
        OutputDataText.text = options[currentOption];
        OnValueChange.Invoke(currentOption);
        
    }


    public int GetCurrentOption()
    {
        return currentOption;
    }
}
