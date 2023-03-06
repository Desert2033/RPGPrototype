using System.Collections;
using UnityEngine;

public class Aggro : MonoBehaviour
{
    [SerializeField] private TriggerObserver _triggerObserver;
    [SerializeField] private Follow _follow;
    [SerializeField] private float _cooldown;

    private Coroutine _aggroCoroutine;
    private bool _hasAggroTarget;

    private void Start()
    {
        SwitchFollowOff();
    }

    private void OnEnable()
    {
        _triggerObserver.TriggerEnter += TriggerEnter;
        _triggerObserver.TriggerExit += TriggerExit;
    }

    private void OnDisable()
    {
        _triggerObserver.TriggerEnter -= TriggerEnter;
        _triggerObserver.TriggerExit -= TriggerExit;
    }

    private void TriggerEnter(Collider obj)
    {
        if (!_hasAggroTarget)
        {
            _hasAggroTarget = true;

            StopAggroCoroutine();

            SwitchFollowOn();
        }
    }

    private void TriggerExit(Collider obj)
    {
        if (_hasAggroTarget)
        {
            _hasAggroTarget = false;

            _aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
        }
    }

    private void StopAggroCoroutine()
    {
        if (_aggroCoroutine != null)
        {
            StopCoroutine(_aggroCoroutine);
            _aggroCoroutine = null;
        }
    }

    private IEnumerator SwitchFollowOffAfterCooldown()
    {
        yield return new WaitForSeconds(_cooldown);

        SwitchFollowOff();
    }

    private void SwitchFollowOn() => 
        _follow.enabled = true;

    private void SwitchFollowOff() => 
        _follow.enabled = false;
}