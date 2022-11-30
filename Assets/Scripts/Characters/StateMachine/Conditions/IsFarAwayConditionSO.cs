using StateMachine;
using StateMachine.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "State Machines/Conditions/IsFarAway")]
public class IsFarAwayConditionSO : StateConditionSO<IsFarAwayCondition>
{
    [SerializeField] public TransformAnchor playerTransformAnchor;
    [SerializeField] public float farAwayDistance=25;
}
public class IsFarAwayCondition : Condition
{
    float distance;
    private Transform _tsf;
    private IsFarAwayConditionSO _originSO => (IsFarAwayConditionSO)base.OriginSO; // The SO this Condition spawned from

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        _tsf = stateMachine.transform;
    }
    public override void OnStateEnter()
    {
    }

    protected override bool Statement() {
        if (!_originSO.playerTransformAnchor.Value)
        {
            return distance > _originSO.farAwayDistance; ;
        }
        distance = (_originSO.playerTransformAnchor.Value.position - _tsf.position).sqrMagnitude;      
        return distance > _originSO.farAwayDistance;
    }
}
