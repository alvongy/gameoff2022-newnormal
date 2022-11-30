using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "IsDeadCondition", menuName = "State Machines/Conditions/Is Dead")]
public class IsDeadConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsDeadCondition();
}

public class IsDeadCondition : Condition
{
	private Damageable _damageableScript;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_damageableScript = stateMachine.GetComponent<Damageable>();
	}

	protected override bool Statement()
	{
		return _damageableScript.IsDead;
	}
}
