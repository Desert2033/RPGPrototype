using Source.Root.Services;

public interface IGameStateMachine : IService 
{
    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
    public void Enter<TState>() where TState : class, IState;
}