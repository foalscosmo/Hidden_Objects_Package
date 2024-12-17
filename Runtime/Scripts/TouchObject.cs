using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchObject : MonoBehaviour
{
    private Camera mainCamera;
    private Finger touchFinger;
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

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += HandleFingerDown;
    }

    private void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= HandleFingerDown;
        EnhancedTouchSupport.Disable();
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < targetObjects.Count; i++)
        {
            var obj = Instantiate(prefabObject, targetObjects[i].transform.position, quaternion.identity,transform);
            obj.GetComponent<SpriteRenderer>().sprite = objectSpritesRenderers[i];
            correctStarParticle[i] = obj.transform.GetChild(0).gameObject;
            clonedPrefabs[i] = obj.transform;
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (touchFinger != null) return;

        Vector2 touchPosition = GetTouchWorldPosition(touchedFinger.screenPosition);

        for (var index = 0; index < targetObjects.Count; index++)
        {
            if (UntouchableObjects.Contains(index)) continue;
            var currentTarget = clonedPrefabs[index];
            if (Vector2.Distance(touchPosition, currentTarget.position) < 1.3f)
            {
                stayedObjs[index] = 99;
                touchFinger = touchedFinger;
                currentObjectIndex = index;
                OnCorrectTouch?.Invoke();
                OnCorrectInvokeIndex?.Invoke(index);
                correctStarParticle[index].gameObject.SetActive(true);
                UntouchableObjects.Add(index);
                var originalScale = currentTarget.localScale;
                
                
                currentTarget.DOScale(1.2f, 0.3f).OnComplete(() =>
                {
                    currentTarget.DOScale(originalScale, 0.3f);
                });
            }
        }

        touchFinger = null;
    }

    private Vector2 GetTouchWorldPosition(Vector2 screenPosition)
    {
        return mainCamera.ScreenToWorldPoint(screenPosition);
    }
}