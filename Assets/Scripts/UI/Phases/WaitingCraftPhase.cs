using UnityEngine;

public class WaitingCraftPhase : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.CurrentMaxCookTime > 0.001f)
        {
            return;
        }

        GameUI.Instance.MoonPhaseProgress.GameState = MoonPhaseProgress.State.COMBAT_START;
    }

    public void OnStateChanged(MoonPhaseProgress.State state, int night)
    {
        gameObject.SetActive(state == MoonPhaseProgress.State.WAIT_CRAFTS);
    }
}