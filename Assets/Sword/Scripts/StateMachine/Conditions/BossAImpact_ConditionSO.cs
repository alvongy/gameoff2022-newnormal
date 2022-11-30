using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossAImpact_Condition", menuName = "State Machines/Conditions/Boss AImpact_Condition")]
public class BossAImpact_ConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new BossAImpact_Condition();
}

public class BossAImpact_Condition : Condition
{
	protected new BossAImpact_ConditionSO OriginSO => (BossAImpact_ConditionSO)base.OriginSO;
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
		return controller.impacting;
	}
	
	public override void OnStateExit()
	{
	}
}
