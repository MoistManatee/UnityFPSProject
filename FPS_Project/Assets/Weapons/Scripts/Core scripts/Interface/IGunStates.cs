public interface IGunStates
{
    void Enter()
    {
        // code that runs when we first enter the state
    }

    void Update()
    {
        // per-frame logic, include condition to transition to a new state
    }

    void Exit()
    {
        // code that runs when we exit the state
    }
}