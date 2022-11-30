using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Tauren_Attack", menuName = "State Machines/Actions/Tauren_Attack")]
public class Tauren_AttackSO : StateActionSO
{
	protected override StateAction CreateAction() => new Tauren_Attack();
	public float speed = 20;
	public float duration = 1.5f;
}

public class Tauren_Attack : StateAction
{
	protected new Tauren_AttackSO OriginSO => (Tauren_AttackSO)base.OriginSO;
	private TaurenController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<TaurenController>();
	}
	
	public override void OnStateEnter()
	{
		controller.chargedTrigger = false;
		controller.collider.isTrigger = true;
		controller.canhit = true;
		timer = OriginSO.duration;
	}

	float timer;

	public override void OnUpdate()
	{
        if (timer >= 0)
        {
			controller.transform.Translate(controller.direction * OriginSO.speed * Time.deltaTime);
			timer -= Time.deltaTime;
            if (timer < 0)
            {
				controller.attackTrigger = false;
			}
		}
	}
	
	public override void OnStateExit()
	{
		controller.collider.isTrigger = false;
		controller.canhit = false;
	}
}
