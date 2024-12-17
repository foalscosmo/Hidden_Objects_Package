using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private DownPanel downPanel;
    [SerializeField] private TouchObject touchObject;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip finishSound;
    private void OnEnable()
    {
        touchObject.OnCorrectTouch += PlayCorrectSound;
        downPanel.OnCurrentLevelFinish += PlayFinishSound;
    }

    private void OnDisable()
    {
        touchObject.OnCorrectTouch -= PlayCorrectSound;
        downPanel.OnCurrentLevelFinish -= PlayFinishSound;
    }


    private void PlayCorrectSound() => audioSource.PlayOneShot(correctSound);
    private void PlayFinishSound() => audioSource.PlayOneShot(finishSound);
}