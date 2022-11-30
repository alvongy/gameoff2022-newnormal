public class MeleeWeapon : Weapon
{
	MeleeWeaponSO _weaponSO;
	public override void Init(MainCharacter root, WeaponSO weaponSO)
	{
		WeaponSO = weaponSO;
		_weaponSO = (MeleeWeaponSO)weaponSO;
	}

	public override void Attack(bool start)
	{
		InMeleeingMain = start;
	}

	public override void AttackSub(bool start)
	{
		InMeleeingSub = start;
	}
	public override void Reload()
	{

	}
}
