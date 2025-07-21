using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnSelectEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string _mainOriginalText;
    Color _originalColor;
    Material _originMaterial;
    Image _buttonBackground;
    [SerializeField] bool _addArrow;
    [SerializeField] Material _targetMaterial;
    [SerializeField] Color _targetColor;
    [SerializeField] TextMeshProUGUI _mainTextMeshProUGUI;
    [SerializeField] List<TextMeshProUGUI> _optionalTextsGO;


    private void Start()
    {
        _buttonBackground = GetComponent<Image>();

        _originMaterial = _mainTextMeshProUGUI.fontSharedMaterial;
        _mainOriginalText = _mainTextMeshProUGUI.text;
        _optionalTextsGO.Add(_mainTextMeshProUGUI);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered");


        foreach (TextMeshProUGUI v in _optionalTextsGO)
        {
            v.fontSharedMaterial = _targetMaterial;
            v.fontWeight = FontWeight.SemiBold;

        }
        if (_addArrow)
        {
            _mainTextMeshProUGUI.text = ">" + _mainOriginalText;
        }

        _buttonBackground.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exited");


        foreach (TextMeshProUGUI v in _optionalTextsGO)
        {
            v.fontSharedMaterial = _originMaterial;
            v.fontWeight = FontWeight.Regular;

        }

        _mainTextMeshProUGUI.text = _mainOriginalText;
        _buttonBackground.color = new Color(1, 1, 1, 0);

    }

}
