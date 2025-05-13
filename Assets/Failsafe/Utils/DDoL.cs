using UnityEngine;

public class DDoL : MonoBehaviour
{
    private void Awake() => DontDestroyOnLoad(gameObject);
}
