using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;
/// <summary>
/// An Action to clear a <see cref="Protagonist.movementVector"/> at the <see cref="StateAction.SpecificMoment"/> <see cref="StopMovementActionSO.Moment"/>
/// </summary>
[CreateAssetMenu(fileName = "StopMovementAction", menuName = "State Machines/Actions/Stop Movement")]
public class StopMovementActionSO : StateActionSO
{
	[SerializeField] StateAction.SpecificMoment _moment = default;
	public StateAction.SpecificMoment Moment => _moment;
	protected override StateAction CreateAction()
	{
		return new StopMovement();
	}
}

public class StopMovement : StateAction
{
	private MainCharacter _character;
	private new StopMovementActionSO OriginSO => (StopMovementActionSO)base.OriginSO;
	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_character = stateMachine.GetComponent<MainCharacter>();
	}

	public override void OnUpdate()
	{
		if (OriginSO.Moment == SpecificMoment.OnUpdate)
		{
			_character.movementVector = Vector3.zero;
		}
	}

	public override void OnStateEnter()
	{
		if (OriginSO.Moment == SpecificMoment.OnStateEnter)
		{
			_character.movementVector = Vector3.zero;
		}
	}

	public override void OnStateExit()
	{
		if (OriginSO.Moment == SpecificMoment.OnStateExit)
		{
			_character.movementVector = Vector3.zero;
		}
	}

}
