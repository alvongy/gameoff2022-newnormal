using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Started Moving")]
public class IsMovingConditionSO : StateConditionSO<IsMovingCondition>
{
	public float treshold = 0.02f;
}
public class IsMovingCondition : Condition
{
	MainCharacter _character;
	IsMovingConditionSO _originSO => (IsMovingConditionSO)base.OriginSO;
	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_character = stateMachine.GetComponent<MainCharacter>();
	}

	protected override bool Statement()
	{
		return _character.movementInput.sqrMagnitude > _originSO.treshold;
	}
}
