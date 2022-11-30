using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "MoveCondition", menuName = "State Machines/Conditions/Move Condition")]
public class MoveConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new MoveCondition();
}

public class MoveCondition : Condition
{
	protected new MoveConditionSO OriginSO => (MoveConditionSO)base.OriginSO;
	private EntityController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<EntityController>();
	}
	
	protected override bool Statement()
	{
		return controller.InputVector != Vector2.zero;
	}
}
