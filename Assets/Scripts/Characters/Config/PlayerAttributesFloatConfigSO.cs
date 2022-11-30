using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributesFloatConfigSO", menuName = "EntityConfig/PlayerAttributesFloatConfigSO")]
public class PlayerAttributesFloatConfigSO : ScriptableObject
{
	[SerializeField] private float _initialValue;

	public float InitialValue => _initialValue;
}