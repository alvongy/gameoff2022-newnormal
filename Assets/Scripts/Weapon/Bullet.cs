using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[NonSerialized] public BulletSO bulletSO;
	[NonSerialized] public RangedWeapon fromWeapon;

	Rigidbody rb;

	Coroutine crRecycle;
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	public void Shoot(Quaternion dir, Vector3 pos)
	{
		transform.rotation = dir;
		transform.position = pos;
		rb.velocity = transform.TransformDirection(Vector3.forward * bulletSO.BulletSpeed);
		RecycleCountDown();
	}
	public void Shoot(Vector3 dir, Vector3 pos)
	{
		transform.rotation = Quaternion.Euler(dir);
		transform.position = pos;
		rb.velocity = transform.TransformDirection(Vector3.forward * bulletSO.BulletSpeed);
		RecycleCountDown();
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Enemy"))
		{
			collision.transform.GetComponent<Damageable>().ReceiveAnAttack(10);
#if UNITY_EDITOR
			Debug.Log("Hit");
#endif
		}
		Recycle();
	}
	void RecycleCountDown()
	{
		crRecycle = StartCoroutine(RecycleCoroutine());
	}
	IEnumerator RecycleCoroutine()
	{
		yield return new WaitForSeconds(3f);
		Recycle();
	}

	private void Recycle()
	{
		if (crRecycle != null)
		{
			StopCoroutine(crRecycle);
			crRecycle = null;
		}
		fromWeapon.RecycleAmmo(this);
	}
}
