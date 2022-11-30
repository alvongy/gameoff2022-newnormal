using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossAWander_Condition", menuName = "State Machines/Conditions/BossAWander_Condition")]
public class BossAWander_ConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new BossAWander_Condition();
}

public class BossAWander_Condition : Condition
{
	protected new BossAWander_ConditionSO OriginSO => (BossAWander_ConditionSO)base.OriginSO;
	private BossAController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<BossAController>();
	}
	
	public override void OnStateEnter()
	{
	}
	
	protected override bool Statement()
	{
		return controller.sqrDistance > controller.sqrWanderRange.x && controller.sqrDistance < controller.sqrWanderRange.y;
	}
	
	public override void OnStateExit()
	{
	}
}
