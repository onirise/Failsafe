using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnSelectEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string _originalText;
    Color _originalColor;
    Material _originMaterial;
    [SerializeField] Material _targetMaterial;
    [SerializeField] Color _targetColor;
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;

    private void Start()
    {
        _originMaterial = _textMeshProUGUI.fontSharedMaterial;
        _originalText = _textMeshProUGUI.text;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered");
        _textMeshProUGUI.fontSharedMaterial = _targetMaterial;
        _textMeshProUGUI.fontWeight = FontWeight.SemiBold;
        _textMeshProUGUI.text = ">" + _originalText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exited");
        _textMeshProUGUI.fontSharedMaterial = _originMaterial;
        _textMeshProUGUI.fontWeight = FontWeight.Regular;
        _textMeshProUGUI.text = _originalText;

    }

}
