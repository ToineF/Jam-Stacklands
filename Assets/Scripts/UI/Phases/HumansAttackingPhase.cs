using UnityEngine;

public class HumansAttackingPhase : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.HasHumans)
        {
            return;
        }

        GameUI.Instance.MoonPhaseProgress.GameState = MoonPhaseProgress.State.COMBAT_END;
    }

    public void OnStateChanged(MoonPhaseProgress.State state, int night)
    {
        gameObject.SetActive(state == MoonPhaseProgress.State.COMBAT_START);
    }
}