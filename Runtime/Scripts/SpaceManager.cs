using System;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    [SerializeField] private DownPanel bedroomPanel;
    public event Action OnSpaceFinished;
    
    private void OnEnable()
    {
        bedroomPanel.OnMaxObjectAnswered += HandleBedroomFinished;
    }

    private void OnDisable()
    {
        bedroomPanel.OnMaxObjectAnswered -= HandleBedroomFinished;
    }

    private void HandleBedroomFinished()
    {
        OnSpaceFinished?.Invoke();
    }
}