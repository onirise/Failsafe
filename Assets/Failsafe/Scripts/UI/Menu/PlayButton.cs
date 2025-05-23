using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetPlayButtonInteractable(bool flag)
    {
        button.interactable = flag; 
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
