using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Enemy_TurnAround", menuName = "State Machines/Actions/Enemy_TurnAround")]
public class Enemy_TurnAroundSO : StateActionSO
{
	protected override StateAction CreateAction() => new Enemy_TurnAround();
}

public class Enemy_TurnAround : StateAction
{
	protected new Enemy_TurnAroundSO OriginSO => (Enemy_TurnAroundSO)base.OriginSO;

	private DestinationSetter destinationSetter;

	private EntityController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		destinationSetter = stateMachine.GetComponent<DestinationSetter>();
		controller = stateMachine.GetComponent<EntityController>();
	}
	
	public override void OnStateEnter()
	{
		
	}
	
	public override void OnUpdate()
	{
		if (destinationSetter.Target.HasValue)
		{
			if (destinationSetter.Target.Value.x - destinationSetter.transform.position.x > 0)
			{
				controller.renderer.flipX = false;
			}
			else
			{
				controller.renderer.flipX = true;
			}
		}
	}
	
	public override void OnStateExit()
	{
		
	}
}
