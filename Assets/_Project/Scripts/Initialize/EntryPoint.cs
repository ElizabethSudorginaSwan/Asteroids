using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpaceShooter.Initializer
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private CompositionRoot _compositionRoot;

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            _compositionRoot.PlayerMovement.SetLive(true);
            _compositionRoot.PlayerUIPresenter.HideGameOverCanvas();
            _compositionRoot.ScoreManager.ResetScore();
            _compositionRoot.PauseGame.SetPause(false);
            _compositionRoot.PlayerUIPresenter.UpdatePlayerData();
            _compositionRoot.SpawnerUFO.StartSpawning().Forget();
            _compositionRoot.SpawnerAsteroid.StartSpawning().Forget();
        }

        private void Update()
        {
            _compositionRoot.PlayerInput.UpdateInput();
            _compositionRoot.SpawnerUFO.UpdateUFOSpawner();
            _compositionRoot.SpawnerAsteroid.UpdateAsteroidSpawner();
        }
    }
}

