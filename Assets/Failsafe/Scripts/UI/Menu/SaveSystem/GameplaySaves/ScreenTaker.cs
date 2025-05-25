using System.IO;
using UnityEngine;
using Zenject;

public class ScreenTaker : MonoBehaviour
{
    public Camera _cam;
    [Inject] GameplaySavesHandler gameplaySavesHandler;
    public string SaveCameraView(GameplaySave _GSave)
    {
        _cam.gameObject.SetActive(true);
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        _cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        _cam.Render();

        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;

        string link = Path.Combine(Application.persistentDataPath, $"Profile{Random.Range(1, 1000)}-" + //gameplaySavesHandler.profileParent.DATA.profileID
                          $"Slot{gameplaySavesHandler.gameplaySaves.IndexOf(_GSave)}.jpg");
        byte[] byteArray = renderedTexture.EncodeToJPG();
        File.WriteAllBytes(link, byteArray);
        _cam.gameObject.SetActive(false);
        return link;
    }

}
