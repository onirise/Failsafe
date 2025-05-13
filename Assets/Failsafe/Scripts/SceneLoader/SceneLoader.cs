using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Failsafe.Scripts.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        public UniTask LoadSceneAsync(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName).ToUniTask();
        }

        public UniTask LoadSceneAsync(string sceneName, LoadSceneParameters parameters)
        {
            return SceneManager.LoadSceneAsync(sceneName, parameters).ToUniTask();
        }
    }
}