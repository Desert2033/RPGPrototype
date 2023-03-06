using Assets.Source.Services;
using Source.Logic;
using Source.Root.Services;

public class Game
{
    //public static IInputService InputService;

    public GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
    {
        StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, AllServices.Container);
    }
}