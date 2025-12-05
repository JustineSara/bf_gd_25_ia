using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Core.Character;
using DG.Tweening;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "JuJump", story: "[Agent] jumps to [Target]", category: "Action", id: "cdf1b84e5c008e8bb8e10b15b3976a0b")]
public partial class JuJumpAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> horizontalForce = new BlackboardVariable<float>(1f);
    [SerializeReference] public BlackboardVariable<float> jumpForce = new BlackboardVariable<float>(20f);
    [SerializeReference] public BlackboardVariable<float> buildupTime = new BlackboardVariable<float>(0.5f);
    [SerializeReference] public BlackboardVariable<float> jumpTime = new BlackboardVariable<float>(1.3f);
    [SerializeReference] public BlackboardVariable<string> animationTriggerName = new BlackboardVariable<string>("Jump");

    [SerializeReference] public BlackboardVariable<bool> shakeCameraOnLanding = new BlackboardVariable<bool>(true);

    private bool hasLanded;

    private Tween buildupTween;
    private Tween jumpTween;

    private Animator animator;
    private Rigidbody2D body;

    protected override Status OnStart()
    {
        animator = Agent.Value.GetComponentInChildren<Animator>();
        body = Agent.Value.GetComponent<Rigidbody2D>();
        buildupTween = DOVirtual.DelayedCall(buildupTime.Value, StartJump, false);
        animator.SetTrigger(animationTriggerName.Value);
        return Status.Running;
    }

    private void StartJump()
    {
        var direction = Target.Value.transform.position.x - Agent.Value.transform.position.x;
        body.AddForce(new Vector2(horizontalForce.Value * direction, jumpForce.Value), ForceMode2D.Impulse);

        jumpTween = DOVirtual.DelayedCall(jumpTime.Value, () =>
        {
            hasLanded = true;
            if (shakeCameraOnLanding.Value)
                CameraController.Instance.ShakeCamera(0.5f);
        }, false);
    }

    protected override Status OnUpdate()
    {
        return hasLanded ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
        buildupTween?.Kill();
        jumpTween?.Kill();
        hasLanded = false;
    }
}

