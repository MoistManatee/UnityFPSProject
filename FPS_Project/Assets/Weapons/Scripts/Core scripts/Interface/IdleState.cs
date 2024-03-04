using UnityEngine;

public class IdleState : IGunStates
{
    private WeaponSway swayScript;

    public IdleState(WeaponSway swayScript)
    {
        this.swayScript = swayScript;
    }

    public void Enter()
    {
        // Debug.Log("Gun is in idle state.");
    }

    public void Update()
    {
        swayScript.applySway();
    }

    public void Exit()
    {
    }
}