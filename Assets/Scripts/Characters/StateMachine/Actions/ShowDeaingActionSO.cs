using StateMachine;
using StateMachine.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShowDeaingActionSO", menuName = "State Machines/Actions/ShowDeaingActionSO")]
public class ShowDeaingActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new ShowDeaingAction();
}

internal class ShowDeaingAction : StateAction
{
	private Damageable _damageable;
	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_damageable = stateMachine.GetComponent<Damageable>();
	}
	public override void OnStateEnter()
	{
		_damageable.PlayDeaingEffect();
	}
	public override void OnUpdate()
	{

	}
}