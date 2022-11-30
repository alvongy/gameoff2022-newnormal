using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInZone", menuName = "State Machines/Conditions/Character In Zone _Summon")]
public class CharacterInZoneSO_Summon : StateConditionSO
{
	public ZoneType zone;

	protected override Condition CreateCondition() => new CharacterInZone_Summon();
}
public class CharacterInZone_Summon : Condition
{
	private Summon _Summon;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_Summon = stateMachine.GetComponent<Summon>();
	}
	protected override bool Statement()
	{
		if (_Summon != null)
		{
			switch ((OriginSO as CharacterInZoneSO).zone)
			{
				case ZoneType.ALERT:
					return _Summon.IsPlayerInAlertZone;
				case ZoneType.ATTACK:
					return _Summon.IsPlayerInAttackZone;
				default:
					break;
			}
		}
		return false;
	}
}
