using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyMovementVector", menuName = "State Machines/Actions/Apply Movement Vector")]
public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction> { }

public class ApplyMovementVectorAction : StateAction
{
	private MainCharacter _character;
	private CharacterController _characterController;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_character = stateMachine.GetComponent<MainCharacter>();
		_characterController = stateMachine.GetComponent<CharacterController>();
	}
	public override void OnUpdate()
	{
		_characterController.Move(_character.movementVector * Time.deltaTime);
		_character.movementVector = _characterController.velocity;
	}
}
