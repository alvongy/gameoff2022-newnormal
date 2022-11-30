using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "CharaSword_Roll", menuName = "State Machines/Actions/Chara Sword_Roll")]
public class CharaSword_RollSO : StateActionSO
{
	protected override StateAction CreateAction() => new CharaSword_Roll();
	public float duration = 0.3f;
	public float speed = 50;
}

public class CharaSword_Roll : StateAction
{
	protected new CharaSword_RollSO OriginSO => (CharaSword_RollSO)base.OriginSO;
	private EntityController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<EntityController>();
	}

	float timer;

	public override void OnStateEnter()
	{
		controller.animator.SetBool("Roll", true);
		CharacterManager.Instance.OnRoll();
		timer = OriginSO.duration;
	}

	public override void OnUpdate()
	{
		controller.movementVector.x = controller.movementInput.x * OriginSO.speed;
		controller.movementVector.z = controller.movementInput.z * OriginSO.speed;
        if (timer < 0)
        {
			controller.rollTrigger = false;
		}
        else
        {
			timer -= Time.deltaTime;
		}
	}

	public override void OnStateExit()
	{
		controller.animator.SetBool("Roll", false);
	}
}
