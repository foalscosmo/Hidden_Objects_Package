using System.Threading.Tasks;
using com.appidea.MiniGamePlatform.CommunicationAPI;
using com.appidea.MiniGamePlatform.Hidden_Objects.Hidden_Objects.Runtime.Scripts;
using UnityEngine;

namespace com.appidea.MiniGamePlatform.Hidden_Objects.Hidden_Objects.Runtime
{
    public class HiddenObjectEntryPoint : BaseMiniGameEntryPoint
    {
        [SerializeField] private GameObject gamePrefab;
        protected override Task LoadInternal()
        {
            var gameManager = Instantiate(gamePrefab);
            gameManager.GetComponent<GameManager>().SetEntryPoint(this);
            return Task.CompletedTask;
        }

        protected override Task UnloadInternal()
        {
            return Task.CompletedTask;
        }
    }
}
