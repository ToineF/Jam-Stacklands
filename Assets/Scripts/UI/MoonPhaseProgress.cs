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

    private bool _isMoonPhaseOver;

    public Action MoonPhaseOverEvent;

    void Start()
    {
        Restart();
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
}
