public interface IState : IExistableState
{
    public void Enter();
}

public interface IPayloadedState<TPayload> : IExistableState
{
    public void Enter(TPayload payload);
}

public interface IExistableState
{
    public void Exit();
}
