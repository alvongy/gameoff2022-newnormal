using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeaponSO", menuName = "Weapon/Ranged")]
public class RangedWeaponSO : WeaponSO
{

	[SerializeField] ShootingModeSO[] _shootingModes;

	public ShootingModeSO[] ShootingModes { get => _shootingModes; set => _shootingModes = value; }

	private void OnEnable()
	{
		WeaponType = WeaponTypeEnum.RANGED;
	}
}
