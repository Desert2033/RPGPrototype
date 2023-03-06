using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimator))]
public class Attack : MonoBehaviour
{
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private float _attackCooldown = 3f;

    public float Cleavage = 0.5f;
    public float EffectiveDistance = 0.5f;
    public float Damage = 10f;

    private Transform _heroTransform;
    private float _attackCooldownIterator;

    private bool _isAttacking;
    private int _layerMask;
    private Collider[] _hits = new Collider[1];
    private bool _attackIsActive;

    public void Construct(Transform heroTransform) =>
        _heroTransform = heroTransform;


    private void Awake()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        UpdateCooldown();

        if (CanAttack())
            StartAttack();
    }

    private void OnAttack()
    {
        if (Hit(out Collider hit))
        {
            PhysicDebug.DrawDebug(StartPoint(), Cleavage, 1f);
            hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
        }
    }

    private void OnAttackEnded()
    {
        _attackCooldownIterator = _attackCooldown;

        _isAttacking = false;
    }

    private bool Hit(out Collider hit)
    {
        int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

        hit = _hits.FirstOrDefault();

        return hitsCount > 0;
    }

    private void UpdateCooldown()
    {
        if (!CooldownIsUp())
            _attackCooldownIterator -= Time.deltaTime;
    }

    private void StartAttack()
    {
        transform.LookAt(_heroTransform);
        _animator.PlayAttack();

        _isAttacking = true;
    }

    private Vector3 StartPoint() =>
        new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                    transform.forward * EffectiveDistance;
   
    private bool CanAttack() => 
        _attackIsActive && CooldownIsUp() && !_isAttacking;

    private bool CooldownIsUp() =>
        _attackCooldownIterator <= 0;

    public void DisableAttack() => 
        _attackIsActive = false;

    public void EnableAttack() => 
        _attackIsActive = true;
}
