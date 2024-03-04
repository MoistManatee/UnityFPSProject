using UnityEngine;

public class ShootingState : IGunStates
{
    private WeaponSway swayScript;
    private ParticleSystem gunFlash;

    public ShootingState(WeaponSway swayScript, ParticleSystem gunFlash)
    {
        this.swayScript = swayScript;
        this.gunFlash = gunFlash;
    }

    public void Enter()
    {
        // Debug.Log("Gun is in shooting state.");
        var emission = gunFlash.emission;
        emission.enabled = true;
    }

    public void Update()
    {
        swayScript.applySway();
    }

    public void Exit()
    {
        var emission = gunFlash.emission;
        emission.enabled = false;
    }
}