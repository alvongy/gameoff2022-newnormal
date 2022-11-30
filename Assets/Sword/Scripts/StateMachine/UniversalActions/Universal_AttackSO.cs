using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Universal_Attack", menuName = "State Machines/Actions/Universal_Attack")]
public class Universal_AttackSO : StateActionSO
{
	protected override StateAction CreateAction() => new Universal_Attack();
	public float delay = 1.5f;
}

public class Universal_Attack : StateAction
{
	protected new Universal_AttackSO OriginSO => (Universal_AttackSO)base.OriginSO;
	private EntityController controller;
	private Animator animater;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<EntityController>();
		animater = stateMachine.GetComponent<Animator>();
	}

	float timer;

	public override void OnStateEnter()
	{
		controller.OnAttackEnter();
		timer = OriginSO.delay;
	}
	
	public override void OnUpdate()
	{
        if (timer >= 0)
        {
			timer -= Time.deltaTime;
			if (timer < 0)
            {
				controller.Attack();
				animater.SetTrigger("Attack");
				controller.attackTrigger = false;
			}
		}
	}
	
	public override void OnStateExit()
	{
		controller.OnAttackExit();
	}
}
