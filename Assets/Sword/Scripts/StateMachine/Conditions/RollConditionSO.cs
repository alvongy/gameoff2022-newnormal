using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RollCondition", menuName = "State Machines/Conditions/Roll Condition")]
public class RollConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new RollCondition();
}

public class RollCondition : Condition
{
	protected new RollConditionSO OriginSO => (RollConditionSO)base.OriginSO;
	private EntityController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<EntityController>();
	}

	protected override bool Statement()
	{
		return controller.rollTrigger;
	}
}
