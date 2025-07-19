using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnSelectEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    List<string> _originalText;
    Color _originalColor;
    Material _originMaterial;
    Image _buttonBackground;
    [SerializeField] Material _targetMaterial;
    [SerializeField] Color _targetColor;
    [SerializeField] List<TextMeshProUGUI> _textMeshProUGUI;

    private void Start()
    {
        _buttonBackground = GetComponent<Image>();
        _originMaterial = _textMeshProUGUI[0].fontSharedMaterial;
        _originalText[0] = _textMeshProUGUI[0].text;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered");
        foreach (var item in _textMeshProUGUI)
        {
            item.fontSharedMaterial = _targetMaterial;
            item.fontWeight = FontWeight.SemiBold;
            item.text = ">" + _originalText[0];
        }

        _buttonBackground.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exited");
        foreach (var item in _textMeshProUGUI)
        {
            item.fontSharedMaterial = _originMaterial;
            item.fontWeight = FontWeight.Regular;
            item.text = _originalText[0];
        }

        _buttonBackground.color = new Color(1, 1, 1, 0);

    }

}
