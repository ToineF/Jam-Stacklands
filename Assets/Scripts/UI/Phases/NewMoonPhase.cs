using System;
using UnityEngine;

public class NewMoonPhase : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewMoon();
        }
    }

    public void NewMoon()
    {
        GameUI.Instance.MoonPhaseProgress.GameState = MoonPhaseProgress.State.NIGHT_START;
        GameUI.Instance.MoonPhaseProgress.NextNight();
    }

    public void OnStateChanged(MoonPhaseProgress.State state, int night)
    {
        gameObject.SetActive(state == MoonPhaseProgress.State.NEW_MOON);
    }
}
