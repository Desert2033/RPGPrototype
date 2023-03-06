using Root.Services.PersistentProgress;
using Source.Data;

internal class LoadProgressState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadService;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
    {
        _stateMachine = gameStateMachine;

        _progressService = progressService;

        _saveLoadService = saveLoadService;
    }

    private void LoadProgressOrInitNew() => 
        _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

    private PlayerProgress NewProgress()
    {
        PlayerProgress progress = new PlayerProgress(intialLevel: "Main");

        progress.HeroState.MaxHP = 50;
        progress.HeroStats.Damage = 1f;
        progress.HeroStats.DamageRadius = 0.5f;
        progress.HeroState.ResetHP();

        return progress;
    }

    public void Enter()
    {
        LoadProgressOrInitNew();

        _stateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit()
    {
    }
}
