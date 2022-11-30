using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
	RangedWeaponSO _weaponSO;

	Dictionary<string, Stack<Bullet>> _ammoPool = new Dictionary<string, Stack<Bullet>>();

	Coroutine crFireMain;
	Coroutine crFireSub;
	public override void Init(MainCharacter root, WeaponSO weaponSO)
	{
		WeaponSO = weaponSO;
		_weaponSO = (RangedWeaponSO)weaponSO;
		MainCharacter = root;
	}

	public override void Attack(bool start)
	{
		if (_weaponSO.ShootingModes.Length > 0)
		{
			if (start)
			{
				ShootingModeSO shootingMode = _weaponSO.ShootingModes[0];
				switch (shootingMode.selectiveFire)
				{
					case ShootingModeSO.SelectiveFire.FULLY_AUTO:
						if (crFireMain == null)
						{
							crFireMain = StartCoroutine(FireContine(shootingMode));
							//StartCoroutine(FireContine(shootingMode));
						}
						break;
					case ShootingModeSO.SelectiveFire.SEMI_AUTO:
						ShootOnce(shootingMode);
						break;
					case ShootingModeSO.SelectiveFire.BURST:
						StartCoroutine(Burst(shootingMode));
						break;
				}
			}
			else
			{
				if (crFireMain != null)
				{
					StopCoroutine(crFireMain);
					crFireMain = null;
				}
			}
		}
	}

	public override void AttackSub(bool start)
	{
		if (_weaponSO.ShootingModes.Length > 1)
		{
			if (start)
			{
				ShootingModeSO shootingMode = _weaponSO.ShootingModes[1];
				switch (shootingMode.selectiveFire)
				{
					case ShootingModeSO.SelectiveFire.FULLY_AUTO:
						if (crFireSub == null)
						{
							crFireSub = StartCoroutine(FireContine(shootingMode));
						}
						break;
					case ShootingModeSO.SelectiveFire.SEMI_AUTO:
						ShootOnce(shootingMode);
						break;
					case ShootingModeSO.SelectiveFire.BURST:
						StartCoroutine(Burst(shootingMode));
						break;
				}
			}
			else
			{
				if (crFireSub != null)
				{
					StopCoroutine(crFireSub);
				}
			}
		}
	}
	public override void Reload()
	{

	}
	IEnumerator FireContine(ShootingModeSO shootingMode)
	{
		ShootOnce(shootingMode);
		while (true)
		{
			yield return new WaitForSeconds(shootingMode.shootingIdle);
			ShootOnce(shootingMode);
		}
	}
	IEnumerator Burst(ShootingModeSO shootingMode)
	{
		ShootOnce(shootingMode);
		for (int i = 0; i < shootingMode.shootsPerBurst - 1; i++)
		{
			yield return new WaitForSeconds(shootingMode.burstIdle);
			ShootOnce(shootingMode);
		}
	}
	void ShootOnce(ShootingModeSO shootingMode)
	{
		for (int i = 0; i < shootingMode.bulletsPerShoot; i++)
		{
			Bullet bullet = GetBullet(shootingMode.bullet);
			Quaternion dir = transform.rotation;
			Vector3 pos = transform.position;
			if (shootingMode.useAngelOffset)
			{
				bullet.Shoot(dir.eulerAngles + Vector3.up * Random.Range(0f, shootingMode.angelOffset), pos);
			}
			else
			{
				bullet.Shoot(dir, pos);
			}
		}
	}

	public Bullet GetBullet(BulletSO bulletSO)
	{
		if (_ammoPool.TryGetValue(bulletSO.Guid, out var ammoStack))
		{
			if (ammoStack.Count > 0)
			{
				ammoStack.Peek().gameObject.SetActive(true);
				return ammoStack.Pop();
			}
		}
		else
		{
			_ammoPool.Add(bulletSO.Guid, new Stack<Bullet>());
		}
		Bullet bullet = GameObject.Instantiate<GameObject>(bulletSO.Prefab).GetComponent<Bullet>();
		bullet.fromWeapon = this;
		bullet.bulletSO = bulletSO;
		return bullet;
	}
	public void RecycleAmmo(Bullet bullet)
	{
		bullet.gameObject.SetActive(false);
		_ammoPool[bullet.bulletSO.Guid].Push(bullet);
	}


}
