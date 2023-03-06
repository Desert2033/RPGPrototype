using UnityEngine;

public class AnimatorStateReporter : StateMachineBehaviour
{
    private IAnimationStateReader _stateReader;

    private void FindReader(Animator animator)
    {
        if (_stateReader != null)
            return;

        _stateReader = animator.GetComponent<IAnimationStateReader>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        FindReader(animator);

        _stateReader.EnteredState(stateInfo.shortNameHash);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        FindReader(animator);

        _stateReader.ExitedState(stateInfo.shortNameHash);
    }
}