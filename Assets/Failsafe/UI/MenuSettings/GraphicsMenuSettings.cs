using UnityEngine;
using UnityEngine.Rendering.PostProcessing; 
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GraphicsMenuSettings : MonoBehaviour
{
    public Slider maxFPS;
    public Slider gamma;
    public GameObject saveButton;
    public GameObject defaultButton;
    public SwitcherScript resolution;
    public SwitcherScript shadowQuallity;
    public SwitcherScript modelQuality;
    public SwitcherScript MaxFPS;
    public SwitcherScript screenModeDropdown;

    Resolution[] resolutions;

    public SwitcherScript[] shadowQList;
    public SwitcherScript[] modelQList;
    public SwitcherScript[] FPSList;

    private readonly FullScreenMode[] screenModes = {
        FullScreenMode.ExclusiveFullScreen,
        FullScreenMode.FullScreenWindow,
        FullScreenMode.Windowed
    };


    public Toggle vSyncToggle;
    public Toggle motionBlurToggle;
    public Toggle bloomToggle;

    private MotionBlur motionBlur;
    private Bloom bloom;
    public PostProcessVolume postProcessVolume;



    public void OnMaxFPSChange()
    {

        Debug.Log(maxFPS.value);



    }

    public void OnMaxGammaChange()
    {

        Debug.Log(gamma.value);



    }

    public void  OnSaveButtonClick()
    {   
        Debug.Log("Settings button pressed");
        

    }

    public void  OnDefaultButtonClick()
    {   
        Debug.Log("Settings button pressed");


    }

    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out motionBlur);
        postProcessVolume.profile.TryGetSettings(out bloom);


        
        InitializeScreenModeDropdown();
        InitializeVSyncToggle();
        InitializeMotionBlurToggle();
        InitializeBloomToggle();
        InitializeMaxFPSDropdown();
        InitializeShadowDropdown();
        InitializeModelDropdown();
        StartResolution();
    }


    void StartResolution()
    {
        resolution.options.Clear();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRateRatio + "Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;


        }
        
        resolution.AddOptions(options);

        LoadSettings(currentResolutionIndex);

    }

    private void InitializeVSyncToggle()
    {
        
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
        vSyncToggle.onValueChanged.AddListener(SetVSync);
    }

    private void InitializeMotionBlurToggle()
    {
        
        motionBlurToggle.isOn = motionBlur != null && motionBlur.active;
        motionBlurToggle.onValueChanged.AddListener(SetMotionBlur);
    }

    private void InitializeBloomToggle()
    {
        
        bloomToggle.isOn = bloom != null && bloom.active;
        bloomToggle.onValueChanged.AddListener(SetBloom);
    }

    public void SetVSync(bool isVSync)
    {
        QualitySettings.vSyncCount = isVSync ? 1 : 0;
        Debug.Log($"VSync is {(isVSync ? "ON" : "OFF")}");
        
    }

    public void SetMotionBlur(bool isMotionBlur)
    {
        if (motionBlur != null)
        {
            motionBlur.active = isMotionBlur;
            Debug.Log($"Motion Blur {(isMotionBlur ? "ON" : "OFF")}");
        }


    }

    public void SetBloom(bool isBloom)
    {
        if (bloom != null)
        {
            bloom.active = isBloom;
            Debug.Log($"Bloom {(isBloom ? "ON" : "OFF")}");
        }


    }


    private void InitializeScreenModeDropdown()
    {
        screenModeDropdown.options.Clear();
        List<string> options = new List<string> {
            "Fullscreen",
            "Borderless",
            "Windowed"
        };

        screenModeDropdown.AddOptions(options);
        SetCurrentScreenModeIndex(); 

    }
    
    private void InitializeMaxFPSDropdown()
    {
        MaxFPS.options.Clear();

        List<string> options = new()
        {
            "60 fps",
            "30 fps",
            "15 fps",
        };
        MaxFPS.AddOptions(options);


    }
    
    private void InitializeShadowDropdown()
    {
        shadowQuallity.options.Clear();
        List<string> options = new List<string> {
            "Low",
            "Medium",
            "High"
        };

        shadowQuallity.AddOptions(options);
 

    }
    
    private void InitializeModelDropdown()
    {
        modelQuality.options.Clear();
        List<string> options = new List<string> {
            "Low",
            "Medium",
            "High"
        };

        modelQuality.AddOptions(options);


    }


    private void SetCurrentScreenModeIndex()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                screenModeDropdown.SetCurrentOption(0);
                break;
            case FullScreenMode.FullScreenWindow:
                screenModeDropdown.SetCurrentOption(1);
                break;
            case FullScreenMode.Windowed:
                screenModeDropdown.SetCurrentOption(2);
                break;
            default:
                screenModeDropdown.SetCurrentOption(1);
                break;
        }
        
    }

    public void SetScreenMode(int modeIndex)
    {
        if (modeIndex >= 0 && modeIndex < screenModes.Length)
        {
            Screen.fullScreenMode = screenModes[modeIndex];
            Debug.Log($"Screen mode set to: {screenModes[modeIndex]}");
        }
    }

  

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution1 = resolutions[resolutionIndex];
        Screen.SetResolution(resolution1.width, resolution1.height, Screen.fullScreen);
    }
    
    public void SetShadow(int shadowIndex)
    {
        if (shadowIndex >= 0 && shadowIndex < shadowQList.Length)
        {
            shadowQuallity.SetCurrentOption(shadowIndex);
            Debug.Log($"Shadow quality was set to: {shadowQList[shadowIndex]}");

        }
    }
    
    public void SetModel(int modelIndex)
    {
        if (modelIndex >= 0 && modelIndex < modelQList.Length)
        {
            modelQuality.SetCurrentOption(modelIndex);
            Debug.Log($"Model quality was set to: {modelQList[modelIndex]}");

        }
    }
    
    public void SetFPSMAX(int FPSIndex)
    {
        if (FPSIndex >= 0 && FPSIndex < FPSList.Length)
        {
            MaxFPS.SetCurrentOption(FPSIndex);
            Debug.Log($"FPS was set to: {modelQList[FPSIndex]}");

        }
    }

    public void OnResolutionDropDownClick()
    {
        

    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", resolution.GetCurrentOption());
        PlayerPrefs.SetInt("ScreenModePreference", screenModeDropdown.GetCurrentOption());
        PlayerPrefs.SetInt("VSyncEnabled", vSyncToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("ShadowQualityPreference", shadowQuallity.GetCurrentOption());
        PlayerPrefs.SetInt("ModelQualityPreference", modelQuality.GetCurrentOption());
        PlayerPrefs.SetInt("MaxFPS", MaxFPS.GetCurrentOption());
        PlayerPrefs.SetInt("motionBlurEnabled", motionBlurToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("bloomToggleEnabled", bloomToggle.isOn ? 1 : 0);

    }

    
    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolution.SetCurrentOption(PlayerPrefs.GetInt("ResolutionPreference"));
        else
            resolution.SetCurrentOption(currentResolutionIndex);

        if (PlayerPrefs.HasKey("ScreenModePreference"))
            screenModeDropdown.SetCurrentOption(PlayerPrefs.GetInt("ScreenModePreference"));
        
        if (PlayerPrefs.HasKey("ShadowQualityPreference"))
            shadowQuallity.SetCurrentOption(PlayerPrefs.GetInt("ShadowQualityPreference"));
        
        if (PlayerPrefs.HasKey("ModelQualityPreference"))
            modelQuality.SetCurrentOption(PlayerPrefs.GetInt("ModelQualityPreference"));
        
        if (PlayerPrefs.HasKey("MaxFPS"))
            MaxFPS.SetCurrentOption(PlayerPrefs.GetInt("MaxFPS"));

        if (PlayerPrefs.HasKey("VSyncEnabled"))
            vSyncToggle.isOn=PlayerPrefs.GetInt("VSyncEnabled") > 0;
        
        if (PlayerPrefs.HasKey("motionBlurEnabled"))
            motionBlurToggle.isOn=PlayerPrefs.GetInt("motionBlurEnabled") > 0;
        
        if (PlayerPrefs.HasKey("VSyncEnabled"))
            bloomToggle.isOn=PlayerPrefs.GetInt("bloomToggleEnabled") > 0;


    



    }










}
