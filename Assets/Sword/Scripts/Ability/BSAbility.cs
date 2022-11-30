using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BSAbility
{
	[TitleGroup("������ֵ��Ϣ")]
	[InfoBox("��ʼ����")]
	//CoolDown: ��ȴ��ActiveTime:����ʱ�䣬Amount:�˺���HitDelay����ʱ��Number������ activeUpdate��ʹ��activeTime �������û��hitdelay
	[DictionaryDrawerSettings(KeyLabel = "����", ValueLabel = "��ֵ")]
	public Dictionary<UpgradeAttribute, NumericMetadata> DataPack = new Dictionary<UpgradeAttribute, NumericMetadata>()
	{
		{ UpgradeAttribute.CD, new NumericMetadata(1,1,0) },
		{ UpgradeAttribute.ActiveTime, new NumericMetadata(1,1,0) },
		{ UpgradeAttribute.Damage, new NumericMetadata(1,1,0) },
		{ UpgradeAttribute.HitDelay, new NumericMetadata(0.1f,1,0) }
	};

	//��������������
	[DictionaryDrawerSettings(KeyLabel = "�ȼ�", ValueLabel = "��ֵ")]
	public Dictionary<int, List<NumericMetadata>> DataPackUpgrade = new Dictionary<int, List<NumericMetadata>>()
	{

	};

	public abstract void Initialize(BSAbilityHolder holder);
	public virtual void OnLevelUp(MFAbilityHolder holder) { }
	public virtual void OnUpdate(MFAbilityHolder holder) { }

}

public enum UpgradeAttribute
{
	None,
	TotalHp,
	Recover,
	AttackLowerLimit,
	AttackUpperLimit,
	MoveSpeed,
	AttackSpeed,
	Defense,				//����
	Dodge,					//����
	Vampire,				//��Ѫ
	CriticalRate,			//������
	CriticalDamage,         //�����˺�
	PenetrateDamage,        //��͸�˺�
	EnemyBasicGold,			//��������������

	Number,
	Damage, //�˺�
	Range,
	Speed, //�ٶ�
	ActiveTime, //����ʱ��
	CD,
	Length,
	EffectAmount,
	HitDelay,  //�˺��ӳ�
	Level,
	Distance,
	ProjectileAmount,
	ChargeTimer,
	AtkSpeed,
	BounceNum,
	FlameDamage,//�����˺�
	SideStepInvincibleTime,//�����޵�ʱ��
	TriggerProbability,//��������
	ExeIncreace,//��������
	MaxHpIncreace,//����ֵ����
	RecoverHp,//��Ѫ
	DropRate,//��Ʒ���伸��
	CureIncreace,//����Ч������
	SkillDamageIncreace,//�����˺�����
	CurrentNumCounter,//������������ĳ�ּ���ʵʱ��������

}