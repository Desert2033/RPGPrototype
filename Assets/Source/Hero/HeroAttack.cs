using Assets.Source.Services;
using Root.Services.PersistentProgress;
using Source.Data;
using Source.Root.Services;
using UnityEngine;

[RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
public class HeroAttack : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private HeroAnimator _animator;
    [SerializeField] private CharacterController _characterController;

    private IInputService _input;
    private static int _layerMask;
    private Collider[] _hits = new Collider[3];
    private Stats _stats;

    private void Awake()
    {
        _input = AllServices.Container.Single<IInputService>();

        _layerMask = 1 << LayerMask.NameToLayer("Hittable");
    }

    private void Update()
    {
        if (_input.IsAttackButtonUp()/* && !_animator.IsAttacking*/)
            _animator.PlayAttack();
    }

    private Vector3 StartPoint() =>
        new Vector3(transform.position.x, _characterController.center.y / 2, transform.position.z);

    private int Hit() =>
        Physics.OverlapSphereNonAlloc(
            StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);

    public void OnAttack()
    {
        for (int i = 0; i < Hit(); i++)
            _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _stats = progress.HeroStats;
    }
}
