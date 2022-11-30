using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "EmptyAction", menuName = "State Machines/Actions/EmptyAction")]
public class EmptyActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new EmptyAction();
}

public class EmptyAction : StateAction
{
	protected new EmptyActionSO OriginSO => (EmptyActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
	}
	
	public override void OnStateEnter()
	{
	}
	
	public override void OnUpdate()
	{
	}
	
	public override void OnStateExit()
	{
	}
}
