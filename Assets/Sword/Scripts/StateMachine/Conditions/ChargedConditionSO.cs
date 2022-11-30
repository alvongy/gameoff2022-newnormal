using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ChargedCondition", menuName = "State Machines/Conditions/Charged Condition")]
public class ChargedConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new ChargedCondition();
}

public class ChargedCondition : Condition
{
	protected new ChargedConditionSO OriginSO => (ChargedConditionSO)base.OriginSO;
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
		return controller.chargedTrigger;
	}
	
	public override void OnStateExit()
	{
	}
}
