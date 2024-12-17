using System;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    [SerializeField] private DownPanel bedroomPanel;
    public event Action OnGardenFinished;
    
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
        OnGardenFinished?.Invoke();
    }
}