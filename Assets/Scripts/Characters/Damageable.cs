using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	[Header("Health")]
	[SerializeField] public PlayerAttributesConfigSO _healthConfigSO;
	[SerializeField] public PlayerAttributesSO _currentHealthSO;

	[Header("Combat")]
	//[SerializeField] private GetHitEffectConfigSO _getHitEffectSO;
	[SerializeField] private DroppableRewardConfigSO _droppableRewardSO;
	[SerializeField] private int _exp = 1;
	[SerializeField] private int _energy = 1;
	[SerializeField] bool _enableHitEffect = false;
	[SerializeField] private Renderer _mainMeshRenderer;
	[SerializeField] private GameObject _deaingEffect;

	[Header("Broadcasting On")]
	[SerializeField] private TransformAnchor _playerTransformAnchor = default;
	[SerializeField] private VoidEventChannelSO _deathEvent = default;
	[SerializeField] private IntEventChannelSO _playerGetExperience = default;
	[SerializeField] private IntEventChannelSO _playerGetEnergy = default;
	[SerializeField] private VoidEventChannelSO _camShake = default;
	[SerializeField] private DamageDisplayChannelSO _damageDisplayChannelSO = default;

	[Header("Listening To")]
	[SerializeField] private IntEventChannelSO _restoreHealth = default; //Getting cured when eating food

	[Header("是否属于任务事件")]
	public bool IsOnQuestUnit;
	[Sirenix.OdinInspector.ShowIf("@(this.IsOnQuestUnit)==true")]
	[SerializeField] private IntEventChannelSO OnEnemyIDDeathSO;
	[Sirenix.OdinInspector.ShowIf("@(this.IsOnQuestUnit)==true")]
	[SerializeField] private int OnEnemyIDDeath;
	[SerializeField] private bool IsOnPlayer;

	public DroppableRewardConfigSO DroppableRewardConfig => _droppableRewardSO;
	//Flags that the StateMachine uses for Conditions to move between states
	public bool GetHit { get; set; }
	public bool IsDead;
	public bool IsCanDamageObj;

	private bool _hyperMuteki = false;

	//public GetHitEffectConfigSO GetHitEffectConfig => _getHitEffectSO;
	public Renderer MainMeshRenderer => _mainMeshRenderer; //used to apply the hit flash effect

	public bool HyperMuteki { get => _hyperMuteki; set => _hyperMuteki = value; }

	public event UnityAction<Damageable> OnDie;

	private MaterialPropertyBlock _normalBlock;
	private MaterialPropertyBlock _hitBlock;
	private bool _inHitEffect = false;
	private float _effectEndTime = 0f;

	private void Awake()
	{
		//If the HealthSO hasn't been provided in the Inspector (as it's the case for the player),
		//we create a new SO unique to this instance of the component. This is typical for enemies.
		if (_currentHealthSO == null)
		{
			_currentHealthSO = ScriptableObject.CreateInstance<PlayerAttributesSO>();
		}
		_currentHealthSO.SetMaxvalue(_healthConfigSO.InitialValue);
		_currentHealthSO.ForceSetCurrentValue(_healthConfigSO.InitialValue);

		if (_enableHitEffect)
		{
			//_mainMeshRenderer.GetPropertyBlock(_normalBlock);
			_normalBlock = new MaterialPropertyBlock();
			_normalBlock.SetFloat(Shader.PropertyToID("_Hit1"), 0f);
			_hitBlock = new MaterialPropertyBlock();
			_hitBlock.SetFloat(Shader.PropertyToID("_Hit1"), 1f);
		}
	}

	private void OnEnable()
	{
		if (_restoreHealth != null)
			_restoreHealth.OnEventRaised += Cure;

	}

	private void OnDisable()
	{
		if (_restoreHealth != null)
			_restoreHealth.OnEventRaised -= Cure;
	}
	[ContextMenu("a")]
	void Test()
	{
		ReceiveAnAttack(1);
	}
	public void ReceiveAnAttack(int damage)
    {
        if (_playerTransformAnchor==null) { return; }
		if (IsOnPlayer && (IsDead || _playerTransformAnchor.Value.GetComponent<MainCharacter>().characterState == CharacterStateType.INVINCIBLE))
        {
			return;
		}
		if (_damageDisplayChannelSO != null)
		{
			_damageDisplayChannelSO.RaiseEvent(transform.position, damage);
		}
		_currentHealthSO.Decrease(damage);

        if (IsOnPlayer)
        {
			_playerTransformAnchor.Value.GetComponent<MainCharacter>()._InvincibleCD.ForceSetCurrentValue(2);

		}

		if (_hyperMuteki)
		{
			_currentHealthSO.Increace(damage);
		}

		if (_camShake != null)
		{
			_camShake.RaiseEvent();
		}

		GetHit = true;

		if (_currentHealthSO.CurrentValue <= 0)
		{
            if (OnEnemyIDDeathSO!=null) { OnEnemyIDDeathSO.RaiseEvent(OnEnemyIDDeath); }
			

			IsDead = true;

			if (OnDie != null)
			{
				OnDie.Invoke(this);
			}
			if (_deathEvent != null)
			{
				_deathEvent.RaiseEvent();
			}
			//if (_playerGetExperience != null)
			//{
			//	_playerGetExperience.RaiseEvent(_exp);//打死加经验移除
			//}
			//if (_playerGetEnergy != null)
			//{
			//	_playerGetEnergy.RaiseEvent(_energy);//打死加能量移除
			//	YKDataInfoManager.Instance._energyTempNum += _energy;
			//}
			if (IsCanDamageObj) { Destroy(this.gameObject); }
			
			//_currentHealthSO.ForceSetCurrentValue(_healthConfigSO.InitialValue);
		}
		PlayHitEffect();
	}
	public void AddTempHp(int count)
    {
		_currentHealthSO.Increace(count, false);

	}
	public void Kill()
	{
		ReceiveAnAttack(_currentHealthSO.CurrentValue);
	}

	/// <summary>
	/// Called by the StateMachine action ResetHealthSO. Used to revive the Rock critters.
	/// </summary>
	public void Revive()
	{
		IsDead = false;
		_currentHealthSO.ForceSetCurrentValue(_healthConfigSO.InitialValue);

		//if (_updateHealthUI != null)
		//	_updateHealthUI.RaiseEvent();

		_inHitEffect = false;
        if (_mainMeshRenderer != null) { _mainMeshRenderer.SetPropertyBlock(_normalBlock); }

		if (_deaingEffect != null)
		{
			_deaingEffect.SetActive(false);
			if (_mainMeshRenderer != null) { _mainMeshRenderer.gameObject.SetActive(true); }
		}
	}

	/// <summary>
	/// Used for cure events, like eating food. Triggered by an IntEventChannelSO.
	/// </summary>
	private void Cure(int healthToAdd)
	{
		if (IsDead)
			return;

		_playerTransformAnchor.Value.GetComponent<MainCharacter>().RuntimeAdditionalDic[CharacterDataType.SkillDamageIncreace].Data +=
			_playerTransformAnchor.Value.GetComponent<MainCharacter>().RuntimeAdditionalDic[CharacterDataType.SkillDamageMultiply].Data;

		float hpMultiply = healthToAdd * _playerTransformAnchor.Value.GetComponent<MainCharacter>().RuntimeAdditionalDic[CharacterDataType.CureIncreace].GetData();
		_currentHealthSO.Increace((int)hpMultiply);
	}
	private void PlayHitEffect()
	{
		if (!_enableHitEffect)
		{
			return;
		}
		if (_inHitEffect)
		{
			_effectEndTime = Time.time + 0.1f;
		}
		else
		{
			StartCoroutine(HitEffect());
		}
	}
	public void PlayDeaingEffect()
	{
		if (_deaingEffect != null)
		{
			_deaingEffect.SetActive(true);
			if (_mainMeshRenderer != null) { _mainMeshRenderer.gameObject.SetActive(false); }
		}
	}
	IEnumerator HitEffect()
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		_inHitEffect = true;
		if (_mainMeshRenderer!=null) { _mainMeshRenderer.SetPropertyBlock(_hitBlock); }
		_effectEndTime = Time.time + 0.1f;
		while (Time.time < _effectEndTime)
		{
			yield return wait;
		}
		_inHitEffect = false;
		if (_mainMeshRenderer != null) { _mainMeshRenderer.SetPropertyBlock(_normalBlock); }
	}
}
