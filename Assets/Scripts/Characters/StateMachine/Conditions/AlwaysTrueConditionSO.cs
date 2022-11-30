using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "AlwaysTrueConditionSO", menuName = "State Machines/Conditions/AlwaysTrueConditionSO")]
public class AlwaysTrueConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new AlwaysTrueCondition();
}

internal class AlwaysTrueCondition : Condition
{
	protected override bool Statement() => true;
}