using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
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

    //public string screenshotLink;
    public RawImage savePreview;

    public LocalizeStringEvent timeLocalizeStringEvent;

    public Camera _cam;

    [Inject] SaveManager saveManager;
    [Inject] GameplaySavesHandler gameplaySavesHandler;
    [Inject] TabletHandler tabletHandler;
    public void UpdateGameplaySaveUI()
    {
        
        selectedTextGO.SetActive(DATA.selected);
        TimeSpan timeSpan = TimeSpan.FromSeconds(tabletHandler.time);
        string timeFormatted = timeSpan.ToString(@"hh\:mm\:ss");
        timeLocalizeStringEvent.StringReference.Arguments = new object[] { timeFormatted };
        timeLocalizeStringEvent.RefreshString();
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
        DATA.selected = false;
        selectedTextGO.SetActive(false);
        
    }

    public void OnSelectGameplaySave()
    {
        
            foreach (var item in gameplaySavesHandler.gameplaySaves)
            {
                item.DeselectGameplaySave();
            }

            int newSaveNubmer = gameplaySavesHandler.GetNewSavesID();
            /*
            DATA.screenshotLink = $"Assets/Failsafe/Scripts/UI/Menu/SaveSystem/GameplaySaves/Screen{newSaveNubmer}.png";
            ScreenCapture.CaptureScreenshot($"Assets/Failsafe/Scripts/UI/Menu/SaveSystem/GameplaySaves/Screen{newSaveNubmer}.png");
            */
            SaveCameraView(_cam, newSaveNubmer);
            DATA.screenshotLink = $"Assets/Failsafe/Scripts/UI/Menu/SaveSystem/GameplaySaves/cameracapture{newSaveNubmer}.jpg";

            byte[] fileData = File.ReadAllBytes(DATA.screenshotLink);
            

            Texture2D loadedTexture = new Texture2D(2, 2); // Временные размеры (автоматически изменятся)
            loadedTexture.LoadImage(fileData);
            savePreview.texture = loadedTexture;
            

            DATA.selected = true;
            UpdateGameplaySaveUI();
            clickToSelectTextGO.SetActive(false);
            saveManager.SaveAll();
       
            
    }

    void SaveCameraView(Camera cam, int _newSaveNubmer)
{
    RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
    cam.targetTexture = screenTexture;
    RenderTexture.active = screenTexture;
    cam.Render();

    Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
    renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    RenderTexture.active = null;

    byte[] byteArray = renderedTexture.EncodeToJPG();
    File.WriteAllBytes($"Assets/Failsafe/Scripts/UI/Menu/SaveSystem/GameplaySaves/cameracapture{_newSaveNubmer}.jpg", byteArray);
}

    public void OnMouseEnterToGameplaySave()
    {
       
            clickToSelectTextGO.SetActive(true);
    

            
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
        OnSelectGameplaySave();
    
    }
/*
    #region LOAD

    public void LoadGameplaySave(GameplaySaveDATA _data)
    {
        
        
        selected = _data.selected;
        UpdateProfileUI();
    }

    #endregion
    */
}
