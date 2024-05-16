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

    public Action<State, int> OnStateChanged;

    private State _state;
    public State GameState
    {
        get => _state;
        set
        {
            _state = value;
            OnStateChanged.Invoke(_state, _currentNightCounter);
        }
    }

    void Start()
    {
        _currentNightCounter = 1;
        GameManager.Instance.OnPauseStateChanged += OnPause;
        GameManager.Instance.OnFastForward += OnFastForward;
        OnStateChanged += _OnStateChanged;
        
        Restart();
    }

    private void OnFastForward(bool isFastForwarding)
    {
        playPauseImage.sprite = isFastForwarding ? fastSprite : playSprite;
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
        if (GameState != State.NIGHT_START)
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

        if (GameManager.Instance.CurrentMaxCookTime > 0.0f)
        {
            GameState = State.WAIT_CRAFTS;
        }
        else if (GameManager.Instance.HasEnemies)
        {
            GameState = State.COMBAT_START;
        }
        else
        {
            GameState = State.NEW_MOON;
        }
    }
    
    private void _OnStateChanged(State state, int night)
    {
        gameObject.SetActive(state == State.NIGHT_START);
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

    public void NextNight()
    {
        _currentNightCounter++;
        nightsText.text = "Night " + _currentNightCounter;
        
        Restart();
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

    public enum State
    {
        NIGHT_START,
        WAIT_CRAFTS,
        COMBAT_START,
        COMBAT_END,
        DEMONS_LEAVE,
        NEW_MOON,
    }
}
