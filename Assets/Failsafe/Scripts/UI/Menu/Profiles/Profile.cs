using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using Zenject;

public class Profile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ProfileDATA data;
    public bool selected = false;
    public GameObject clickToSelectTextGO;
    public GameObject selectedTextGO;

    public TMP_Text nameText;

    public TMP_Text indexText;

    public int localeEntryIndex;

    public int profileID = 1;

    public LocalizeStringEvent locStrEvent;

    [Inject] ProfilesHandler profilesHandler;
    [Inject] GameplaySavesHandler gameplaySavesHandler;
    [Inject] SaveManager saveManager;

    void Start()
    {
        
    }

    public void SetNewLocaleEntry()
    {
        var stringTable = LocalizationSettings.StringDatabase.GetTable("ProfileNamesTable"); 
        localeEntryIndex = Random.Range(1, stringTable.Count+1);
        Debug.Log(localeEntryIndex);
    }
           
     

    public void UpdateProfileUI()
    {
        indexText.text = profileID.ToString();
        locStrEvent.SetEntry(localeEntryIndex.ToString());
        selectedTextGO.SetActive(selected);
    }

   

    public void DeselectProfile()
    {
        selected = false;
        selectedTextGO.SetActive(false);
        
    }

    public void OnSelectProfile()
    {
        if(!selected)
        {
            foreach (var item in profilesHandler.profiles)
            {
                item.DeselectProfile();
            }
            selected = true;
            selectedTextGO.SetActive(true);
            clickToSelectTextGO.SetActive(false);
            gameplaySavesHandler.SetSavesFromProfile(data.gameplaySaveDATAs);
            saveManager.SaveAll();
        }
            
    }

    public void OnMouseEnterToProfile()
    {
        if(!selected)
        {
            clickToSelectTextGO.SetActive(true);
        }

            
    }

    public void OnMouseExitToProfile()
    {
        if(!selected)
        {
            clickToSelectTextGO.SetActive(false);
        }

            
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

    #region LOAD
     public void LoadProfile(ProfileDATA _data)
    {
        localeEntryIndex = _data.localeEntryIndex;
        profileID = _data.profileID;
        selected = _data.selected;
        UpdateProfileUI();
    }

    #endregion
}
