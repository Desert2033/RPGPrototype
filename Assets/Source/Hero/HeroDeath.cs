using UnityEngine;

[RequireComponent(typeof(HeroHealth))]
public class HeroDeath : MonoBehaviour
{
    [SerializeField] private HeroHealth _health;
    [SerializeField] private HeroMove _move;
    [SerializeField] private HeroAnimator _animator;
    [SerializeField] private HeroAttack _attack;
    [SerializeField] private GameObject _deathFx;

    private bool _isDead;

    private void Start() =>
        _health.HealthChanged += HealthChanged;

    private void OnDestroy() =>
        _health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
        if (!_isDead && _health.Current <= 0)
            Die();
    }

    private void Die()
    {
        _move.enabled = false;
        _attack.enabled = false;
        _animator.PlayDeath();

        Instantiate(_deathFx, transform.position, Quaternion.identity);

        _isDead = true;
    }
}
