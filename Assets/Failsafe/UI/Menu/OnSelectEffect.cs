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
    [SerializeField] Material _targetMaterial;
    [SerializeField] Color _targetColor;
    // это текст, у которого должна появляться стрелочка при наведении
    [SerializeField] TextMeshProUGUI _mainTextMeshProUGUI;

    // это тексты в случае если у объекта несколько текстов (как у профилей) и им не нужна стрелочка при наведении
    //  у профилей вообще только эти тексты и заполнены, mainTextMeshProUGUI пуст
    [SerializeField] List<TextMeshProUGUI> _optionalTextsGO;
    // задумка в том, что объекту могут быть одновременно нужны как изменения текста с добавлением стрелочки
    // так и как раз дополнительные тексты, которым стрелка не нужна
    // наследование я делать не хотел.

    private void Start()
    {
        _buttonBackground = GetComponent<Image>();
        if (_mainTextMeshProUGUI != null)
        {
            _originMaterial = _mainTextMeshProUGUI.fontSharedMaterial;
            _mainOriginalText = _mainTextMeshProUGUI.text;
        }
        else
        {
            _originMaterial = _optionalTextsGO[0].fontSharedMaterial;
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered");


        for (int i = 0; i < _optionalTextsGO.Count; i++)
        {
            _optionalTextsGO[i].fontSharedMaterial = _targetMaterial;
            _optionalTextsGO[i].fontWeight = FontWeight.SemiBold;

        }

        if (_mainTextMeshProUGUI != null)
        {
            _mainTextMeshProUGUI.fontSharedMaterial = _targetMaterial;
            _mainTextMeshProUGUI.fontWeight = FontWeight.SemiBold;
            _mainTextMeshProUGUI.text = ">" + _mainOriginalText;
        }


        _buttonBackground.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exited");


        for (int i = 0; i < _optionalTextsGO.Count; i++)
        {
            _optionalTextsGO[i].fontSharedMaterial = _originMaterial;
            _optionalTextsGO[i].fontWeight = FontWeight.Regular;

        }

        if (_mainTextMeshProUGUI != null)
        {
            _mainTextMeshProUGUI.fontSharedMaterial = _originMaterial;
            _mainTextMeshProUGUI.fontWeight = FontWeight.Regular;
            _mainTextMeshProUGUI.text = _mainOriginalText;
        }


        _buttonBackground.color = new Color(1, 1, 1, 0);

    }

}
