using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Utilities.VariableManagement;

namespace GameManagers
{
    public class LightPriorityManager : IFixedUpdateBehaviour
    {
        private readonly List<Light> _sceneLights;
        private readonly List<Light> _playerLights;
        private List<LightDistanceData> _lightDistanceData;

        public LightPriorityManager(
            List<Light> p_sceneLights,
            List<Light> p_playerLights)
        {
            _sceneLights = p_sceneLights;
            _playerLights = p_playerLights;
        }

        public void RunFixedUpdate()
        {
            if (_lightDistanceData == null)
                InitializeLightDistanceData();

            _lightDistanceData.Sort((light1, light2) => light1.distanceFromCamera.CompareTo(light2.distanceFromCamera));
            
            SetLightsRenderMode();
        }

        private void SetLightsRenderMode()
        {
            for (int i = 0; i < _lightDistanceData.Count; i++)
            {
                if (i < VariablesManager.cameraVariables.renderedLightsInCamera)
                    _lightDistanceData[i].light.renderMode = LightRenderMode.ForcePixel;
                else
                    _lightDistanceData[i].light.renderMode = LightRenderMode.ForceVertex;
            }
        }

        private void InitializeLightDistanceData()
        {
            _lightDistanceData = new List<LightDistanceData>();

            foreach (Light __light in _sceneLights)
            {
                if (_playerLights.Contains(__light)) continue;

                _lightDistanceData.Add(new LightDistanceData
                {
                    light = __light,
                    distanceFromCamera = Vector3.Distance(__light.transform.position, Camera.main.transform.position)
                });
            }
        }
    }
}
