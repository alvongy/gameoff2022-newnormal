using StateMachine;
using StateMachine.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAShoot_Condition", menuName = "State Machines/Conditions/BossAShoot_Condition")]
public class BossAShoot_ConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new BossAShoot_Condition();
}

public class BossAShoot_Condition : Condition
{
	protected new BossAShoot_ConditionSO OriginSO => (BossAShoot_ConditionSO)base.OriginSO;
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
		return controller.shooting;
	}

	public override void OnStateExit()
	{
	}
}

