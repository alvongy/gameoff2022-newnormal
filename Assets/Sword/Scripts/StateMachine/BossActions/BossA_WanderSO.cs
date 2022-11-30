using StateMachine;
using StateMachine.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossA_Wander", menuName = "State Machines/Actions/BossA_Wander")]
public class BossA_WanderSO : StateActionSO
{
	protected override StateAction CreateAction() => new BossA_Wander();
}

public class BossA_Wander : StateAction
{
	protected new BossA_WanderSO OriginSO => (BossA_WanderSO)base.OriginSO;
	private BossAController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<BossAController>();
	}

	Vector3 direction;

	public override void OnStateEnter()
	{
		Vector3 p = Vector3.right;
		int angel = Random.Range(0, 360);
		Quaternion q;
		q = Quaternion.Euler(Vector3.up * angel);
		p = q * p;
		direction = p.normalized;
		controller.destinationSetter.CanMove(true);
		controller.destinationSetter.speed = controller.Data.MoveSpeed;
	}

	public override void OnUpdate()
	{
		controller.destinationSetter.SetDestination(controller.transform.position + direction);
	}

	public override void OnStateExit()
	{
		controller.destinationSetter.CanMove(false);
	}
}
