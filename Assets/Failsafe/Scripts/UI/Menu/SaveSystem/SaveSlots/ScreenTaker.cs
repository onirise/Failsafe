using System.IO;
using UnityEngine;


public class ScreenTaker : MonoBehaviour
{
    Camera _cam;

    public string SaveCameraView(SaveSlotDATA _GSave)
    {
        _cam.gameObject.SetActive(true);
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        _cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        _cam.Render();

        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;

        string link = Path.Combine(Application.persistentDataPath, $"Profile{ProfilesHandler.GetSelectedProfileIndex()}-" + //gameplaySavesHandler.profileParent.DATA.profileID
                           $"Slot{SaveSlotsHandler.SaveSlots.IndexOf(_GSave)}.jpg");
        byte[] byteArray = renderedTexture.EncodeToJPG();
        File.WriteAllBytes(link, byteArray);
        _cam.gameObject.SetActive(false);
        return link;

    }

}
