using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSAbilityHolder : MonoBehaviour
{
	//运行时的数据
	public Dictionary<UpgradeAttribute, NumericMetadata> RuntimeDataPack;
	public Dictionary<UpgradeAttribute, BSCounterTrigger> RuntimeTimerDic;
	public Dictionary<UpgradeAttribute, BSCounterTrigger> RuntimeCustomCounterDic;

	[HideInInspector]
	public BSAbility ability;

	List<UpgradeAttribute> list;

    private void Awake()
    {
		list = new List<UpgradeAttribute>();
	}

    private void Update()
    {
		list.Clear();
		list.AddRange(RuntimeTimerDic.Keys);
		foreach (var key in list)
		{
			RuntimeTimerDic[key].CounterChange(this, -Time.deltaTime);
		}
	}


    public virtual void ResetData()
	{
		if (RuntimeDataPack == null)
		{
			RuntimeDataPack = new Dictionary<UpgradeAttribute, NumericMetadata>();
		}
		else
		{
			RuntimeDataPack.Clear();
		}
		if (RuntimeTimerDic == null)
		{
			RuntimeTimerDic = new Dictionary<UpgradeAttribute, BSCounterTrigger>();
		}
		else
		{
			RuntimeTimerDic.Clear();
		}
		if (RuntimeCustomCounterDic == null)
		{
			RuntimeCustomCounterDic = new Dictionary<UpgradeAttribute, BSCounterTrigger>();
		}
		else
		{
			RuntimeCustomCounterDic.Clear();
		}
	}

	public void Init(BSAbility ab)
	{
		ResetData();
		ability = ab;
		foreach (var kv in ability.DataPack)
		{
			RuntimeDataPack.Add(kv.Key, kv.Value.Copy());
		}
		ability.Initialize(this);
	}
}
