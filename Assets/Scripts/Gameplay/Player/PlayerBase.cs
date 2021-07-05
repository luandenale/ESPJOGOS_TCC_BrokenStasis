using System;
using GameManagers;
using Gameplay.Player.Animation;
using Gameplay.Player.Health;
using Gameplay.Player.Item;
using Gameplay.Player.Motion;
using Gameplay.Player.Sensors;
using SaveSystem;
using Utilities;
using Utilities.VariableManagement;

namespace Gameplay.Player
{
    public class PlayerBase : IFixedUpdateBehaviour, IUpdateBehaviour
    {
        #region PUBLIC FIELDS
        public Action<int> onPlayerDamaged;

        //TODO 23/07/2020 -> Implement player health gain
        public Action<int> onPlayerHealthIncrease;
        public Action<PlayerSuitEnum> onSuitChange;
        #endregion

        #region PRIVATE FIELDS
        private PlayerContainer _playerContainer;
        private PlayerHealth _playerHealth;
        private PlayerMovement _playerMovement;
        private PlayerSoundColliderActivator _playerSoundColliderActivator;
        private PlayerIlluminationController _playerIlluminationController;
        private PlayerDetectableLightController _playerDetectableLightController;
        #endregion

        public PlayerBase(PlayerContainer p_playerContainer)
        {
            _playerContainer = p_playerContainer;
        }

        public void InitializePlayer()
        {
            PlayerStatesManager.onStateChanged = null;
            PlayerStatesManager.SetPlayerState(PlayerState.STATIC);

            RegisterObjectsGraph();

            onPlayerDamaged += _playerHealth.ReceiveDamage;
            onPlayerHealthIncrease += _playerHealth.IncreaseHealth;
            onSuitChange = HandleSuitChange;
        }

        private void RegisterObjectsGraph()
        {
            _playerIlluminationController = new PlayerIlluminationController(
                _playerContainer.playerIlluminationGameObject
            );

            _playerContainer.playerTunnelBehaviour.InitializePlayerTunnelBehaviour(
                VariablesManager.playerVariables.regularSpeed * VariablesManager.playerVariables.slowSpeedMultiplier,
                _playerContainer.playerTransform,
                _playerContainer.characterController);

            _playerMovement = new PlayerMovement(
                _playerContainer.characterController,
                _playerContainer.playerTransform,
                _playerContainer.playerTunnelBehaviour
            );

            _playerSoundColliderActivator = new PlayerSoundColliderActivator(
                _playerContainer.lowSoundCollider,
                _playerContainer.lowSoundShader,
                _playerContainer.mediumSoundCollider,
                _playerContainer.mediumSoundShader,
                _playerContainer.loudSoundCollider,
                _playerContainer.loudSoundShader
            );

            _playerDetectableLightController = new PlayerDetectableLightController(
                _playerContainer.lightDetectableObject,
                _playerContainer.lightDetectableCollider,
                _playerContainer.lightDetectorLayersToDetect,
                _playerContainer.lightOriginTransform
            );

            RegisterPlayerAnimator();

            _playerHealth = new PlayerHealth();
        }

        private void RegisterPlayerAnimator()
        {
            foreach (PlayerSuitData __playerSuit in _playerContainer.suits)
            {
                PlayerAnimator __playerAnimator = new PlayerAnimator(__playerSuit.suitAnimator, __playerSuit.suitAnimationEventHandler);
            }
        }

        public void RunUpdate()
        {
            if(InputController.GamePlay.ToggleIllumination())
            {
                _playerIlluminationController.SetActive(GameplayManager.instance.inventoryController.inventoryList.Contains(ItemEnum.FLASHLIGHT_BATTERY));
                _playerIlluminationController.Toggle();
            }

            _playerDetectableLightController.RunUpdate();
        }

        public void RunFixedUpdate()
        {
            _playerMovement.RunFixedUpdate();
        }

        // TODO: Transferir para PlayerItemController
        private void HandleSuitChange(PlayerSuitEnum p_playerSuit)
        {
            foreach (PlayerSuitData __playerSuit in _playerContainer.suits)
            {
                __playerSuit.suitGameObject.SetActive(__playerSuit.suitType == p_playerSuit);
            }
        }

        // TODO: Transferir para PlayerItemController
        private PlayerSuitEnum GetActiveSuit()
        {
            foreach (PlayerSuitData __playerSuit in _playerContainer.suits)
            {
                if (__playerSuit.suitGameObject.activeSelf)
                {
                    return __playerSuit.suitType;
                }
            }

            return PlayerSuitEnum.NAKED;
        }

        public SlotSaveData GetPlayerSaveData()
        {
            SlotSaveData __gameSaveData = new SlotSaveData();

            _playerIlluminationController.SetActive(GameplayManager.instance.inventoryController.inventoryList.Contains(ItemEnum.FLASHLIGHT_BATTERY));

            __gameSaveData.playerData.suit = GetActiveSuit();
            __gameSaveData.playerData.position = _playerContainer.playerTransform.position;
            __gameSaveData.playerData.health = _playerHealth.GetPlayerHealth();
            __gameSaveData.playerData.playerIlluminationState = _playerIlluminationController.lanternState;

            return __gameSaveData;
        }

        public void SetPlayerSaveData(SlotSaveData p_gameSaveData)
        {
            _playerContainer.playerTransform.position = p_gameSaveData.playerData.position;
            HandleSuitChange(p_gameSaveData.playerData.suit);
            _playerHealth.SetPlayerHealth(p_gameSaveData.playerData.health);
            _playerIlluminationController.lanternState = p_gameSaveData.playerData.playerIlluminationState;
        }
    }
}
