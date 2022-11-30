using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "CharaSword_OnStateEnter", menuName = "State Machines/Actions/CharaSword_OnStateEnterSO")]
public class CharaSword_OnStateEnterSO : StateActionSO
{
	protected override StateAction CreateAction() => new CharaSword_OnStateEnter();
	public CharacterController_Sword.State state;
}

public class CharaSword_OnStateEnter : StateAction
{
	protected new CharaSword_OnStateEnterSO OriginSO => (CharaSword_OnStateEnterSO)base.OriginSO;
	private CharacterController_Sword controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<CharacterController_Sword>();
	}

	public override void OnStateEnter()
	{
		controller.state = OriginSO.state;
	}
	
	public override void OnUpdate()
	{

	}
	
	public override void OnStateExit()
	{

	}
}
