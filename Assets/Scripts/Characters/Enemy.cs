using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[HideInInspector] public bool IsPlayerInAlertZone;
	[HideInInspector] public bool IsPlayerInAttackZone;
	[HideInInspector] public WaveSpawner IsOnWaveSpawner;
	public float IsSummonAlertTimer;//传递刷新巡逻的时间
	public bool IsSummon;
	[HideInInspector] public bool isFindEnemy;

	[SerializeField] protected TransformAnchor _playerTransformAnchor;

	[SerializeField] protected int _attack = 1;

	[SerializeField] private PlayerAttributesFloatConfigSO _enemyMoveSpeedConfig;

	[SerializeField] public bool isPlayerOn = false;

	[ReadOnlyInspector] public Damageable CurrentTarget; //The StateMachine evaluates its health when needed

	public PlayerAttributesFloatConfigSO EnemyMoveSpeedConfig { get => _enemyMoveSpeedConfig; set => _enemyMoveSpeedConfig = value; }

	private void Start()
	{
		//CurrentTarget = _playerTransformAnchor.Value?.GetComponent<Damageable>();
		IsPlayerInAlertZone = !ReferenceEquals(CurrentTarget, null);

		IsPlayerInAlertZone = false;
	}
	private void OnDisable()
	{
		IsPlayerInAttackZone = false;
        if (IsOnWaveSpawner!=null && IsOnWaveSpawner._enemyList.Contains(gameObject))
        {
			IsOnWaveSpawner._enemyList.Remove(gameObject);
		}

	}
    private void Update()
    {
  //      if (IsSummon && !isFindEnemy)
  //      {
		//	isFindEnemy = AcquireEnemyTarget();

		//}
	}

    public void OnAlertTriggerChange(bool entered, GameObject who)
	{
        //IsPlayerInAlertZone = entered;

        //if (entered && who.TryGetComponent(out Damageable d))
        //{
        //    CurrentTarget = d;
        //    CurrentTarget.OnDie += OnTargetDead;
        //}
        //else
        //{
        //    CurrentTarget = null;
        //}
    }

	public void OnAttackTriggerChange(bool entered, GameObject who)
	{
		IsPlayerInAttackZone = entered;
        if (entered && who.TryGetComponent(out Damageable d))
        {
            CurrentTarget = d;
            CurrentTarget.OnDie += OnTargetDead;
        }
        else
        {
            CurrentTarget = null;
        }
        //No need to set the target. If we did, we would get currentTarget to null even if
        //a target exited the Attack zone (inner) but stayed in the Alert zone (outer).
    }

	private void OnTargetDead(Damageable en)
	{
		CurrentTarget = null;
		IsPlayerInAlertZone = false;
		IsPlayerInAttackZone = false;
	}

	//攻击动画中使用关键帧调用此攻击方法
	public virtual void AttackTarget()
	{
		if (IsPlayerInAttackZone&& CurrentTarget)
		{
			CurrentTarget.ReceiveAnAttack(_attack);
		}
	}
}
