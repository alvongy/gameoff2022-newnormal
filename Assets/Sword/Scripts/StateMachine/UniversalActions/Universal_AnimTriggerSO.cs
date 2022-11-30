using StateMachine;
using StateMachine.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Universal_AnimTrigger", menuName = "State Machines/Actions/Universal_AnimTrigger")]
public class Universal_AnimTriggerSO : StateActionSO
{
	protected override StateAction CreateAction() => new Universal_AnimTrigger();
	public float delay = 0f;
	public string Trigger;
}

public class Universal_AnimTrigger : StateAction
{
	protected new Universal_AnimTriggerSO OriginSO => (Universal_AnimTriggerSO)base.OriginSO;
	private Animator animater;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		animater = stateMachine.GetComponent<Animator>();
	}

	float timer;

	public override void OnStateEnter()
	{
		timer = OriginSO.delay;
	}

	public override void OnUpdate()
	{
		if (timer >= 0)
		{
			timer -= Time.deltaTime;
            if (timer < 0)
            {
				animater.SetTrigger(OriginSO.Trigger);
			}
		}
	}

	public override void OnStateExit()
	{

	}
}
