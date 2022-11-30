using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossA_Back", menuName = "State Machines/Actions/BossA_Back")]
public class BossA_BackSO : StateActionSO
{
	protected override StateAction CreateAction() => new BossA_Back();
}

public class BossA_Back : StateAction
{
	protected new BossA_BackSO OriginSO => (BossA_BackSO)base.OriginSO;
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
		controller.destinationSetter.SetDestination(controller.transform.position + (-controller.toCharacter).normalized);
	}

	public override void OnStateExit()
	{
		controller.destinationSetter.CanMove(false);
	}
}
