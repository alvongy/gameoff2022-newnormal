using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerAttributesSO", menuName = "EntityConfig/PlayerAttributesSO")]
public class PlayerAttributesSO : ScriptableObject
{
	[Tooltip("The initial")]
	[SerializeField][ReadOnlyInspector] private int _maxValue;
	[SerializeField] private int _currentValue;

	public UnityAction OnValueChanged = delegate { };
	public int MaxValue => _maxValue;
	public int CurrentValue => _currentValue;

	public void SetMaxvalue(int newValue)
	{
		_maxValue = newValue;
	}

	public void ForceSetCurrentValue(int newValue)
	{
		_currentValue = newValue;
		OnValueChanged.Invoke();
	}

	public void Decrease(int DamageValue)
	{
		_currentValue -= DamageValue;
		OnValueChanged.Invoke();
	}

	public void Increace(int HealthValue, bool clamp = true)
	{
		_currentValue += HealthValue;
		if (clamp && _currentValue > _maxValue)
		{
			_currentValue = _maxValue;
		}
		OnValueChanged.Invoke();
	}
}

