using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	private MainCharacter mainCharacter;
	public MainCharacter MainCharacter { get => mainCharacter; set => mainCharacter = value; }

	public WeaponSO WeaponSO { get; set; }
	public bool InMeleeingMain = false;
	public bool InMeleeingSub = false;
	public bool InFireMain = false;
	public bool InFireSub= false;

	public abstract void Init(MainCharacter root, WeaponSO weaponSO);
	public abstract void Attack(bool start);
	public abstract void AttackSub(bool start);
	public abstract void Reload();
}
