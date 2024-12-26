using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace com.appidea.MiniGamePlatform.Hidden_Objects.Hidden_Objects.Runtime.Scripts
{
    public class DownPanel : MonoBehaviour
    {
        [SerializeField] private TouchObject touchObject;
        [SerializeField] private List<Image> downPanelObjects = new();
        [SerializeField] private int correctAmountCounter;
        [SerializeField] private GameObject checkContainer;
        [SerializeField] private GameObject checkPrefab;
        [SerializeField] private List<GameObject> checks = new();
        public event Action OnMaxObjectAnswered;
        public event Action OnCurrentLevelFinish;
        private void OnEnable()
        {
            touchObject.OnCorrectInvokeIndex += HandleScaleOfCorrectObject;
        }

        private void OnDisable()
        {
            touchObject.OnCorrectInvokeIndex -= HandleScaleOfCorrectObject;
        }
        private void Awake()
        {
            for (var index = 0; index < downPanelObjects.Count; index++)
            {
                var checkClone = Instantiate(checkPrefab, downPanelObjects[index].transform.position,
                    quaternion.identity, checkContainer.transform);
                checks[index] = checkClone;
                var sr = downPanelObjects[index];
                var color = sr.color;
                color.a = 1f;
                sr.color = color;
            }
        }

        private void HandleScaleOfCorrectObject(int index)
        {
            correctAmountCounter++;
            var originalScale = downPanelObjects[index].transform.localScale;
            downPanelObjects[index].transform.DOScale(originalScale * 1.2f, 0.2f).OnComplete(() =>
            {
                var sr = downPanelObjects[index]; 
                var color = sr.color;
                var targetColor = new Color(color.r, color.g, color.b, 0.5f);
                DOTween.To(() => sr.color, x => sr.color = x, targetColor, 0.5f);
                downPanelObjects[index].transform.DOScale(originalScale, 0.2f).OnComplete(() =>
                    { checks[index].SetActive(true); });
            });
            if (correctAmountCounter == touchObject.MaxObjectCount)
            {
                StartCoroutine(HandleFinishTimer());
            }
        }

        private IEnumerator HandleFinishTimer()
        {
            yield return new WaitForSecondsRealtime(1f);
            OnCurrentLevelFinish?.Invoke();
            yield return new WaitForSecondsRealtime(2f);
            OnMaxObjectAnswered?.Invoke();
        }
    }
}