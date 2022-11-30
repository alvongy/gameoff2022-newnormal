using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

public enum ZoneType
{
	ALERT,
	ATTACK,
}

[CreateAssetMenu(fileName = "CharacterInZone", menuName = "State Machines/Conditions/Character In Zone")]
public class CharacterInZoneSO : StateConditionSO
{
	public ZoneType zone;

	protected override Condition CreateCondition() => new CharacterInZone();
}
public class CharacterInZone : Condition
{
	private Enemy _Enemy;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_Enemy = stateMachine.GetComponent<Enemy>();
	}
	protected override bool Statement()
	{
		if (_Enemy != null)
		{
			switch ((OriginSO as CharacterInZoneSO).zone)
			{
				case ZoneType.ALERT:
					return _Enemy.IsPlayerInAlertZone;
				case ZoneType.ATTACK:
					return _Enemy.IsPlayerInAttackZone;
				default:
					break;
			}
		}
		return false;
	}
}
