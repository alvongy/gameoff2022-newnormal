using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossA_Pursuit", menuName = "State Machines/Actions/BossA_Pursuit")]
public class BossA_PursuitSO : StateActionSO
{
	protected override StateAction CreateAction() => new BossA_Pursuit();
}

public class BossA_Pursuit : StateAction
{
	protected new BossA_PursuitSO OriginSO => (BossA_PursuitSO)base.OriginSO;
	private BossAController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<BossAController>();
	}

	public override void OnStateEnter()
	{
		controller.destinationSetter.CanMove(true);
		controller.destinationSetter.speed = controller.Data.MoveSpeed;
	}

	public override void OnUpdate()
	{
		controller.destinationSetter.SetDestination(controller.transform.position + controller.toCharacter.normalized);
	}

	public override void OnStateExit()
	{
		controller.destinationSetter.CanMove(false);
	}
}
