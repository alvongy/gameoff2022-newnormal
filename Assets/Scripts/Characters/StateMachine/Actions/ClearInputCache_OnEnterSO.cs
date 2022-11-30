using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;


[CreateAssetMenu(fileName = "ClearInputCache_OnEnter", menuName = "State Machines/Actions/Clear Input Cache On Enter")]
public class ClearInputCache_OnEnterSO : StateActionSO<ClearInputCache_OnEnter>
{
	protected override StateAction CreateAction() => new ClearInputCache_OnEnter();
}

public class ClearInputCache_OnEnter : StateAction
{
	private MainCharacter _protagonist;
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_protagonist = stateMachine.GetComponent<MainCharacter>();
		_interactionManager = stateMachine.GetComponentInChildren<InteractionManager>();
	}

	public override void OnUpdate()
	{
	}

	public override void OnStateEnter()
	{
		_protagonist.sidestepInput = false;

	}
}
