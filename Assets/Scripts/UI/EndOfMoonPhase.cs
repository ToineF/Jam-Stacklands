using System;
using UnityEngine;

public class EndOfMoonPhase : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FinishEndOfMoon();
        }
    }

    public void EndOfMoon()
    {
        GameUI.Instance.MoonPhaseProgress.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void FinishEndOfMoon()
    {
        GameManager.Instance.IsFastForwarding = false;
        
        GameUI.Instance.MoonPhaseProgress.gameObject.SetActive(true);
        GameUI.Instance.MoonPhaseProgress.NextNight();
        GameUI.Instance.MoonPhaseProgress.IsMoonPhaseOver = false;
        gameObject.SetActive(false);
    }
}
