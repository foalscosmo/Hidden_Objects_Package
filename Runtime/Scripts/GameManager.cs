using System.Collections;
using com.appidea.MiniGamePlatform.CommunicationAPI;
using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BedroomManager bedroomManager;
        [SerializeField] private GardenManager gardenManager;
        [SerializeField] private SpaceManager spaceManager;
        [SerializeField] private DownPanel lastPanel;
        [SerializeField] private int levelIndex;
        private BaseMiniGameEntryPoint _entryPoint;

        private void OnEnable()
        {
            bedroomManager.OnBedroomFinished += SetLevel;
            gardenManager.OnGardenFinished += SetLevel;
            spaceManager.OnSpaceFinished += SetLevel;
        }

        private void OnDisable()
        {
            bedroomManager.OnBedroomFinished -= SetLevel;
            gardenManager.OnGardenFinished -= SetLevel;
            spaceManager.OnSpaceFinished -= SetLevel;
        }

        private void Awake()
        {
            SetLevel();
        }

        private void SetLevel()
        {
            switch (levelIndex)
            {
                case 0:
                    bedroomManager.gameObject.SetActive(true);
                    levelIndex++;
                    break;
                case 1:
                    bedroomManager.gameObject.SetActive(false);
                    gardenManager.gameObject.SetActive(true);
                    levelIndex++;
                    break;
                case 2:
                    gardenManager.gameObject.SetActive(false);
                    spaceManager.gameObject.SetActive(true);
                    levelIndex++;
                    break;
                case 3:
                    lastPanel.gameObject.SetActive(false);
                    SetFinishForPackage();
                    break;
            }
        }

        public void SetEntryPoint(BaseMiniGameEntryPoint entryPoint)
        {
            _entryPoint = entryPoint;
        }

        private void SetFinishForPackage()
        {
            StartCoroutine(FinishAfterFireWorks());
        }

        private IEnumerator FinishAfterFireWorks()
        {
            yield return new WaitForSecondsRealtime(5f);
            _entryPoint.InvokeGameFinished();
        }
    }
