using ArtSetting;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDataType
{
	MaxHP,//���Ѫ��
	TempHP,//��ʱѪ��
	BasicAttack,//����������
	PickRange,//ʰȡ��Χ
	MoveSpeed,//�ƶ��ٶ�
	AttackSpeed,//�����ٶ�
	ProjectileSpeed,//�����ٶ�
	BounceNum,//�������
	FlameDamage,//�����˺�
	SideStepDistance,//��������
	SideStepCD,//������ȴʱ��
	SideStepInvincibleTime,//�����޵�ʱ��
	InvincibleTime,  //�޵���ȴʱ��
	ExeIncreace,//�����ȡ����
	CureIncreace,//����Ч������
	SkillDamageIncreace,//�����˺�����
	SkillDamageMultiply,//���μ����˺���������
	Diffusion, //��ɢ
	RecoverHp,//��Ѫ��
	PickUpExpTriggerProbability, //ʰȡ��괥�������¼�����

}
public enum CharacterStateType
{
	NORMAL,
	INVINCIBLE,
}

public class CharacterData
{
	/// <summary>
	/// �˺�ʲô��
	/// </summary>
	public float Data = 1f;
	/// <summary>
	/// ������ֵ����
	/// </summary>
	public float DataRatio = 1f;
	public CharacterData(float d, float dr)
	{
		Data = d;
		DataRatio = dr;
	}
	public float GetData()
	{
		return (Data * DataRatio);
	}
}

public class MainCharacter : SerializedMonoBehaviour
{
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private WeaponManager _weaponManager = default;
	[SerializeField] private IntEventChannelSO _playerGetExperience = default;
	[SerializeField] private int[] _levelExperienceList;
	[SerializeField] public PlayerAttributesSO _experienceSO;
	[SerializeField] private PlayerAttributesSO _MoveSpeedSO;
	[SerializeField] private GameDataEquation _gameDataEquationSO;
	[SerializeField] private VoidEventChannelSO _LevelUpChannelSO;
	[SerializeField] private VoidEventChannelSO _SideStepChannelSO;
	[SerializeField] private PlayerAttributesFloatSO _SideStepCD;
	[SerializeField] public PlayerAttributesFloatSO _InvincibleCD;
	[SerializeField] VoidEventChannelSO OpenAbilityChoiceUIEvent;

	public bool CanSideStep { get => _SideStepCD .MaxValue<= _SideStepCD.CurrentValue; }
	//[SerializeField] private Collider _pickerCollider;
    public SphereCollider _pickerColliders;
	public EffectManage coverEffectManage;
	//public float _pickRadius = 3f;
	//public float _basicAttack = 1;

	private Vector2 _inputVector;
	private float _previousSpeed;
	private int _playerExperience;
	private int _skillPoint = 0;
	private bool _disableSelectCord = false;
	private bool isOnCover = false;

	private float _startTime;
	private string _startTimeStr;
	private float shankeTime;

	private float exeMultiply;

	//��������ʱ�ٶ�
	private bool runningShooting;
	private float runningShootingSpeed;
	private float runningShootingTime;
	private float runningShootingTimer;

	public int _playerLevel = 0;
	[NonSerialized] public Vector3 movementInput;
	[NonSerialized] public Vector3 movementVector;
	[NonSerialized] public bool sidestepInput;
	[NonSerialized] public bool inFire;
	[NonSerialized] public bool inMelee;
	[NonSerialized] public bool isRunningShooting;
	[NonSerialized] public CharacterStateType characterState;
	//[NonSerialized] public float additionalSpeed=1f;

	[InfoBox("�ӳ���Ϣ")]
	[SerializeField, DictionaryDrawerSettings(KeyLabel = "�ӳ�����", ValueLabel = "��ֵ")]
	public Dictionary<CharacterDataType, CharacterData> RuntimeAdditionalDic = new Dictionary<CharacterDataType, CharacterData>()
	{
		{ CharacterDataType.MaxHP, new CharacterData(0,1) },
		{ CharacterDataType.TempHP, new CharacterData(0,1) },
		{ CharacterDataType.BasicAttack, new CharacterData(1,1) },
		{ CharacterDataType.PickRange, new CharacterData(1,1) },
		{ CharacterDataType.MoveSpeed, new CharacterData(1,1) },
		{ CharacterDataType.AttackSpeed, new CharacterData(1,1) },
		{ CharacterDataType.ProjectileSpeed,new CharacterData(1,1) },
		{ CharacterDataType.BounceNum,new CharacterData(1,1) },
		{ CharacterDataType.FlameDamage,new CharacterData(1,1) },
		{ CharacterDataType.SideStepDistance,new CharacterData(1,1) },
		{ CharacterDataType.SideStepCD,new CharacterData(1,1) },
		{ CharacterDataType.SideStepInvincibleTime,new CharacterData(1,1) },
		{ CharacterDataType.InvincibleTime,new CharacterData(1,1) },
		{ CharacterDataType.Diffusion,new CharacterData(1,1) },
		{ CharacterDataType.ExeIncreace,new CharacterData(1,1) },
		{ CharacterDataType.CureIncreace,new CharacterData(1,1) },
		{ CharacterDataType.SkillDamageIncreace,new CharacterData(1,1) },
	};

	public bool DisableSelectCord { get => _disableSelectCord; set => _disableSelectCord = value; }

	private void Start()
	{
		//_experienceSO.SetMaxvalue(_gameDataEquationSO.GetLevelExpNeed(_playerLevel));
		//_experienceSO.Decrease(_experienceSO.CurrentValue);
		_pickerColliders.radius = RuntimeAdditionalDic[CharacterDataType.PickRange].GetData();

		RuntimeAdditional();
		GlobalDataInit();
	}
	private void OnEnable()
	{
		_inputReader.MoveEvent += OnMove;
		_inputReader.SidestepEvent += OnSidestep;
		_playerGetExperience.OnEventRaised += AddExperience;
		_LevelUpChannelSO.OnEventRaised += PlayerLevelUp;
		 _startTime = Time.time;
		_startTimeStr = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
	}

	private void OnDisable()
	{
		_inputReader.MoveEvent -= OnMove;
		_inputReader.SidestepEvent -= OnSidestep;
		_playerGetExperience.OnEventRaised -= AddExperience;
	}

	private void Update()
	{
		RecalculateMovement();
		SideStepCD();
		SideStepSpeedUp();
		InvincibleCD();
	}

	void GlobalDataInit()
    {
		_playerLevel = YKDataInfoManager.Instance._globalPlayerLevel;

		YKDataInfoManager.Instance._isPlayerDie = false;
		YKDataInfoManager.Instance._energyTempNum = 0;
	}

	/// <summary>
	/// �������ӳ�Ч��
	/// </summary>
	private void RuntimeAdditional()
    {
		if (YKRewardWatcher.Instance != null)
		{
			foreach (var r in YKRewardWatcher.Instance.storer.RewardStorerFloat)//ÿ�����¼���ļӳ�
			{
				//if (r.Key == CharacterDataType.MaxHP) { RuntimeAdditionalDic[CharacterDataType.MaxHP].AddData(r.Value); }
				//else if (r.Key == CharacterDataType.BasicAttack) { RuntimeAdditionalDic[CharacterDataType.BasicAttack].AddData(r.Value); }
				//else if (r.Key == CharacterDataType.ProjectileSpeed) { RuntimeAdditionalDic[CharacterDataType.MoveSpeed].AddData(r.Value); }
				//else if (r.Key == CharacterDataType.PickRange) { RuntimeAdditionalDic[CharacterDataType.PickRange].AddData(r.Value); }
				//else if (r.Key == CharacterDataType.AttackSpeed) { RuntimeAdditionalDic[CharacterDataType.AttackSpeed].AddData(r.Value); }
                //else { Debug.Log("δ�ҵ���Ӧ�ļӳ�Ч����"+ r.Key); }

				if (RuntimeAdditionalDic.ContainsKey(r.Key))
				{
					RuntimeAdditionalDic[r.Key].Data += r.Value.Data;
					RuntimeAdditionalDic[r.Key].DataRatio += r.Value.DataRatio;
				}
			}
		}

		//���Ѫ���ӳ�
		int additionalHp = gameObject.GetComponent<Damageable>()._healthConfigSO.InitialValue + (int)RuntimeAdditionalDic[CharacterDataType.MaxHP].GetData();
		gameObject.GetComponent<Damageable>()._currentHealthSO.SetMaxvalue(additionalHp);
		gameObject.GetComponent<Damageable>()._currentHealthSO.ForceSetCurrentValue(additionalHp);
		//Debug.Log("���Ѫ���ӳɡ�additionalHp��"+ additionalHp+"��RuntimeAdditionalDic[CharacterDataType.MAXHP].GetData()��"+ (int)RuntimeAdditionalDic[CharacterDataType.MAXHP].GetData());

		//�ٶȼӳ�
	   // additionalSpeed= (RuntimeAdditionalDic.ContainsKey(CharacterDataType.SPEED) ? RuntimeAdditionalDic[CharacterDataType.SPEED].GetData() : 1);
	    //additionalSpeed= ((RuntimeAdditionalDic[CharacterDataType.SPEED].GetData()!=0) ? RuntimeAdditionalDic[CharacterDataType.SPEED].GetData() : 1);

		//ʰȡ��Χ�ӳ�
		//_pickRadius += (RuntimeAdditionalDic.ContainsKey(CharacterDataType.PICKRANGE) ? RuntimeAdditionalDic[CharacterDataType.PICKRANGE].GetData() : 0);

		//�������ӳ�: ����һ���������֮����Ӷ�Ӧ�ӳ�
		//_basicAttack += (RuntimeAdditionalDic.ContainsKey(CharacterDataType.ATTACK) ? RuntimeAdditionalDic[CharacterDataType.ATTACK].GetData() : 0);

		//���ټӳɣ�Ԥ��

	}
	private void RecalculateMovement()
	{
		float targetSpeed;
		Vector3 adjustedMovement;
		Vector3 movement;

		adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);
		//Accelerate/decelerate
		targetSpeed = Mathf.Clamp01(_inputVector.magnitude) * _MoveSpeedSO.CurrentValue;
		if (targetSpeed > 0f)
		{
			//if (isRunning)
			//	targetSpeed = 1f;

			//if (attackInput)
			//	targetSpeed = .05f;
		}
        //	targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4f);
        if (RuntimeAdditionalDic.ContainsKey(CharacterDataType.MoveSpeed)) 
		{
			movement = adjustedMovement.normalized * RuntimeAdditionalDic[CharacterDataType.MoveSpeed].GetData();//��������ϵ��ٶ������üӷ��ŵ��������
		}
        else
        {
			movement = adjustedMovement.normalized;
		}
		

		movementInput = adjustedMovement.normalized * targetSpeed + movement;
		_previousSpeed = targetSpeed;

	}

	private void OnMove(Vector2 arg0) => _inputVector = arg0;

	private void OnSidestep(bool arg0)
	{
		sidestepInput = arg0;
	}
	public void RaiseSideStep()
	{
		float variate = (RuntimeAdditionalDic.ContainsKey(CharacterDataType.SideStepCD))? RuntimeAdditionalDic[CharacterDataType.SideStepCD].GetData():0;
		//float invincible = (RuntimeAdditionalDic.ContainsKey(CharacterDataType.InvincibleTime))? RuntimeAdditionalDic[CharacterDataType.InvincibleTime].GetData():0;
		float invincible = (RuntimeAdditionalDic.ContainsKey(CharacterDataType.SideStepInvincibleTime))? RuntimeAdditionalDic[CharacterDataType.SideStepInvincibleTime].GetData():0;

		_SideStepChannelSO?.RaiseEvent();
		_SideStepCD.ForceSetCurrentValue( 0f + variate);//��������CD
		_InvincibleCD.ForceSetCurrentValue(0.5f+ invincible);//����޵�ʱ��
	}

	/// <summary>
	/// ��������
	/// </summary>
	void SideStepCD() 
	{
		if (_SideStepCD.MaxValue > _SideStepCD.CurrentValue) 
		{
			_SideStepCD.Increace(Time.deltaTime,true);
			if (_SideStepCD.MaxValue <= _SideStepCD.CurrentValue)
			{
				_SideStepCD.ForceSetCurrentValue((float)_SideStepCD.MaxValue);
                if (isRunningShooting) 
				{ 
					runningShooting = true;
					RuntimeAdditionalDic[CharacterDataType.MoveSpeed].DataRatio += runningShootingSpeed;
				}
			}
		}
	}
	void SideStepSpeedUp()
    {
        if (!runningShooting) { return; }

		if (runningShootingTime > runningShootingTimer)
        {
			runningShootingTimer += Time.deltaTime;
            if (runningShootingTime <= runningShootingTimer)
            {
				runningShooting = false;	
				RuntimeAdditionalDic[CharacterDataType.MoveSpeed].DataRatio -= runningShootingSpeed;
				runningShootingTimer = 0;
			}
		}
    }
	public void RunningShooting(float speed,float time)
    {
		isRunningShooting = true;
		runningShootingSpeed = speed;
		runningShootingTime = time;
	}

	void InvincibleCD()
	{
		if (_InvincibleCD.MaxValue < _InvincibleCD.CurrentValue)
		{
			characterState = CharacterStateType.INVINCIBLE;
			_InvincibleCD.Decrease(Time.deltaTime);

			PlayerFlicker();

			if (_InvincibleCD.MaxValue >= _InvincibleCD.CurrentValue)
			{
				_InvincibleCD.ForceSetCurrentValue((float)_InvincibleCD.MaxValue);
				characterState = CharacterStateType.NORMAL;
				coverEffectManage.gameObject.SetActive(false);
				isOnCover = false;
			}
		}
	}
	void PlayerFlicker()
    {
        if (!isOnCover)
        {
			isOnCover = true;
			coverEffectManage.gameObject.SetActive(true);
			coverEffectManage.Play();
		}
		//shankeTime += Time.deltaTime;
		//if (shankeTime % 1 > 0.5f)
		//{
		//	//transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
		//	transform.GetChild(0).gameObject.SetActive(false);
		//}
		//else
		//{
		//	//transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
		//	transform.GetChild(0).gameObject.SetActive(true);
		//}
	}

	private void OnAttackMain(bool arg0)
	{

	}

	private bool _inAblilitySelecting = false;

	private void PlayerLevelUp()
	{
		_pickerColliders.radius = RuntimeAdditionalDic[CharacterDataType.PickRange].GetData();
	}

#if UNITY_EDITOR
	[SerializeField] private bool _disableAddExp = false;

	private void AddExperience(int e)
	{
		if (_disableAddExp)
		{
			return;
		}
#else
	private void AddExperience(int e)
	{
#endif
		AddExpSpecialHandling(e);

		while (_levelExperienceList.Length > _playerLevel && _levelExperienceList[_playerLevel] <= _experienceSO.CurrentValue)
		{
			_playerLevel++;
			_experienceSO.SetMaxvalue(_levelExperienceList[_playerLevel]);
			//_experienceSO.InflictDamage(_levelExperienceList[_playerLevel - 1]);
			_experienceSO.ForceSetCurrentValue(0);
			_skillPoint++;
		}

		//int levelExpNeed = 1 << _playerLevel;
		int levelExpNeed=_gameDataEquationSO.GetLevelExpNeed(_playerLevel + 1);
		while (_experienceSO.MaxValue <= _experienceSO.CurrentValue)
		{
			_experienceSO.Decrease(_experienceSO.MaxValue);
			_playerLevel++;
			_experienceSO.SetMaxvalue(_gameDataEquationSO.GetLevelExpNeed(_playerLevel));
			_skillPoint++;
			YKDC.YKDC.PostLevelUpdate(_playerLevel.ToString(), _startTimeStr, ((int)(Time.time - _startTime)).ToString());

			//try
			//{
			//	DobestAnalysisSDK.Instance.SetCustomEvent(
			//		"LevelUpdate",
			//		"LevelUpdate",
			//		new DobestAnalysisEvent.CustomInfo
			//		{
			//			custom = new System.Collections.Generic.Dictionary<string, object>
			//		{
			//										{"Duration",((int)(Time.time - _startTime)).ToString() },
			//										{"evel_index",_playerLevel},
			//										{"dentify",DateTimeOffset.Now.ToUnixTimeSeconds()},
			//										{"duration",((int)(Time.time - _startTime)) },
			//		}
			//		},
			//		"LevelUpdate");
			//}
			//catch (Exception)
			//{
			//}
		}
		if (!_inAblilitySelecting)
		{
			AddAbilityIfPointRemain();
		}
	}

	/// <summary>
	/// ʰȡ���������⴦��
	/// </summary>
	/// <param name="e"></param>
	private void AddExpSpecialHandling(int e)
    {
		exeMultiply = (RuntimeAdditionalDic.ContainsKey(CharacterDataType.ExeIncreace) ? RuntimeAdditionalDic[CharacterDataType.ExeIncreace].GetData() : 1f);
		_experienceSO.Increace((int)(e * exeMultiply), false);

		float probability= (RuntimeAdditionalDic.ContainsKey(CharacterDataType.PickUpExpTriggerProbability) ? RuntimeAdditionalDic[CharacterDataType.PickUpExpTriggerProbability].GetData() : 0f);

		if (probability != 0)
        {
			float p = UnityEngine.Random.value;
            if (p <= probability)
            {
				float hp = (RuntimeAdditionalDic.ContainsKey(CharacterDataType.RecoverHp) ? RuntimeAdditionalDic[CharacterDataType.RecoverHp].GetData() : 0f);
				gameObject.GetComponent<Damageable>()._currentHealthSO.Increace((int)hp);
				//Debug.Log("����ʰȡ�����Ѫ���츳����Ѫ����"+hp);
			}
        }
	}

	public void AddAbilityIfPointRemain()
	{
		if (!_disableSelectCord && _skillPoint > 0)
		{
			_inAblilitySelecting = true;
			Time.timeScale = 0f;
			_skillPoint--;
			//MFAbilityHandler.Instance.StartAbilitySelect();
			//if (_LevelUpChannelSO != null)
			//{
			//	_LevelUpChannelSO.RaiseEvent();
			//}
            if (OpenAbilityChoiceUIEvent!=null)
            {
				OpenAbilityChoiceUIEvent.RaiseEvent();
			}

		}
		else
		{
			_inAblilitySelecting = false;
			Time.timeScale = 1f;
		}
	}
	[ContextMenu("Add Exp")]
	void AddExp()
	{
		AddExperience(1);
	}
}
