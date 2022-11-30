using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AttackingCondition", menuName = "State Machines/Conditions/Attacking Condition")]
public class AttackingConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new AttackingCondition();
}

public class AttackingCondition : Condition
{
	protected new AttackingConditionSO OriginSO => (AttackingConditionSO)base.OriginSO;
	private EntityController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<EntityController>();
	}
	
	protected override bool Statement()
	{
		return controller.attackTrigger;
	}
}
