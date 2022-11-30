using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Tauren_Charged", menuName = "State Machines/Actions/Tauren_Charged")]
public class Tauren_ChargedSO : StateActionSO
{
	protected override StateAction CreateAction() => new Tauren_Charged();
	public float duration = 1f;
}

public class Tauren_Charged : StateAction
{
	protected new Tauren_ChargedSO OriginSO => (Tauren_ChargedSO)base.OriginSO;
	private TaurenController controller;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		controller = stateMachine.GetComponent<TaurenController>();
	}
	
	public override void OnStateEnter()
	{
		controller.warningAreaParNode.transform.right = controller.direction;
		controller.warningArea.enabled = true;
		controller.readySign.enabled = true;
		timer = OriginSO.duration;
	}

	float timer;

	public override void OnUpdate()
	{
        if (timer >= 0)
        {
			timer -= Time.deltaTime;
            if (timer < 0)
            {
				controller.attackTrigger = true;
			}
		}
	}
	
	public override void OnStateExit()
	{
		controller.warningArea.enabled = false;
		controller.readySign.enabled = false;
	}
}
