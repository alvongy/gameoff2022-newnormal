using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Enemy_MoveState", menuName = "State Machines/Actions/Enemy_MoveState")]
public class Enemy_MoveStateSO : StateActionSO
{
	protected override StateAction CreateAction() => new Enemy_MoveState();
}

public class Enemy_MoveState : StateAction
{
	protected new Enemy_MoveStateSO OriginSO => (Enemy_MoveStateSO)base.OriginSO;

	private DestinationSetter destinationSetter;


	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		destinationSetter = stateMachine.GetComponent<DestinationSetter>();
	}

	public override void OnStateEnter()
	{
		destinationSetter.CanMove(true);
	}

	public override void OnUpdate()
	{
		
	}

	public override void OnStateExit()
	{
		destinationSetter.CanMove(false);
	}
}
