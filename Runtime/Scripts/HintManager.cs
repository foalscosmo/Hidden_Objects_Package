using DG.Tweening;
using UnityEngine;

namespace com.appidea.MiniGamePlatform.Hidden_Objects.Hidden_Objects.Runtime.Scripts
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField] private TouchObject touchObject;
        [SerializeField] private DownPanel downPanel;
        [SerializeField] private int hintIndex;
        private bool startTimer;
        private float afkTimer;
        private const float AfkCheckInterval = 8f;

        private void OnEnable()
        {
            touchObject.OnCorrectInvokeIndex += HintIndexHandler;
            downPanel.OnMaxObjectAnswered += DeactivateTimer;
        }

        private void OnDisable()
        {
            touchObject.OnCorrectInvokeIndex -= HintIndexHandler;
            downPanel.OnMaxObjectAnswered -= DeactivateTimer;
        }

        private void Awake()
        {
            ActivateTimer();
        }

        private void Update()
        {
            switch (startTimer)
            {
                case true:
                {
                    afkTimer += Time.deltaTime; 
                    if (!(afkTimer >= AfkCheckInterval)) return; 
                    CheckAfk();
                    break;
                }
                default:
                    afkTimer = 0;
                    break;
            }
        }

        private void CheckAfk()
        {
            if (!startTimer) return; 
            afkTimer = 0f;
            HandleHintObject();
        }

        private void HandleHintObject()
        {
            if(hintIndex >= touchObject.MaxObjectCount) return;
            ScaleAndRotate(touchObject.ClonePrefabs[hintIndex].transform);
        }

        private void ScaleAndRotate(Transform target)
        {
            Vector3 originalScale = target.localScale;

            DOTween.Sequence()
                .Append(target.DOScale(originalScale + new Vector3(0.15f, 0.15f, 0.15f), 0.3f))
                .Join(target.DORotate(new Vector3(0, 0, 15f), 0.3f))
                .Append(target.DORotate(new Vector3(0, 0, -15f), 0.3f))
                .Append(target.DORotate(Vector3.zero, 0.3f))
                .Append(target.DOScale(originalScale, 0.3f))
                .SetLoops(1, LoopType.Restart);
        }

        private void ActivateTimer() => startTimer = true;
        private void DeactivateTimer() => startTimer = false;
        private void HintIndexHandler(int index)
        {
            afkTimer = 0;

            foreach (var obj in touchObject.StayedObjs)
            {
                if (obj < 10)
                {
                    hintIndex = obj;
                    break; // Exit the loop once we've found a valid object
                }
            }
        }
    }
}