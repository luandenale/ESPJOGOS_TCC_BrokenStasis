
using UnityEngine;

namespace Utilities.VariableManagement
{
    public static class VariablesManager
    {
        private static PlayerVariablesScriptableObject _playerVariables;
        public static PlayerVariablesScriptableObject playerVariables 
        {
            get
            {
                if(_playerVariables == null) _playerVariables = Resources.Load<PlayerVariablesScriptableObject>("Variables/PlayerVariables");

                return _playerVariables;
            }
        }

        private static CameraVariablesScriptableObject _cameraVariables;
        public static CameraVariablesScriptableObject cameraVariables 
        {
            get
            {
                if(_cameraVariables == null) _cameraVariables = Resources.Load<CameraVariablesScriptableObject>("Variables/CameraVariables");

                return _cameraVariables;
            }
        }

        private static UIVariablesScriptableObject _uiVariables;
        public static UIVariablesScriptableObject uiVariables 
        {
            get
            {
                if(_uiVariables == null) _uiVariables = Resources.Load<UIVariablesScriptableObject>("Variables/UIVariables");

                return _uiVariables;
            }
        }

        private static GameplayVariablesScriptableObject _gameplayVariables;
        public static GameplayVariablesScriptableObject gameplayVariables 
        {
            get
            {
                if(_gameplayVariables == null) _gameplayVariables = Resources.Load<GameplayVariablesScriptableObject>("Variables/GameplayVariables");

                return _gameplayVariables;
            }
        }

        private static EnvironmentVariablesScriptableObject _environmentVariables;
        public static EnvironmentVariablesScriptableObject environmentVariables 
        {
            get
            {
                if(_environmentVariables == null) _environmentVariables = Resources.Load<EnvironmentVariablesScriptableObject>("Variables/EnvironmentVariables");

                return _environmentVariables;
            }
        }
    }
}