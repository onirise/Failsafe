using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using Zenject;

public class Profile : BaseMenu, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject clickToSelectTextGO;

    public GameObject clickToOpenSavesTextGO;

    public GameObject selectedTextGO;

    public GameObject newProfileTextGO;


    public TMP_Text indexText;

    public int profileIndex = 0;

    public LocalizeStringEvent locStrEvent;

    [Inject] ProfilesHandler profilesHandler;



    public void DeleteProfile()
    {
        CallConfirm(() =>
        {
            profilesHandler.RemoveFromProfilesList(profileIndex);
        });
    }

    public void SetDATA(ProfileDATA _profileDATA, int _index)
    {
        locStrEvent.SetEntry(_profileDATA.localeEntryIndex.ToString());
        selectedTextGO.SetActive(profilesHandler.IsSelectedProfile(_index));
        newProfileTextGO.SetActive(_profileDATA.isNew);
        profileIndex = _index;
        indexText.text = (profileIndex + 1).ToString();
        clickToSelectTextGO.SetActive(false);

    }


    public void OnSelectProfile()
    {
        //if(!profilesHandler.profiles1[profileIndex].isNew)
        // {
        //     gameplaySavesHandler.SetSavesFromSelectedProfile(profilesHandler.profiles1[profileIndex].gameplaySaveDATAs);        
        //     gameplaySavesHandler.OpenGSavesWindow(this, SaveState.Load);
        // }

        if (profilesHandler.profiles1[profileIndex].isNew && !profilesHandler.IsSelectedProfile(profileIndex))
            profilesHandler.SetSelectedProfile(profileIndex);


    }

    public void OnMouseEnterToProfile()
    {
        if (!profilesHandler.IsSelectedProfile(profileIndex))
        {
            clickToSelectTextGO.SetActive(true);
        }
        else if (!profilesHandler.profiles1[profileIndex].isNew)
            clickToOpenSavesTextGO.SetActive(true);


    }

    public void OnMouseExitToProfile()
    {
        clickToSelectTextGO.SetActive(false);
        clickToOpenSavesTextGO.SetActive(false);

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
