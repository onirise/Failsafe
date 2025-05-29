using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;


public class Profile : BaseMenu, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    LocalizeStringEvent _profileNameLocStrEvent;
    [SerializeField]
    GameObject _clickToSelectTextGO;
    [SerializeField]
    GameObject _clickToOpenSavesTextGO;
    [SerializeField]
    GameObject _selectedTextGO;
    [SerializeField]
    GameObject _newProfileTextGO;
    [SerializeField]
    TMP_Text _indexText;




    int _profileIndex = 0;



    public void DeleteProfile()
    {
        CallConfirm(() =>
        {
            ProfilesHandler.RemoveFromProfilesList(_profileIndex);
        });
    }

    public void SetDATA(ProfileDATA _profileDATA, int _index)
    {
        _profileNameLocStrEvent.SetEntry(_profileDATA.LocaleEntryIndex.ToString());
        _selectedTextGO.SetActive(ProfilesHandler.IsSelectedProfile(_index));
        _newProfileTextGO.SetActive(_profileDATA.IsNew);
        _profileIndex = _index;
        _indexText.text = (_profileIndex + 1).ToString();
        _clickToSelectTextGO.SetActive(false);

    }


    public void OnSelectProfile()
    {
        //if(!profilesHandler.profiles1[profileIndex].isNew)
        // {
        //     gameplaySavesHandler.SetSavesFromSelectedProfile(profilesHandler.profiles1[profileIndex].gameplaySaveDATAs);        
        //     gameplaySavesHandler.OpenGSavesWindow(this, SaveState.Load);
        // }

        if (ProfilesHandler.IsSelectedProfileIsNew(_profileIndex))
            ProfilesHandler.SetSelectedProfile(_profileIndex);


    }

    public void OnMouseEnterToProfile()
    {
        if (!ProfilesHandler.IsSelectedProfile(_profileIndex))
        {
            _clickToSelectTextGO.SetActive(true);
        }
        else if (!ProfilesHandler.Profiles[_profileIndex].IsNew)
            _clickToOpenSavesTextGO.SetActive(true);


    }

    public void OnMouseExitToProfile()
    {
        _clickToSelectTextGO.SetActive(false);
        _clickToOpenSavesTextGO.SetActive(false);

    }


    // Использовал ивенты таким образом, ибо если использовать компонент Event trigger,
    // то не будет работать скроллинг профилей пока мышка находится на одном из профилей
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterToProfile();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitToProfile();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelectProfile();

    }

}
