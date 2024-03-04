using UnityEngine;

public class ReloadingState : IGunStates
{
    private WeaponSway swayScript;

    public ReloadingState(WeaponSway swayScript)
    {
        this.swayScript = swayScript;
    }

    public void Enter()
    {
        Debug.Log("Gun is in reloading state.");
    }

    public void Update()
    {
        swayScript.applySway();
    }

    public void Exit()
    {
    }
}