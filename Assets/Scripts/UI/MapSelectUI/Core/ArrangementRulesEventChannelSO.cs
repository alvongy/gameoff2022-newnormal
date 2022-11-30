using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Events/ArrangementRules Event Channel")]
public class ArrangementRulesEventChannelSO : SerializedScriptableObject
{
	public enum ArrangeType
	{
		Line,
		Oblique,
		Alone,
		Squares,
		Homochromatic
	}
	public enum ColorType
    {
		Red,
		Green,
		Blue
    }

	[SerializeField]
	public class ColorNum
    {
		public ColorType colorType;
		public int nums;
	}

	[DrawWithUnity]
	public LocalizedString ArrangementDes;
	public event UnityAction<bool> OnEventRaised;

	[HideInInspector]public bool IsActive;
	public ArrangeType arrangeType;
	public MFAbility Ability;
	public int RuleLevel;
	public Dictionary<CharacterDataType, CharacterData> DataPack = new Dictionary<CharacterDataType, CharacterData>();
	public List<ColorNum> cardColorNums = new List<ColorNum>();

	/// <summary>
	/// 是否解锁该加成接口
	/// </summary>
	/// <param name="value"></param>
	public void RaiseEvent(bool value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
        if (value){ IsActive = value; }
	}
}
