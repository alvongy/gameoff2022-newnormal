using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Universal_TurnAround", menuName = "State Machines/Actions/Universal_TurnAround")]
public class Universal_TurnAroundSO : StateActionSO
{
	protected override StateAction CreateAction() => new Universal_TurnAround();
}

public class Universal_TurnAround : StateAction
{
	protected new Universal_TurnAroundSO OriginSO => (Universal_TurnAroundSO)base.OriginSO;
	private EntityController controller;

	private Vector3 faceLeft;
	private Vector3 faceRight;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<EntityController>();
		faceLeft = new Vector3(-Mathf.Abs(controller.transform.localScale.x), controller.transform.localScale.y, controller.transform.localScale.z);
		faceRight = new Vector3(Mathf.Abs(controller.transform.localScale.x), controller.transform.localScale.y, controller.transform.localScale.z);
	}
	
	public override void OnStateEnter()
	{

	}
	
	public override void OnUpdate()
	{
        if (controller.movementVector.x > 0)
        {
			controller.transform.localScale = faceRight;
		}
        else
        {
			controller.transform.localScale = faceLeft;
		}
	}
	
	public override void OnStateExit()
	{

	}
}
