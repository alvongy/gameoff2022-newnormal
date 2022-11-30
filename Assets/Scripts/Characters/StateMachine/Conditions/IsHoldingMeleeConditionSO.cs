using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/IsHoldingMeleeAction")]
public class IsHoldingMeleeConditionSO : StateConditionSO<IsHoldingMeleeCondition> { }

public class IsHoldingMeleeCondition : Condition
{
	private WeaponManager _weaponManager;
	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_weaponManager = stateMachine.GetComponent<WeaponManager>();
	}
	protected override bool Statement()
	{
		return _weaponManager.inMeleeingMain;
	}
}