using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Gun : MonoBehaviour
{
    public IGunStates CurrentState;

    public IdleState idleState;
    public ShootingState shootingState;
    public ReloadingState reloadingState;

    public event Action<IGunStates> stateChanged;

    public void Initialize(IGunStates state)
    {
        CurrentState = state;
        state.Enter();
        stateChanged?.Invoke(state);
    }

    public void SetState(IGunStates state)
    {
        CurrentState.Exit();
        CurrentState = state;
        state.Enter();
        stateChanged?.Invoke(state);
    }

    public abstract void ShotAttempted(InputAction.CallbackContext ctx);

    public abstract void ShotCanceled(InputAction.CallbackContext ctx);

    public abstract void ReloadAttempted(InputAction.CallbackContext ctx);
}