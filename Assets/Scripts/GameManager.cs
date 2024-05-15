using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private float fastForwardTimeScale;
    
    private bool _isFastForwarding;
    public bool IsFastForwarding
    {
        get => _isFastForwarding;
        set
        {
            Time.timeScale = value ? fastForwardTimeScale : 1.0f;
            _isFastForwarding = value;
        }
    }

    private bool _isPaused;
    public bool IsPaused
    {
        get => _isPaused;
        set
        {
            _isPaused = value;
            OnPauseStateChanged.Invoke(value);
        }
    }

    public Action<bool> OnPauseStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsPaused = !IsPaused;
        }
    }
}
