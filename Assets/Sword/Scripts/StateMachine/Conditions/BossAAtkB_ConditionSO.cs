using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossAAtkB_Condition", menuName = "State Machines/Conditions/BossAAtkB_Condition")]
public class BossAAtkB_ConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new BossAAtkB_Condition();
}

public class BossAAtkB_Condition : Condition
{
	protected new BossAAtkB_ConditionSO OriginSO => (BossAAtkB_ConditionSO)base.OriginSO;
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
		return controller.sqrDistance > controller.sqrAttackRange && controller.shootSkill;
	}
	
	public override void OnStateExit()
	{
	}
}
