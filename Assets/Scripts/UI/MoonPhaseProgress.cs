using System;
using TMPro;
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
    
    [SerializeField]
    private TMP_Text nightsText;
    private int _currentNightCounter;

    [HideInInspector]
    public bool IsMoonPhaseOver;

    public Action MoonPhaseOverEvent;

    void Start()
    {
        _currentNightCounter = 1;
        GameManager.Instance.OnPauseStateChanged += OnPause;
        GameManager.Instance.OnFastForward += OnFastForward;
        
        Restart();
    }

    private void OnFastForward(bool isFastForwarding)
    {
        playPauseImage.sprite = isFastForwarding ? fastSprite : playSprite;
    }

    public void NextNight()
    {
        IsMoonPhaseOver = false;
        _currentNightCounter++;
        nightsText.text = "Night " + _currentNightCounter;
        
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
        if (IsMoonPhaseOver)
        {
            return;
        }
        
        HandleInputs();
        
        if (GameManager.Instance.IsPaused)
        {
            return;
        }
        
        moonPhaseImage.fillAmount += Time.deltaTime / timePerPhase;
        if (moonPhaseImage.fillAmount < 1.0f)
        {
            return;
        }
        
        IsMoonPhaseOver = true;
        MoonPhaseOverEvent?.Invoke();
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.IsPaused = !GameManager.Instance.IsPaused;
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.Instance.IsPaused)
            {
                GameManager.Instance.IsPaused = false;
            }
            else
            {
                GameManager.Instance.IsFastForwarding = !GameManager.Instance.IsFastForwarding;
            }
        }
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
        }
        else if (!GameManager.Instance.IsFastForwarding)
        {
            GameManager.Instance.IsFastForwarding = true;
        }
        else
        {
            GameManager.Instance.IsFastForwarding = false;
            GameManager.Instance.IsPaused = true;
        }
    }
}
