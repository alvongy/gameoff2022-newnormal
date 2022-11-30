using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "StopNavAgent", menuName = "State Machines/Actions/Stop NavMesh Agent")]
public class StopNavAgentSO : StateActionSO
{
	protected override StateAction CreateAction() => new StopNavAgent();

}
public class StopNavAgent : StateAction
{
	private NavMeshAgent _agent;
	private bool _agentDefined;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
		_agentDefined = _agent != null;
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		if (_agentDefined)
		{
			_agent.isStopped = true;
		}
	}
}
