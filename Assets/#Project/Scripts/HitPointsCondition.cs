using Core.Combat;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Hit Points", story: "[Destructable] HP less than [Value]", category: "Conditions/Boss Fight", id: "8929f2b25b2666fbd2ee95caa68cfda0")]
public partial class HitPointsCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Destructable> Destructable;
    [SerializeReference] public BlackboardVariable<int> Value;

    public override bool IsTrue()
    {
        return Destructable.Value.CurrentHealth <= Value.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
