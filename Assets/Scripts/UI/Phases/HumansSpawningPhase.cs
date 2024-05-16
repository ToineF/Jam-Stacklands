using UnityEngine;

public class HumansSpawningPhase : MonoBehaviour
{
    private void Update()
    {
        Debug.Log("d,skfl,df,dsfl,dsfkds,lsd");
        // todo: spawn humans
        GameUI.Instance.MoonPhaseProgress.GameState = MoonPhaseProgress.State.DEMONS_LEAVE;
    }

    public void OnStateChanged(MoonPhaseProgress.State state, int night)
    {
        gameObject.SetActive(state == MoonPhaseProgress.State.COMBAT_END);
    }
}