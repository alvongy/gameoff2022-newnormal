using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Universal_MoveDone", menuName = "State Machines/Actions/Universal_Move Done")]
public class Universal_MoveDoneSO : StateActionSO
{
	protected override StateAction CreateAction() => new Universal_MoveDone();
}

public class Universal_MoveDone : StateAction
{
	protected new Universal_MoveDoneSO OriginSO => (Universal_MoveDoneSO)base.OriginSO;
	private EntityController controller;
	private CharacterController characterController;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<CharacterController_Sword>();
		characterController = stateMachine.GetComponent<CharacterController>();
	}

	public override void OnStateEnter()
	{
	}
	
	public override void OnUpdate()
	{
		characterController.Move(controller.movementVector * Time.deltaTime);
	}
	
	public override void OnStateExit()
	{
	}
}
