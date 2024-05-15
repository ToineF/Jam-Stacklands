using System;
using UnityEngine;
using UnityEngine.UI;

public class MoonPhaseProgress : MonoBehaviour
{
    [SerializeField]
    private Image moonPhaseImage;
    [SerializeField]
    [Tooltip("Time in seconds")]
    private float timePerPhase;

    [SerializeField]
    private Image playPauseImage;
    [SerializeField]
    private Sprite playSprite;
    [SerializeField]
    private Sprite pauseSprite;
    [SerializeField]
    private Sprite fastSprite;

    private bool _isMoonPhaseOver;

    public Action MoonPhaseOverEvent;

    void Start()
    {
        GameManager.Instance.OnPauseStateChanged += OnPause;
        Restart();
    }

    private void OnPause(bool isPaused)
    {
        if (!isPaused && GameManager.Instance.IsFastForwarding)
        {
            playPauseImage.sprite = fastSprite;
        }
        else
        {
            playPauseImage.sprite = isPaused ? pauseSprite : playSprite;
        }
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused)
        {
            return;
        }
        
        if (_isMoonPhaseOver)
        {
            return;
        }
        
        moonPhaseImage.fillAmount += Time.deltaTime / timePerPhase;
        if (moonPhaseImage.fillAmount < 1.0f)
        {
            return;
        }
        
        _isMoonPhaseOver = true;
        MoonPhaseOverEvent?.Invoke();
    }

    void Restart()
    {
        moonPhaseImage.fillAmount = 0.0f;
    }

    public void CycleThroughSpeeds()
    {
        if (GameManager.Instance.IsPaused)
        {
            GameManager.Instance.IsPaused = false;
            GameManager.Instance.IsFastForwarding = false;
            playPauseImage.sprite = playSprite;
        }
        else if (!GameManager.Instance.IsFastForwarding)
        {
            GameManager.Instance.IsFastForwarding = true;
            playPauseImage.sprite = fastSprite;
        }
        else
        {
            GameManager.Instance.IsFastForwarding = false;
            GameManager.Instance.IsPaused = true;
        }
    }
}
