using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossA_Impact", menuName = "State Machines/Actions/Boss A_Impact")]
public class BossA_ImpactSO : StateActionSO
{
	protected override StateAction CreateAction() => new BossA_Impact();
	public float speed = 100;
	public float duration = 0.3f;
}

public class BossA_Impact : StateAction
{
	protected new BossA_ImpactSO OriginSO => (BossA_ImpactSO)base.OriginSO;
	private BossAController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<BossAController>();
	}
	
	public override void OnStateEnter()
	{
		controller.collider.isTrigger = true;
		controller.canhit = true;
		timer = OriginSO.duration;
		forward = true;
	}

	bool forward;
	float timer;

	public override void OnUpdate()
	{
		if (timer >= 0)
		{
            if (forward)
            {
				controller.transform.Translate(controller.warningAreaParNode_Impact.transform.right * (OriginSO.speed * Time.deltaTime));
			}
            else
            {
				controller.transform.Translate(-controller.warningAreaParNode_Impact.transform.right * (OriginSO.speed * Time.deltaTime));
			}
			timer -= Time.deltaTime;
			if (timer < 0)
			{
                if (forward)
                {
					timer = OriginSO.duration;
					forward = false;
					controller.canhit = true;
				}
                else
                {
					controller.impacting = false;
				}
			}
		}
	}
	
	public override void OnStateExit()
	{
		controller.collider.isTrigger = false;
		controller.canhit = false;
	}
}
