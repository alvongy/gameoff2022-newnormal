using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorySO", menuName = "Weapon/Armory")]
public class ArmorySO : DescriptionBaseSO
{
	[SerializeField] List<WeaponSO> weapons;

	public WeaponSO GetWeaponSOById(int id)
	{
		return weapons[id];
	}
}
