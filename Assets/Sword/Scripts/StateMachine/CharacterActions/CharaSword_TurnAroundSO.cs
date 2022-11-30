using StateMachine;
using StateMachine.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharaSword_TurnAroundSO", menuName = "State Machines/Actions/CharaSword_TurnAround")]
public class CharaSword_TurnAroundSO : StateActionSO
{
    protected override StateAction CreateAction() => new CharaSword_TurnAround();



}
public class CharaSword_TurnAround : StateAction
{
	protected new CharaSword_TurnAroundSO OriginSO => (CharaSword_TurnAroundSO)base.OriginSO;

	private EntityController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<EntityController>();
     }

	public override void OnUpdate()
    {
		if (controller.movementVector.x > 0)
		{
			controller.renderer.flipX = false;
		}
		else
		{
			controller.renderer.flipX = true;
		}
	}
}
