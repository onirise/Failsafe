using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using Zenject;

public class Profile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ProfileDATA DATA;
    
    public GameObject clickToSelectTextGO;

    public GameObject clickToOpenSavesTextGO;

    public GameObject selectedTextGO;

    public GameObject newProfileTextGO;
   

    public TMP_Text indexText;

    //public bool selected; 
    //public int localeEntryIndex;

    //public int profileID = 1;

    public LocalizeStringEvent locStrEvent;

    [Inject] ProfilesHandler profilesHandler;
    [Inject] GameplaySavesHandler gameplaySavesHandler;
    [Inject] TabletHandler tabletHandler;
    

    public void SetDATA(ProfileDATA _profileDATA)
    {
        DATA = _profileDATA;
        UpdateProfileUI();
    }

    public void SetNewLocaleEntry()
    {
        var stringTable = LocalizationSettings.StringDatabase.GetTable("ProfileNamesTable"); 
        DATA.localeEntryIndex = Random.Range(1, stringTable.Count+1);
        Debug.Log(DATA.localeEntryIndex);
    }
           
     

    public void UpdateProfileUI()
    {
        indexText.text = DATA.profileID.ToString();
        locStrEvent.SetEntry(DATA.localeEntryIndex.ToString());
        selectedTextGO.SetActive(DATA.selected);
        newProfileTextGO.SetActive(DATA.isNew);
    }

   

    public void DeselectProfile()
    {
        DATA.selected = false;
        selectedTextGO.SetActive(false);
        
    }

    public void OnSelectProfile()
    {   
        if(!DATA.isNew || DATA.isNew && DATA.selected)
        {
            gameplaySavesHandler.SetSavesFromSelectedProfile(DATA.gameplaySaveDATAs);        
            gameplaySavesHandler.OpenGSavesWindow(this, SaveState.Load);
        }

        if(DATA.isNew && !DATA.selected)
        SelectCLickedProfile();
       
        
            
       
            
      
        
            
    }

    public void SelectCLickedProfile()
    {        
        if(!DATA.selected)
        {
            foreach (var item in profilesHandler.profiles)
            {
                item.DeselectProfile();
            }
            DATA.selected = true;
            selectedTextGO.SetActive(true);
            tabletHandler.playButton.SetPlayButtonInteractable(true);
            clickToSelectTextGO.SetActive(false);
            //gameplaySavesHandler.SetSavesFromSelectedProfile(DATA.gameplaySaveDATAs);
            SaveManager.SaveAll();
        }
    }

    public void OnMouseEnterToProfile()
    {
        if(!DATA.selected)
        {
            clickToSelectTextGO.SetActive(true);
        }
        else
            clickToOpenSavesTextGO.SetActive(true);

            
    }

    public void OnMouseExitToProfile()
    {
        if(!DATA.selected)
        {
            clickToSelectTextGO.SetActive(false);
        }
        else
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
