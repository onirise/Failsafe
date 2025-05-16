using Failsafe.Scripts.Configs;
using VContainer.Unity;

namespace Failsafe.Scripts.Bootstrap
{
    public class Bootstrapper : IStartable
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly GameConfig _gameConfig;

        public Bootstrapper(ISceneLoader sceneLoader, GameConfig gameConfig)
        {
            _sceneLoader = sceneLoader;
            _gameConfig = gameConfig;
        }
        
        public async void Start()
        {
            //logic after container build & IInitializable
            
            await _sceneLoader.LoadSceneAsync(_gameConfig.MainMenuSceneName);
            
            //game started, main scene loaded
        }
    }
}