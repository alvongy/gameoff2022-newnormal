using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossAAtkA_Condition", menuName = "State Machines/Conditions/BossAAtkA_Condition")]
public class BossAAtkA_ConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new BossAAtkA_Condition();
}

public class BossAAtkA_Condition : Condition
{
	protected new BossAAtkA_ConditionSO OriginSO => (BossAAtkA_ConditionSO)base.OriginSO;
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
		return controller.sqrDistance < controller.sqrAttackRange && controller.impactSkill;
	}
	
	public override void OnStateExit()
	{
	}
}
