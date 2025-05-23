using System.IO;
using UnityEngine;

public class ScreenTaker : MonoBehaviour
{
    public Camera _cam;
    public void SaveCameraView(int _newSaveNubmer)
    {
        _cam.gameObject.SetActive(true);
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        _cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        _cam.Render();

        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;

        byte[] byteArray = renderedTexture.EncodeToJPG();
        File.WriteAllBytes($"Assets/Failsafe/Scripts/UI/Menu/SaveSystem/GameplaySaves/cameracapture{_newSaveNubmer}.jpg", byteArray);
        _cam.gameObject.SetActive(false);
    }
    
}
