using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/IsHoldingSidestep")]
public class IsHoldingSidestepConditionSO : StateConditionSO<IsHoldingSidestepCondition> { }

public class IsHoldingSidestepCondition : Condition
{
	MainCharacter _mainCharacter;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_mainCharacter = stateMachine.GetComponent<MainCharacter>();
	}

	protected override bool Statement() => _mainCharacter.sidestepInput;
}