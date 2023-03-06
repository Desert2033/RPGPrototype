using Root.Services.PersistentProgress;
using Source.Data;
using System;
using UnityEngine;

[RequireComponent(typeof(HeroAnimator))]
public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
{
    [SerializeField] private HeroAnimator _animator;

    private State _state;

    public float Current
    {
        get => _state.CurrentHP;
        set
        {
            if (_state.CurrentHP != value)
            {
                _state.CurrentHP = value;

                HealthChanged?.Invoke();
            }
        }
    }
    public float Max
    {
        get => _state.MaxHP;
        set => _state.MaxHP = value;
    }

    public event Action HealthChanged;

    public void LoadProgress(PlayerProgress progress)
    {
        _state = progress.HeroState;

        HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.HeroState.CurrentHP = Current;
        progress.HeroState.MaxHP = Max;
    }

    public void TakeDamage(float damage)
    {
        if (Current <= 0)
            return;

        Current -= damage;

        _animator.PlayHit();
    }
}
