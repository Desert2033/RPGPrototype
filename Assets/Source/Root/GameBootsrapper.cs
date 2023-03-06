using UnityEngine;
using Source.Logic;

public class GameBootsrapper : MonoBehaviour, ICoroutineRunner
{
    [SerializeField] private LoadingCurtain _curtainPrefab;
    
    private Game _game;

    private void Awake()
    {
        _game = new Game(this, Instantiate(_curtainPrefab));
        _game.StateMachine.Enter<BootstrapState>();

        DontDestroyOnLoad(this);
    }
}
