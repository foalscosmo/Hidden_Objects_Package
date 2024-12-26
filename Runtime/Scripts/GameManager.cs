using System.Collections;
using com.appidea.MiniGamePlatform.CommunicationAPI;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

namespace com.appidea.MiniGamePlatform.Hidden_Objects.Hidden_Objects.Runtime.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BedroomManager bedroomManager;
        [SerializeField] private GardenManager gardenManager;
        [SerializeField] private SpaceManager spaceManager;
        [SerializeField] private DownPanel lastPanel;
        [SerializeField] private int levelIndex;
        [SerializeField] private Button exitButton;
        private BaseMiniGameEntryPoint _entryPoint;

        private void OnEnable()
        {
            TouchSimulation.Enable();
            bedroomManager.OnBedroomFinished += SetLevel;
            gardenManager.OnGardenFinished += SetLevel;
            spaceManager.OnSpaceFinished += SetLevel;
        }

        private void OnDisable()
        {
            TouchSimulation.Disable();
            bedroomManager.OnBedroomFinished -= SetLevel;
            gardenManager.OnGardenFinished -= SetLevel;
            spaceManager.OnSpaceFinished -= SetLevel;
        }

        private void Awake()
        {
            Application.targetFrameRate = 120;
            SetLevel();
            exitButton.onClick.AddListener(SetFinishOnButton);
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

        private void SetFinishOnButton()
        {
            _entryPoint.InvokeGameFinished();
        }

        private IEnumerator FinishAfterFireWorks()
        {
            yield return new WaitForSecondsRealtime(5f);
            _entryPoint.InvokeGameFinished();
        }
    }
}
