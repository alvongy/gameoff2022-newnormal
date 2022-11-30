using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponSO", menuName = "Weapon/Melee")]
public class MeleeWeaponSO : WeaponSO
{
	private void OnEnable()
	{
		WeaponType = WeaponTypeEnum.MELEE;
	}
}
