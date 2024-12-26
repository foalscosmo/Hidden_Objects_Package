using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace com.appidea.MiniGamePlatform.Hidden_Objects.Hidden_Objects.Runtime.Scripts
{
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
}