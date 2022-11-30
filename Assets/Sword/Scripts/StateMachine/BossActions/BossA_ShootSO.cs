using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "BossA_Shoot", menuName = "State Machines/Actions/BossA_Shoot")]
public class BossA_ShootSO : StateActionSO
{
	protected override StateAction CreateAction() => new BossA_Shoot();
	public float delay = 1f;
}

public class BossA_Shoot : StateAction
{
	protected new BossA_ShootSO OriginSO => (BossA_ShootSO)base.OriginSO;
	private BossAController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<BossAController>();
	}

	int amount;
	float timer;

	public override void OnStateEnter()
	{
		amount = Random.Range(3, 6);
		controller.shooting = true;
		controller.shootSkill = false;
		Aim();
		//controller.warningAreaParNode_Shoot.SetActive(true);
	}

	private void Aim()
    {
		amount--;
		controller.warningAreaParNode_Shoot.transform.right = controller.toCharacter.normalized;
		timer = OriginSO.delay;
	}

	public override void OnUpdate()
	{
		if (timer >= 0)
		{
			timer -= Time.deltaTime;
			if (timer < 0)
			{
				controller.Shoot();
                if (amount > 0)
                {
					Aim();
				}
                else 
				{
					controller.shooting = false;
				}
			}
		}
	}
	
	public override void OnStateExit()
	{
		//controller.warningAreaParNode_Shoot.SetActive(false);
	}
}
