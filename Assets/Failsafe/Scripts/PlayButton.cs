using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

   
}
