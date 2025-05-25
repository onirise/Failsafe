using Failsafe.Scripts.Configs;
using System;
using UnityEngine;
using Zenject;

namespace Failsafe.Scripts.Bootstrap
{

    public class Bootstrapper : IInitializable
    {
        [Inject] ISceneLoader _sceneLoader;
        [Inject] GameConfig _gameConfig;

        public async void Initialize()
        {
            if (_sceneLoader == null)
                Debug.LogError("_sceneLoader is null!");
            if (_gameConfig == null)
                Debug.LogError("_gameConfig is null!");
            await _sceneLoader.LoadSceneAsync(_gameConfig.MainMenuSceneName);
        }




    }
}