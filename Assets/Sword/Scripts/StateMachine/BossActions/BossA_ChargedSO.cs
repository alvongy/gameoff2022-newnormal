using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossA_Charged", menuName = "State Machines/Actions/BossA_Charged")]
public class BossA_ChargedSO : StateActionSO
{
	protected override StateAction CreateAction() => new BossA_Charged();
	public float duration = 0.5f;
}

public class BossA_Charged : StateAction
{
	protected new BossA_ChargedSO OriginSO => (BossA_ChargedSO)base.OriginSO;
	private BossAController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<BossAController>();
	}

	public override void OnStateEnter()
	{
		controller.impactSkill = false;
		controller.warningAreaParNode_Impact.transform.right = controller.toCharacter.normalized;
		controller.warningArea_Impact.enabled = true;
		timer = OriginSO.duration;
		controller.chargedTrigger = true;
	}

	float timer;

	public override void OnUpdate()
	{
		if (timer >= 0)
		{
			timer -= Time.deltaTime;
			if (timer < 0)
			{
				controller.impacting = true;
			}
		}
	}
	
	public override void OnStateExit()
	{
		controller.warningArea_Impact.enabled = false;
		controller.chargedTrigger = false;
	}
}
