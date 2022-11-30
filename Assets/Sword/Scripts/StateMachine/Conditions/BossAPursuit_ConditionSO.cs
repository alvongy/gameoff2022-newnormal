using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossAPursuit_Condition", menuName = "State Machines/Conditions/BossAPursuit_Condition")]
public class BossAPursuit_ConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new BossAPursuit_Condition();
}

public class BossAPursuit_Condition : Condition
{
	protected new BossAPursuit_ConditionSO OriginSO => (BossAPursuit_ConditionSO)base.OriginSO;
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
		return controller.sqrDistance > controller.sqrPursuitDistance;
	}
	
	public override void OnStateExit()
	{
	}
}
