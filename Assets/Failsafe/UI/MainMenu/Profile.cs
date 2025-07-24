using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{

    ProfileMenu _profileMenu;

    [SerializeField] TMP_Text _indexText;
    [SerializeField] TMP_Text _profileNameText;
    [SerializeField] GameObject _selectedFrame;




    private void Awake()
    {
        _profileMenu = GetComponentInParent<ProfileMenu>();

    }

    public void SetData(int index)
    {
        _indexText.text = "0" + index;
    }
    public void OnProfileClicked()
    {
        _profileMenu.ProfileClickAction(this);
    }

    public void ShowSelectedProfile()
    {
        _selectedFrame.SetActive(true);
    }

    public void DeselectProfile()
    {
        _selectedFrame.SetActive(false);
    }

}
