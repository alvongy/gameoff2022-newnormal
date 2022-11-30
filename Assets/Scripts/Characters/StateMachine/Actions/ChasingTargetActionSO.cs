using StateMachine;
using StateMachine.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChasingTargetAction", menuName = "State Machines/Actions/Chasing Target Action")]
public class ChasingTargetActionSO : StateActionSO
{
	[Tooltip("Target transform anchor.")]
	[SerializeField] private TransformAnchor _targetTransform = default;

	[Tooltip("NPC chasing speed")]
	[SerializeField] private float _chasingSpeed = default;

	public Vector3 TargetPosition => _targetTransform.Value.position;
	public Transform EnemyTarget;
	public float ChasingSpeed => _chasingSpeed;

	protected override StateAction CreateAction() => new ChasingTargetAction();
}

public class ChasingTargetAction : StateAction
{
	private ChasingTargetActionSO _config;
	private Enemy _enemy;
	private PlayerAttributesFloatConfigSO _speedConfigSO;
	private NavMeshAgent _agent;
	private bool _isActiveAgent;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_config = (ChasingTargetActionSO)OriginSO;
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
		_enemy = stateMachine.gameObject.GetComponent<Enemy>();
		_speedConfigSO = _enemy.EnemyMoveSpeedConfig;
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;



	}

	public override void OnUpdate()
	{
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;

        if (_enemy.IsSummon)
        {
			if (!_enemy.isFindEnemy){ _enemy.isFindEnemy = AcquireEnemyTarget(); }
			if (_isActiveAgent && _enemy.isFindEnemy)
			{
				_agent.isStopped = false;
				_agent.speed = _speedConfigSO.InitialValue;
				_agent.SetDestination(_config.EnemyTarget.position);

				//if (_enemy.CurrentTarget == null) { _enemy.isFindEnemy = false; }//攻击的敌人死亡后重新搜寻敌人（应该在敌人离开攻击范围后也要重新搜寻）
				if (_enemy.CurrentTarget == null || _enemy.CurrentTarget.IsDead) { _enemy.isFindEnemy = false; }//攻击的敌人死亡后重新搜寻敌人（应该在敌人离开攻击范围后也要重新搜寻）
			}
			else
			{
				//_agent.SetDestination();
				OnPatrolState();
			}
        }
        else
        {
			if (_isActiveAgent && _enemy.isPlayerOn)
			{
				_agent.isStopped = false;
				_agent.speed = _speedConfigSO.InitialValue;
				_agent.SetDestination(_config.TargetPosition);
			}
			else
			{
				//_agent.SetDestination();
				OnPatrolState();
			}
		}
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
		//if(_enemy.IsOnWaveSpawner.isEnemyAlertTimer <= 0f)
		//{
		//	_enemy.IsOnWaveSpawner.isEnemyAlertTimer = 3f;
		//	float rx = Random.Range(-3, 3);
		//	float ry = Random.Range(-3, 3);
		//
		//	Vector3 PatrolV3 = new Vector3(_enemy.IsOnWaveSpawner._enemyWaveSO.waves[0].zero.x + rx, 0, _enemy.IsOnWaveSpawner._enemyWaveSO.waves[0].zero.z + ry);
		//	if (_isActiveAgent) _agent.SetDestination(PatrolV3);
		//}
		//_enemy.IsOnWaveSpawner.isEnemyAlertTimer -= Time.deltaTime;

		if (_enemy.IsSummonAlertTimer <= 0f)
		{
			_enemy.IsSummonAlertTimer = 2f;
			float rx = Random.Range(-3, 3);
			float ry = Random.Range(-3, 3);

			Vector3 PatrolV3 = new Vector3(_config.TargetPosition.x + rx, 0, _config.TargetPosition.z + ry);
			if (_isActiveAgent) _agent.SetDestination(PatrolV3);
		}
		_enemy.IsSummonAlertTimer -= Time.deltaTime;
	}

	/// <summary>
	/// 检测有没有发现敌人
	/// </summary>
	/// <returns></returns>
	private bool AcquireEnemyTarget()
	{
		//Collider[] targets = Physics.OverlapSphere(_enemy.transform.position, 10f, LayerMask.GetMask("Enemy"));//根据召唤物周围一圈检查可攻击的敌人
		Collider[] targets = Physics.OverlapSphere(_config.TargetPosition, 10f, LayerMask.GetMask("Enemy"));//根据玩家周围一圈检查可攻击的敌人
		List<Transform> targetsRange=new List<Transform>();
		foreach (var c in targets)
		{
			if (c.transform.tag.Contains("Enemy"))
			{
				targetsRange.Add(c.transform);
			}
		}
		if (targetsRange.Count > 0)
        {
			float dis;
			float minDis = 1000f;
			Transform target= targetsRange[0];
			foreach (var a in targetsRange)
			{
				dis = (_config.TargetPosition - a.transform.position).sqrMagnitude;
				if (dis < minDis)
				{
					minDis = dis;
					target = a;
				}
			}
			_config.EnemyTarget = target;
			_enemy.CurrentTarget = target.GetComponent<Damageable>();

			//int r = Random.Range(0, targetsRange.Count);
			//_config.EnemyTarget = targetsRange[r];
			//_enemy.CurrentTarget = targetsRange[r].transform.GetComponent<Damageable>();
			return true;
		}
		return false;
	}
}