using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace com.appidea.MiniGamePlatform.Hidden_Objects.Hidden_Objects.Runtime.Scripts
{
    public class TouchObject : MonoBehaviour
    {
        private Camera mainCamera;
        [SerializeField] private List<Transform> targetObjects = new();
        [SerializeField] private List<Sprite> objectSpritesRenderers = new();
        [SerializeField] private List<Transform> clonedPrefabs = new();
        [SerializeField] private List<GameObject> correctStarParticle = new();
        [SerializeField] private List<int> stayedObjs = new();
        [SerializeField] private GameObject prefabObject;
        [SerializeField] private int maxObjectCount;
        [SerializeField] private int currentObjectIndex;
        private HashSet<int> UntouchableObjects { get; } = new();

        public List<int> StayedObjs
        {
            get => stayedObjs;
            set => stayedObjs = value;
        }

        public int MaxObjectCount => maxObjectCount;
        public event Action OnCorrectTouch;
        public event Action<int> OnCorrectInvokeIndex;

        public int CurrentObjectIndex => currentObjectIndex;

        public List<Transform> ClonePrefabs => clonedPrefabs;
        private readonly List<SpriteRenderer> spriteRenderers = new();


        private void Awake()
        {
            mainCamera = Camera.main;

            for (int i = 0; i < targetObjects.Count; i++)
            {
                var obj = Instantiate(prefabObject, targetObjects[i].transform.position, quaternion.identity, transform);
                var spriteRenderer = obj.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = objectSpritesRenderers[i];
                correctStarParticle[i] = obj.transform.GetChild(0).gameObject;
                clonedPrefabs[i] = obj.transform;
                spriteRenderers.Add(spriteRenderer);
            }
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        HandleTouch(touch.position);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch(Input.mousePosition);
            }
        }

        private void HandleTouch(Vector2 screenPosition)
        {
            Vector2 touchPosition = GetTouchWorldPosition(screenPosition);
            HandleTouchLogic(touchPosition);
        }

        private void HandleTouchLogic(Vector2 touchPosition)
        {
            for (var index = 0; index < targetObjects.Count; index++)
            {
                if (UntouchableObjects.Contains(index)) continue;

                var currentTarget = clonedPrefabs[index];
                if (Vector2.Distance(touchPosition, currentTarget.position) < 0.9f)
                {
                    stayedObjs[index] = 99;
                    currentObjectIndex = index;
                    OnCorrectTouch?.Invoke();
                    OnCorrectInvokeIndex?.Invoke(index);
                    correctStarParticle[index].gameObject.SetActive(true);
                    UntouchableObjects.Add(index);

                    var originalScale = currentTarget.localScale;
                    var index1 = index;
                    currentTarget.DOScale(1.2f, 0.3f).OnComplete(() =>
                    {
                        currentTarget.DOScale(originalScale, 0.3f);
                        var spriteRenderer = spriteRenderers[index1];
                        var color = spriteRenderer.color;
                        var targetColor = new Color(color.r, color.g, color.b, 150f / 255f);
                        DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, targetColor, 0.5f);
                    });
                }
            }
        }

        private Vector2 GetTouchWorldPosition(Vector2 screenPosition)
        {
            return mainCamera.ScreenToWorldPoint(screenPosition);
        }
        
        private void OnDrawGizmos()
        {
            // Visualize the touch interaction area with Gizmos
            Gizmos.color = Color.green; // Color for the Gizmo circle
            foreach (var target in clonedPrefabs)
            {
                Gizmos.DrawWireSphere(target.position, 0.9f); // Draw a wire sphere with a radius of 1f
            }
        }
    }
}