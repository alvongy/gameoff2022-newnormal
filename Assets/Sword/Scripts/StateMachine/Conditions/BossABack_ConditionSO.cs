using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossABack_Condition", menuName = "State Machines/Conditions/BossABack_Condition")]
public class BossABack_ConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new BossABack_Condition();
}

public class BossABack_Condition : Condition
{
	protected new BossABack_ConditionSO OriginSO => (BossABack_ConditionSO)base.OriginSO;
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
		return controller.sqrDistance < controller.sqrMinDistance;
	}
	
	public override void OnStateExit()
	{
	}
}
