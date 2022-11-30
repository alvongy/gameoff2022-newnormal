using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	[SerializeField] MainCharacter mainCharacter;
	[SerializeField] Transform weaponObjRoot;
	[SerializeField] ArmorySO armory;
	[SerializeField] InputReader inputReader;

	List<Weapon> _weapons = new List<Weapon>();
	Weapon _inHoldingWeapon = null;
	int _inHoldingIdx = 0;
	public Weapon InHoldingWeapon
	{
		get => _inHoldingWeapon;
	}
	public bool inMeleeingMain = false;
	public bool inMeleeingSub = false;
	public bool inFireMain = false;
	public bool inFireSub = false;

	public int InHoldingIdx
	{
		get => _inHoldingIdx;
		set
		{
			if (value == _inHoldingIdx)
			{
				return;
			}
			if (_inHoldingIdx != -1)
			{
				UnholdWeapon(_inHoldingIdx);
			}
			if (value == -1)
			{
				_inHoldingWeapon = null;
			}
			else
			{
				_inHoldingWeapon = _weapons[value];
				HoldWeapon(value);
			}
			_inHoldingIdx = value;
		}
	}
	private void OnEnable()
	{
		inputReader.WeaponFirst += () => SwitchWeapon(0);
		inputReader.WeaponSecond += () => SwitchWeapon(1);
		inputReader.LoopSwitchWeapon += LoopSwitchWeapon;
	}
	private void OnDisable()
	{
		inputReader.WeaponFirst -= () => SwitchWeapon(0);
		inputReader.WeaponSecond -= () => SwitchWeapon(1);
		inputReader.LoopSwitchWeapon -= LoopSwitchWeapon;
	}
	public void LoopSwitchWeapon(bool next = true)
	{
		if (_weapons.Count < 2)
		{
			return;
		}
		SwitchWeapon((InHoldingIdx + 1) % _weapons.Count);
	}
	public void SwitchWeapon(int idx)
	{
		if (idx < _weapons.Count)
		{
			InHoldingIdx = idx;
		}
	}
	public void EnableMelee()
	{

	}
	public void DisableMelee()
	{

	}
	public void ConsumeAttackInput()
	{
		inMeleeingMain = false;
		inMeleeingSub = false;
	}
	public void OnEventMeleeMain(bool start)
	{
		inMeleeingMain = start;
	}
	public void OnEventMeleeSub(bool start)
	{
		inMeleeingSub = start;
	}
	public void HoldWeapon(int weaponIdx)
	{
		Weapon weapon = _weapons[weaponIdx];
		inputReader.AttackMainEvent += weapon.Attack;
		inputReader.AttackSubEvent += weapon.AttackSub;
		inputReader.ReloadEvent += weapon.Reload;
		if (weapon.WeaponSO.WeaponType == WeaponSO.WeaponTypeEnum.MELEE)
		{
			inputReader.AttackMainEvent += OnEventMeleeMain;
			inputReader.AttackSubEvent += OnEventMeleeSub;
		}
		weapon.gameObject.SetActive(true);
	}
	public void UnholdWeapon(int weaponIdx)
	{
		Weapon weapon = _weapons[weaponIdx];
		inputReader.AttackMainEvent -= weapon.Attack;
		inputReader.AttackSubEvent -= weapon.AttackSub;
		inputReader.ReloadEvent -= weapon.Reload;
		if (weapon.WeaponSO.WeaponType == WeaponSO.WeaponTypeEnum.MELEE)
		{
			inputReader.AttackMainEvent -= OnEventMeleeMain;
			inputReader.AttackSubEvent -= OnEventMeleeSub;
		}
		weapon.gameObject.SetActive(false);
	}
	public void AddWeapon(WeaponSO weaponSO)
	{
		GameObject wp = Instantiate(weaponSO.Prefab, weaponObjRoot);
		Weapon weapon;

		switch (weaponSO.WeaponType)
		{
			case WeaponSO.WeaponTypeEnum.MELEE:
				weapon = wp.AddComponent<MeleeWeapon>();
				break;
			case WeaponSO.WeaponTypeEnum.RANGED:
				weapon = wp.AddComponent<RangedWeapon>();
				break;
			case WeaponSO.WeaponTypeEnum.PLACED:
				return;
			case WeaponSO.WeaponTypeEnum.ASSISTANCE:
				return;
			default:
				return;
		}
		weapon.Init(mainCharacter, weaponSO);
		_weapons.Add(weapon);
		SwitchWeapon(_weapons.Count - 1);
	}
#if UNITY_EDITOR
	[ContextMenu("Add Weapon test")]
	public void AddWeaponTest()
	{
		AddWeapon(armory.GetWeaponSOById(0));
	}
	[ContextMenu("Add Weapon test2")]
	public void AddWeaponTest2()
	{
		AddWeapon(armory.GetWeaponSOById(1));
	}
#endif
}
