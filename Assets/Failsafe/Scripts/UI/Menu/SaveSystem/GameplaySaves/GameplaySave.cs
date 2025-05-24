using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameplaySave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameplaySaveDATA DATA;
    //public bool selected = false;

    [SerializeField]
    LocalizeStringEvent nameTextLocEvent;

    public GameObject clickToSelectTextGO;
    public GameObject selectedTextGO;

    public GameObject clearButtonGO;

    //public string screenshotLink;
    public RawImage savePreview;

    public LocalizeStringEvent timeLocalizeStringEvent;
    
    bool isStartAutosave = false;

    //[Inject] SaveManager saveManager;
    [Inject] GameplaySavesHandler gameplaySavesHandler;
    [Inject] TabletHandler tabletHandler;
    [Inject] ScreenTaker screenTaker;
    public void UpdateGameplaySaveUI(bool _setStartAutosave = false)
    {
        isStartAutosave = _setStartAutosave;
        if(isStartAutosave)
            clearButtonGO.SetActive(false);

        selectedTextGO.SetActive(DATA.lastSave);
        TimeSpan timeSpan = TimeSpan.FromSeconds(DATA.time); //tabletHandler.time
        string timeFormatted = timeSpan.ToString(@"hh\:mm\:ss");
        timeLocalizeStringEvent.StringReference.Arguments = new object[] { timeFormatted };
        timeLocalizeStringEvent.RefreshString();

        //временно взял этот код вообще из другого места
         if(DATA.screenshotLink != "")
        {
            byte[] fileData = File.ReadAllBytes(DATA.screenshotLink);    
            Texture2D loadedTexture = new Texture2D(2, 2); // Временные размеры (автоматически изменятся)
            loadedTexture.LoadImage(fileData);
            savePreview.texture = loadedTexture;
        }
        else
        {
            savePreview.texture = null;
        }
    }

    

    public void SetNewDATA(string _scrLink, bool _lastSave, float _time, bool _isEmpty)
    {
        DATA.screenshotLink = _scrLink;
        DATA.lastSave = _lastSave;
        DATA.time = _time;
        DATA.isEmpty = _isEmpty;
    }

    public void ClearDATA()
    {
        SetNewDATA("", false, 0, true);        
        UpdateGameplaySaveUI();
        SaveManager.SaveAll();
        for (int i = gameplaySavesHandler.gameplaySaves.Count-1; i >= 0; i--)
        {
            if(!gameplaySavesHandler.gameplaySaves[i].DATA.isEmpty)
            {
                gameplaySavesHandler.gameplaySaves[i].DATA.lastSave = true;
                gameplaySavesHandler.gameplaySaves[i].selectedTextGO.SetActive(true);
                break;
            }
                

        }
        
        
    }
   

    public void SetSaveName(string _entryName, int id)
    {
        nameTextLocEvent.SetEntry(_entryName);
        if(id != -1)
            nameTextLocEvent.StringReference.Arguments = new object[] { id };
        nameTextLocEvent.RefreshString();
    }


    public void DeselectGameplaySave()
    {
        DATA.lastSave = false;
        selectedTextGO.SetActive(false);
        
    }

    public void OnSaveGameplaySave()
    {
        
            foreach (var item in gameplaySavesHandler.gameplaySaves)
            {
                item.DeselectGameplaySave();
            }

            
            string link = screenTaker.SaveCameraView(this);
            SetNewDATA(link, true, tabletHandler.time, false);
            UpdateGameplaySaveUI();
            clickToSelectTextGO.SetActive(false);
            gameplaySavesHandler.profileParent.SelectCLickedProfile();
            SaveManager.SaveAll();
            //gameplaySavesHandler.gameplaySavesGO.SetActive(false);
           
       
            
    }

    public void OnLoadGameplaySave()
    {
        if(!DATA.isEmpty)
        {
            clickToSelectTextGO.SetActive(false);
            gameplaySavesHandler.profileParent.SelectCLickedProfile();
            gameplaySavesHandler.gameplaySavesGO.SetActive(false);
            SaveManager.SaveAll();
        }
        
    }

    

    public void OnMouseEnterToGameplaySave()
    {
        if((!isStartAutosave && gameplaySavesHandler.saveState == SaveState.Save) ||  (!DATA.isEmpty && gameplaySavesHandler.saveState == SaveState.Load))
        {           
                clickToSelectTextGO.SetActive(true);            
        }
           
    

            
    }

    public void OnMouseExitToGameplaySave()
    {
        
            clickToSelectTextGO.SetActive(false);
      

            
    }

   
    // Использовал ивенты таким образом, ибо если использовать компонент Event trigger,
    // то не будет работать скроллинг профилей пока мышка находится на одном из профилей
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterToGameplaySave();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitToGameplaySave();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(gameplaySavesHandler.saveState == SaveState.Save && !isStartAutosave)
        {
            OnSaveGameplaySave();
        }
        else
            OnLoadGameplaySave();
        
    
    }

}
