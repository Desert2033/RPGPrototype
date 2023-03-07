using System.Collections.Generic;
using System;
using Source.Logic;
using Source.Root.Services;
using Source.Root;
using Root.Services.PersistentProgress;

public class GameStateMachine : IGameStateMachine
{
    private readonly Dictionary<Type, IExistableState> _states;
    private IExistableState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain, AllServices services)
    {
        _states = new Dictionary<Type, IExistableState>()
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
            [typeof(LoadLevelState)] = new LoadLevelState(
                this,
                sceneLoader,
                curtain,
                services.Single<IGameFactory>(),
                services.Single<IPersistentProgressService>(),
                services.Single<IStaticDataService>(),
                services.Single<IUIFactory>()),
            [typeof(LoadProgressState)] = new LoadProgressState(
                this,
                services.Single<IPersistentProgressService>(),
                services.Single<ISaveLoadService>()),
            [typeof(GameLoopState)] = new GameLoopState(this),
        };
    }

    private TState GetState<TState>() where TState : class, IExistableState =>
        _states[typeof(TState)] as TState;

    private TState ChangeState<TState>() where TState : class, IExistableState
    {
        _activeState?.Exit();
        TState state = GetState<TState>();
        _activeState = state;

        return state;
    }

    public void Enter<TState>() where TState : class, IState
    {
        IState state = ChangeState<TState>();
        state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
        TState state = ChangeState<TState>();
        state.Enter(payload);
    }
}
