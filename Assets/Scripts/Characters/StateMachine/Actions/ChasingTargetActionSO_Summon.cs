using StateMachine;
using StateMachine.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChasingTargetAction", menuName = "State Machines/Actions/Chasing Target Action_Summon")]
public class ChasingTargetActionSO_Summon : StateActionSO
{
	[Tooltip("Target transform anchor.")]
	[SerializeField] private TransformAnchor _targetTransform = default;

	[Tooltip("NPC chasing speed")]
	[SerializeField] private float _chasingSpeed = default;

	public Vector3 TargetPosition => _targetTransform.Value.position;
	public Transform EnemyTarget;
	public float ChasingSpeed => _chasingSpeed;

	protected override StateAction CreateAction() => new ChasingTargetAction_Summon();
}

public class ChasingTargetAction_Summon : StateAction
{
	private ChasingTargetActionSO_Summon _config;
	private Enemy _enemy;
	private PlayerAttributesFloatConfigSO _speedConfigSO;
	private NavMeshAgent _agent;
	private bool _isActiveAgent;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_config = (ChasingTargetActionSO_Summon)OriginSO;
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
		_enemy = stateMachine.gameObject.GetComponent<Enemy>();
		_speedConfigSO = _enemy.EnemyMoveSpeedConfig;
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;



	}

	public override void OnUpdate()
	{
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;

		if (_enemy.IsSummon && !_enemy.isFindEnemy) { _enemy.isFindEnemy = AcquireEnemyTarget(); }
		if (_isActiveAgent && _enemy.isFindEnemy)
		{
			_agent.isStopped = false;
			_agent.speed = _speedConfigSO.InitialValue;
			_agent.SetDestination(_config.EnemyTarget.position);

			if (_enemy.CurrentTarget == null) { _enemy.isFindEnemy = false; }//攻击的敌人死亡后重新搜寻敌人（应该在敌人离开攻击范围后也要重新搜寻）
		}
		else
		{
			//_agent.SetDestination();
			OnPatrolState();
		}
	}
	/// <summary>
	/// 检测有没有发现敌人
	/// </summary>
	/// <returns></returns>
	private bool AcquireEnemyTarget()
	{
		Collider[] targets = Physics.OverlapSphere(_enemy.transform.position, 15f, LayerMask.GetMask("Enemy"));
		foreach (var c in targets)
		{
			if (c.transform.tag.Contains("Enemy"))
			{
				_config.EnemyTarget = c.transform;
				_enemy.CurrentTarget = c.transform.GetComponent<Damageable>();
				Debug.Log("召唤物检测到攻击的敌人："+ _config.EnemyTarget);
				return true;
			}
		}
		return false;
	}

	public override void OnStateEnter()
	{
		if (_isActiveAgent)
		{
			_agent.speed = _config.ChasingSpeed;
		}
	}

	private void OnPatrolState()
	{
		if (_enemy.IsSummonAlertTimer <= 0f)
		{
			_enemy.IsSummonAlertTimer = 3f;
			float rx = Random.Range(-3, 3);
			float ry = Random.Range(-3, 3);

			Vector3 PatrolV3 = new Vector3(_config.TargetPosition.x + rx, 0, _config.TargetPosition.z + ry);
			if (_isActiveAgent) _agent.SetDestination(PatrolV3);
			Debug.Log("进入巡逻状态，跟随玩家移动。PatrolV3："+ PatrolV3);
		}
		_enemy.IsSummonAlertTimer -= Time.deltaTime;
	}

}