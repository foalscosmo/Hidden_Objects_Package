using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.appidea.MiniGamePlatform.CommunicationAPI;
using UnityEngine;

public class HiddenObjectEntryPoint : BaseMiniGameEntryPoint
{
    [SerializeField] private GameObject gamePrefab;
    protected override Task LoadInternal()
    {
        var gameManager = Instantiate(gamePrefab);
        gameManager.GetComponent<GameManager>().SetEntryPoint(this);
        throw new System.NotImplementedException();
    }

    protected override Task UnloadInternal()
    {
        throw new System.NotImplementedException();
    }
}
