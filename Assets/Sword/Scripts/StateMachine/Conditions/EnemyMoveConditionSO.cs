using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "EnemyMoveCondition", menuName = "State Machines/Conditions/Enemy Move Condition")]
public class EnemyMoveConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new EnemyMoveCondition();
}

public class EnemyMoveCondition : Condition
{
	protected new EnemyMoveConditionSO OriginSO => (EnemyMoveConditionSO)base.OriginSO;
	private EnemyController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<EnemyController>();
	}
	
	public override void OnStateEnter()
	{
	}
	
	protected override bool Statement()
	{
		return controller.moveTrigger;
	}
	
	public override void OnStateExit()
	{
	}
}
