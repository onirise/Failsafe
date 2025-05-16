using UnityEngine;

public class OpenCloseProfilesButton : MonoBehaviour
{
    public GameObject profilesMainGO;
    public GameObject mainMenuMainGO;
    
    public void OnProfilesOpen()
    {
        mainMenuMainGO.SetActive(!mainMenuMainGO.activeSelf);
        profilesMainGO.SetActive(!profilesMainGO.activeSelf);
    } 
}
