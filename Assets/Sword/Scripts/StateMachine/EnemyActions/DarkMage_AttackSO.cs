using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "DarkMage_Attack", menuName = "State Machines/Actions/Dark Mage_Attack")]
public class DarkMage_AttackSO : StateActionSO
{
	protected override StateAction CreateAction() => new DarkMage_Attack();
	public float delay = 1.5f;
}

public class DarkMage_Attack : StateAction
{
	protected new DarkMage_AttackSO OriginSO => (DarkMage_AttackSO)base.OriginSO;
	private DarkMageController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<DarkMageController>();
	}

	float timer;

	public override void OnStateEnter()
	{
		timer = OriginSO.delay;
		controller.OnAttackEnter();
	}

	public override void OnUpdate()
	{
		if (timer >= 0)
		{
			timer -= Time.deltaTime;
			if (timer < 0)
			{
				controller.Attack();
				controller.attackTrigger = false;
			}
		}
	}
	
	public override void OnStateExit()
	{
		controller.OnAttackExit();
	}
}
