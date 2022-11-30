using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "HorizontalMove", menuName = "State Machines/Actions/Horizontal Move")]
public class HorizontalMoveActionSO : StateActionSO<HorizontalMoveAction>
{
	[Tooltip("Horizontal XZ plane speed multiplier")]
	public float speed = 8f;
}
public class HorizontalMoveAction : StateAction
{
	private MainCharacter _character;
	private HorizontalMoveActionSO _originSO => (HorizontalMoveActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_character = stateMachine.GetComponent<MainCharacter>();
	}
	public override void OnUpdate()
	{
		_character.movementVector.x = _character.movementInput.x * _originSO.speed;
		_character.movementVector.z = _character.movementInput.z * _originSO.speed;
		//_character.movementVector.x = _character.movementInput.x ;
		//_character.movementVector.z = _character.movementInput.z ;
	}
}
