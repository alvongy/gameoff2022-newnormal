using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributesConfigSO", menuName = "EntityConfig/PlayerAttributesConfigSO")]
public class PlayerAttributesConfigSO : ScriptableObject
{
	[SerializeField] private int _initialValue = 1;

	public int InitialValue => _initialValue;
}
