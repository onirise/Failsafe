using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Failsafe.Scripts
{
    public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName);
        UniTask LoadSceneAsync(string sceneName, LoadSceneParameters parameters);
    }
}