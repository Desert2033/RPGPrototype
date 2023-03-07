using Source.Root.Services;
using UnityEngine;

public class LevelTransferTrigger : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [SerializeField] private string TransferTo;
    
    private IGameStateMachine _stateMachine;
    private bool _triggered;

    private void Awake()
    {
        _stateMachine = AllServices.Container.Single<IGameStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered)
            return;

        if (other.CompareTag(PlayerTag))
        {
            _stateMachine.Enter<LoadLevelState, string>(TransferTo);
            _triggered = true;
        }
    }
}
