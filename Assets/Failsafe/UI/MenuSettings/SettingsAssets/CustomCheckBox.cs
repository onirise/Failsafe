using UnityEngine;

public class CustomCheckBox : MonoBehaviour
{
    public RectTransform CheckMark;
    

    public void OnSwitch(bool isOn)
    {
        if (isOn)
        {
            CheckMark.anchorMin = new Vector2(1, 0);
            CheckMark.anchorMax = new Vector2(1, 1);
            CheckMark.anchoredPosition = new Vector2(-17.5f,0);
            
            
        }
        else
        {
            
            
            CheckMark.anchorMin = new Vector2(0, 0);
            CheckMark.anchorMax = new Vector2(0, 1);
            CheckMark.anchoredPosition = new Vector2(17.5f,0);
        }
        
    }
}
