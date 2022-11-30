using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Universal_OrdInLayer", menuName = "State Machines/Actions/Universal_Ord In Layer")]
public class Universal_OrdInLayerSO : StateActionSO
{
	protected override StateAction CreateAction() => new Universal_OrdInLayer();
}

public class Universal_OrdInLayer : StateAction
{
	protected new Universal_OrdInLayerSO OriginSO => (Universal_OrdInLayerSO)base.OriginSO;
	private BlockLayer obj;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		obj = stateMachine.GetComponent<BlockLayer>();
	}
	
	public override void OnStateEnter()
	{

	}
	
	public override void OnUpdate()
	{
		obj.ResetOrderInLayer();
	}
	
	public override void OnStateExit()
	{

	}
}
