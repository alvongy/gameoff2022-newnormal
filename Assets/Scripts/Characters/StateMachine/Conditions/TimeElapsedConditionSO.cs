using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Time elapsed")]
public class TimeElapsedConditionSO : StateConditionSO<TimeElapsedCondition>
{
	public float timerLength = .5f;

}
public class TimeElapsedCondition : Condition
{
	float _stopTime;
	private TimeElapsedConditionSO _originSO => (TimeElapsedConditionSO)base.OriginSO; // The SO this Condition spawned from

	public override void OnStateEnter()
	{

		_stopTime = Time.time + _originSO.timerLength;
	}
	
	protected override bool Statement() => Time.time > _stopTime;
}
