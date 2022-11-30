using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BSAbility
{
	[TitleGroup("技能数值信息")]
	[InfoBox("初始属性")]
	//CoolDown: 冷却，ActiveTime:运行时间，Amount:伤害，HitDelay：延时，Number：数量 activeUpdate不使用activeTime 射击技能没有hitdelay
	[DictionaryDrawerSettings(KeyLabel = "名字", ValueLabel = "数值")]
	public Dictionary<UpgradeAttribute, NumericMetadata> DataPack = new Dictionary<UpgradeAttribute, NumericMetadata>()
	{
		{ UpgradeAttribute.CD, new NumericMetadata(1,1,0) },
		{ UpgradeAttribute.ActiveTime, new NumericMetadata(1,1,0) },
		{ UpgradeAttribute.Damage, new NumericMetadata(1,1,0) },
		{ UpgradeAttribute.HitDelay, new NumericMetadata(0.1f,1,0) }
	};

	//用来升级的数据
	[DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "数值")]
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
	Defense,				//防御
	Dodge,					//闪避
	Vampire,				//吸血
	CriticalRate,			//暴击率
	CriticalDamage,         //暴击伤害
	PenetrateDamage,        //穿透伤害
	EnemyBasicGold,			//掉落金币数量基数

	Number,
	Damage, //伤害
	Range,
	Speed, //速度
	ActiveTime, //持续时间
	CD,
	Length,
	EffectAmount,
	HitDelay,  //伤害延迟
	Level,
	Distance,
	ProjectileAmount,
	ChargeTimer,
	AtkSpeed,
	BounceNum,
	FlameDamage,//火焰伤害
	SideStepInvincibleTime,//翻滚无敌时间
	TriggerProbability,//触发几率
	ExeIncreace,//经验提升
	MaxHpIncreace,//生命值上限
	RecoverHp,//回血
	DropRate,//物品掉落几率
	CureIncreace,//治疗效果提升
	SkillDamageIncreace,//技能伤害提升
	CurrentNumCounter,//计数器，用于某种技能实时计算数量

}