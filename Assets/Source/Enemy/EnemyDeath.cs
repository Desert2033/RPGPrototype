using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private GameObject _deathFx;
    [SerializeField] private AgentMoveToPlayer _agentMoveToPlayer;
    [SerializeField] private Aggro _aggro;

    public event Action Happened;

    private void Start() => 
        _health.HealthChanged += HealthChanged;

    private void OnDestroy() => 
        _health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
        if (_health.Current <= 0)
            Die();
    }

    private void Die()
    {
        _health.HealthChanged -= HealthChanged;

        _agentMoveToPlayer.enabled = false;
        _aggro.enabled = false;

        _animator.PlayDeath();

        SpawnDeathFx();

        StartCoroutine(DestroyTimer());

        Happened?.Invoke();
    }

    private void SpawnDeathFx() => 
        Instantiate(_deathFx, transform.position, Quaternion.identity);

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
