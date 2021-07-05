using GameManagers;
using Gameplay.Enemy.EnemiesBase;
using Gameplay.Player.Item;
using Gameplay.Player.Motion;
using UI.EndGamePuzzle;
using UI.ToolTip;
using UnityEngine;
using Utilities;
using Utilities.Audio;
using Utilities.UI;

namespace Gameplay.Objects.Interaction
{
    public class EndGameDoorController : InteractionObjectWithColliders
    {
        private EndGamePuzzleController _endGamePuzzleController;
        private bool _enabled;
        private bool _runningPuzzle;
        [SerializeField] private DoorController _door;
        [SerializeField] private BasherEnemy _enemy;
        [SerializeField] private ToolTip _doorToolTip;
        [SerializeField] private BoxCollider _corridorC8Collider;

        // TODO: Substituir Depois por uma porta com os 3 indicadores
        // Referência para o Basher/EnemiesManager (para enviar um evento de perseguir player na porta)

        private void Start()
        {
            _enabled = true;
            _runningPuzzle = false;

            _door.LockDoor();

            _endGamePuzzleController = new EndGamePuzzleController();
            _endGamePuzzleController.onBarCompleted = HandleCurrentBarCompleted;
            _endGamePuzzleController.onAllBarsCompleted = HandlePuzzleCompleted;

            GameplayManager.instance.onPlayerDamaged += delegate (int p_damage)
            {
                if (_runningPuzzle)
                    InterruptPuzzle();
            };
        }

        public override void Interact()
        {
            if (!_runningPuzzle && _enabled && GameplayManager.instance.inventoryController.inventoryList.Contains(ItemEnum.KEYCARD))
            {
                _doorToolTip.InteractToolTip();
                _runningPuzzle = true;
                PlayerStatesManager.SetPlayerState(PlayerState.INTERACT_WITH_ENDLEVEL_DOOR);
                _endGamePuzzleController.StartLoading();
            }
            else
            {
                InputController.GamePlay.InputEnabled = false;
                PlayerStatesManager.SetPlayerState(PlayerState.PRESS_BUTTON);
                AudioManager.instance.Play(AudioNameEnum.ITEM_LANTERN_PICKUP_DENIED, false, () =>
                {
                    GameHudManager.instance.uiDialogHud.StartDialog(DialogEnum.ACT_03_KEYKARD_NEEDED);
                });
            }
        }

        public override void RunFixedUpdate()
        {
            if (_runningPuzzle && InputController.GamePlay.NavigationAxis() != Vector3.zero)
            {
                InterruptPuzzle();
            }
        }

        private void InterruptPuzzle()
        {
            _runningPuzzle = false;
            PlayerStatesManager.SetPlayerState(PlayerState.EXITED_ENDLEVEL_DOOR_AREA);
            _endGamePuzzleController.StopLoading();
        }

        private void HandleCurrentBarCompleted(int p_currentBar)
        {
            _door.UnlockDoorLock();

            _enemy.TeleportToEndgameDoorSpawn(transform.position);
        }

        private void HandlePuzzleCompleted()
        {
            _runningPuzzle = false;
            _corridorC8Collider.enabled = true;
            PlayerStatesManager.SetPlayerState(PlayerState.EXITED_ENDLEVEL_DOOR_AREA);
            GameHudManager.instance.endGameUI.HideUI();
            _door.Interact();

            _enabled = false;
            AudioManager.instance.PlayMusic(AudioNameEnum.SOUND_TRACK_ESCAPE, 1);
        }
    }
}