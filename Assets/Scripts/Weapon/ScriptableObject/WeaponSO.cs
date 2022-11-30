using UnityEngine;

public class WeaponSO : ItemSO
{
	public enum WeaponTypeEnum
	{
		MELEE,
		RANGED,
		PLACED,
		ASSISTANCE,
	}
	[SerializeField] [ReadOnlyInspector] WeaponTypeEnum _weaponType = WeaponTypeEnum.MELEE;
	[SerializeField] float _switchTime = 0.3f;

	public WeaponTypeEnum WeaponType { get => _weaponType; set => _weaponType = value; }
}
