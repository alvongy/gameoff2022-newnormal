using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayParticlesAction", menuName = "State Machines/Actions/PlayParticlesAction")]
public class PlayParticlesActionSO : StateActionSO<PlayParticlesAction>
{
	public string particleName = default;
}

public class PlayParticlesAction : StateAction
{
	private PlayerEffectController _effController;
	private PlayParticlesActionSO _originSO => (PlayParticlesActionSO)base.OriginSO;
	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_effController = stateMachine.GetComponent<PlayerEffectController>();
	}
	public override void OnStateEnter()
	{
		_effController.EnableParticles(_originSO.particleName);
	}
	public override void OnUpdate()
	{
	}
	public override void OnStateExit()
	{
		base.OnStateExit();
	}
}
