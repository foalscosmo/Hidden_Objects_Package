using System;
using UnityEngine;
using UnityEngine.XR;

public class BedroomManager : MonoBehaviour
{
    [SerializeField] private DownPanel bedroomPanel;
    public event Action OnBedroomFinished;
    
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
        OnBedroomFinished?.Invoke();
    }
}