using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyEntity", menuName = "State Machines/Actions/Destroy Entity")]
public class DestroyEntitySO : StateActionSO
{
	[SerializeField] private bool _usePool = true;

	public bool UsePool { get => _usePool; set => _usePool = value; }

	protected override StateAction CreateAction() => new DestroyEntity();
}

public class DestroyEntity : StateAction
{
	private GameObject _gameObject;
	private DestroyEntitySO destroyEntitySO => (DestroyEntitySO)base.OriginSO;
	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_gameObject = stateMachine.gameObject;
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		if (destroyEntitySO.UsePool)
		{
			YK.Game.Events.ObjectPool.Destroy(_gameObject);
			_gameObject.SetActive(false);
		}
		else
		{
			GameObject.Destroy(_gameObject);
		}
	}
}
