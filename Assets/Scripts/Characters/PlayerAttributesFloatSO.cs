using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerAttributesFloatSO", menuName = "EntityConfig/PlayerAttributesFloatSO")]
public class PlayerAttributesFloatSO : ScriptableObject
{
	[Tooltip("The initial")]
	[SerializeField] [ReadOnlyInspector] private float _maxValue;
	[SerializeField] [ReadOnlyInspector] private float _currentValue;

	public UnityAction OnValueChanged = delegate { };
	public float MaxValue => _maxValue;
	public float CurrentValue => _currentValue;

	public void SetMaxvalue(float newValue)
	{
		_maxValue = newValue;
	}

	public void ForceSetCurrentValue(float newValue)
	{
		_currentValue = newValue;
		OnValueChanged.Invoke();
	}

	public void Decrease(float DamageValue)
	{
		_currentValue -= DamageValue;
		OnValueChanged.Invoke();
	}

	public void Increace(float HealthValue, bool clamp = true)
	{
		_currentValue += HealthValue;
		if (clamp && _currentValue > _maxValue)
		{
			_currentValue = _maxValue;
		}
		OnValueChanged.Invoke();
	}
}
